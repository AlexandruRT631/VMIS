import string
import requests
from bs4 import BeautifulSoup
import json
from io import BytesIO
import random
import re
from googletrans import Translator


def translate_text(text, dest_language='en'):
    translator = Translator()
    return translator.translate(str(text), dest=dest_language).text


# This file contains on each line a link to a listing from Autovit.
with open("URLs.txt", "r") as file:
    URLs = [line.strip() for line in file.readlines()]

outputFile = open("output.txt", "w")
outputErrorFile = open("outputErrors.txt", "w")

users = requests.get("http://localhost:5213/api/User").json()
users = list(filter(lambda x: x["role"] == 2 or x["role"] == 3, users))

for URL in URLs:
    # Opening the page and obtaining what the tag <script> contains, that being the details of the listing.
    page = requests.get(URL)
    soup = BeautifulSoup(page.content, "html.parser")
    page = soup.find('script', attrs={"id": "__NEXT_DATA__"})

    # Checking if all the fields we will need are available.
    json_data = json.loads(page.string)
    if ("props" not in json_data
            or "pageProps" not in json_data["props"]
            or "advert" not in json_data["props"]["pageProps"]
            or "details" not in json_data["props"]["pageProps"]["advert"]):
        outputErrorFile.write(f"Listing from URL {URL} is missing listing_details. Continuing to next Listing.\n")
        continue
    if ("equipment" not in json_data["props"]["pageProps"]["advert"]
            or len(json_data["props"]["pageProps"]["advert"]["equipment"]) == 0):
        outputErrorFile.write(f"Listing from URL {URL} is missing equipment. Continuing to next Listing.\n")
        continue
    if ("description" not in json_data["props"]["pageProps"]["advert"] or str(json_data["props"]["pageProps"]["advert"]["description"]) == ""):
        outputErrorFile.write(f"Listing from URL {URL} is missing description. Continuing to next Listing.\n")
        continue
    if ("images" not in json_data["props"]["pageProps"]["advert"]
            or "photos" not in json_data["props"]["pageProps"]["advert"]["images"]
            or len(json_data["props"]["pageProps"]["advert"]["images"]["photos"]) == 0):
        outputErrorFile.write(f"Listing from URL {URL} is missing images. Continuing to next Listing.\n")
        continue
    details = json_data["props"]["pageProps"]["advert"]["details"]
    listing_details = {detail["key"]: detail["value"] for detail in details}
    print(listing_details)
    equipments = json_data["props"]["pageProps"]["advert"]["equipment"]
    listing_equipments = []
    for equipment in equipments:
        listing_equipments.extend({value["label"] for value in equipment["values"]})
    print(listing_equipments)
    images = json_data["props"]["pageProps"]["advert"]["images"]["photos"]
    listing_images = []
    for image in images:
        listing_images.append(image["url"])
    print(listing_images)
    if ("model" not in listing_details
            or "make" not in listing_details
            or "year" not in listing_details
            or "body_type" not in listing_details
            or "door_count" not in listing_details
            or "engine_capacity" not in listing_details
            or "engine_power" not in listing_details
            or "fuel_type" not in listing_details
            or "transmission" not in listing_details
            or "gearbox" not in listing_details
            or "color" not in listing_details
            or "mileage" not in listing_details):
        outputErrorFile.write(
            f"Listing from URL {URL} is missing some details in listing_details. Continuing to next Listing.\n"
        )
        continue

    # Obtaining the car from de database. We assume it already exists.
    models = requests.get("http://localhost:5019/api/Model").json()
    model = None
    for m in models:
        if m["name"] == listing_details["model"] and m["make"]["name"] == listing_details["make"]:
            model = m
            break
    cars = requests.get("http://localhost:5019/api/Car/getCarByModelYear?modelId=" + str(model["id"]) + "&year=" +
                        listing_details["year"]).json()
    if len(cars) == 0:
        outputErrorFile.write(f"Car from URL {URL} not found in database.\n")
        continue
    car = None
    if len(cars) == 1:
        car = cars[0]
    elif "generation" in listing_details:
        startYear = re.search(r'\[(\d{4})', listing_details["generation"]).group(1)
        for c in cars:
            if c["startYear"] == int(startYear):
                car = c
                break
    if car is None:
        outputErrorFile.write(f"Car from URL {URL} not found in database.\n")
        continue
    print(car)

    # Obtaining the category, and adding it to the possibleCategories if it doesn't have it, or creating a new one.
    category = None
    for possibleCategory in car["possibleCategories"]:
        if possibleCategory["name"] == listing_details["body_type"]:
            category = possibleCategory
            break
    if category is None:
        categories = requests.get("http://localhost:5019/api/Category").json()
        for c in categories:
            if c["name"] == listing_details["body_type"]:
                category = c
                car["possibleCategories"].append(category)
                requests.put("http://localhost:5019/api/Car/", json=car)
                outputFile.write(
                    f"possibleCategories for car with id {car['id']} was updated with category {{id: {category['id']}, name: {category['name']}}}.\n")
                break
    if category is None:
        response = requests.post("http://localhost:5019/api/Category", json={"name": listing_details["body_type"]})
        category = response.json()
        car["possibleCategories"].append(category)
        requests.put("http://localhost:5019/api/Car/", json=car)
        outputFile.write(f"A new category has been added {{id: {category['id']}, name: {category['name']}}}.\n")
    print(category)

    # Obtaining the doorType, and adding it to the possibleDoorTypes if it doesn't have it, or creating a new one.
    doorType = None
    for possibleDoorType in car["possibleDoorTypes"]:
        if possibleDoorType["name"] == listing_details["door_count"]:
            doorType = possibleDoorType
            break
    if doorType is None:
        doorTypes = requests.get("http://localhost:5019/api/DoorType").json()
        for d in doorTypes:
            if d["name"] == listing_details["door_count"]:
                doorType = d
                car["possibleDoorTypes"].append(doorType)
                requests.put("http://localhost:5019/api/Car/", json=car)
                outputFile.write(
                    f"possibleDoorTypes for car with id {car['id']} was updated with door type {{id: {doorType['id']}, name: {doorType['name']}}}.\n")
                break
    if doorType is None:
        response = requests.post("http://localhost:5019/api/DoorType", json={"name": listing_details["door_count"]})
        doorType = response.json()
        car["possibleDoorTypes"].append(doorType)
        requests.put("http://localhost:5019/api/Car/", json=car)
        outputFile.write(f"A new door type has been added {{id: {doorType['id']}, name: {doorType['name']}}}.\n")
    print(doorType)

    # Obtaining the engine, and adding it to the possibleEngines if it doesn't have it, or creating a new one.
    # Also, the engine added either to the list of possibleEngines or created needs to be checked, this engine
    # assignment was done only to be able to add the listing and the engine might be wrong.
    engine = None
    for possibleEngine in car["possibleEngines"]:
        if (possibleEngine["displacement"] == int(
                listing_details["engine_capacity"].replace(" cm3", "").replace(" ", ""))
            and possibleEngine["power"] == int(listing_details["engine_power"].replace(" CP", "").replace(" ", ""))) \
                and possibleEngine["fuel"]["name"] == listing_details["fuel_type"]:
            engine = possibleEngine
            break
    if engine is None:
        engines = requests.get("http://localhost:5019/api/Engine").json()
        for e in engines:
            if (e["displacement"] == int(listing_details["engine_capacity"].replace(" cm3", "").replace(" ", ""))
                and e["power"] == int(listing_details["engine_power"].replace(" CP", "").replace(" ", ""))) \
                    and e["fuel"]["name"] == listing_details["fuel_type"]:
                engine = e
                car["possibleEngines"].append(engine)
                requests.put("http://localhost:5019/api/Car/", json=car)
                outputFile.write(
                    f"An engine not in the list of possible engines was used, with id: {str(engine["id"])}. Please check if the engine is the right one. Engine was also added to possibleEngines for car with id {str(car["id"])}.\n")
                break
    if engine is None:
        fuel = None
        fuels = requests.get("http://localhost:5019/api/Fuel").json()
        for f in fuels:
            if f["name"] == listing_details["fuel_type"]:
                fuel = f
                break
        if fuel is None:
            response = requests.post("http://localhost:5019/api/Fuel", json={"name": listing_details["fuel_type"]})
            fuel = response.json()
            outputFile.write(
                f"A new fuel has been added with id: {str(fuel['id'])}. Please update the name. Fuel was also added to the engine.\n")

        response = requests.post("http://localhost:5019/api/Engine", json={
            "make": {
                "id": car["model"]["make"]["id"]
            },
            "engineCode": 'AAA' + ''.join(random.choices(string.ascii_uppercase + string.digits, k=5)),
            "displacement": int(listing_details["engine_capacity"].replace(" cm3", "").replace(" ", "")),
            "fuel": {
                "id": fuel["id"]
            },
            "power": int(listing_details["engine_power"].replace(" CP", "").replace(" ", "")),
            "torque": 1
        })
        engine = response.json()
        car["possibleEngines"].append(engine)
        requests.put("http://localhost:5019/api/Car/", json=car)
        outputFile.write(
            f"A new engine has been added with id: {str(engine["id"])}. Please update the make, engineCode, and torque. Engine was also added to possibleEngines for car with id {str(car["id"])}.\n")
    print(engine)

    # Obtaining the traction, and adding it to the possibleTractions if it doesn't have it, or creating a new one.
    traction = None
    for possibleTraction in car["possibleTractions"]:
        if possibleTraction["name"] == listing_details["transmission"]:
            traction = possibleTraction
            break
    if traction is None:
        tractions = requests.get("http://localhost:5019/api/Traction").json()
        for t in tractions:
            if t["name"] == listing_details["transmission"]:
                traction = t
                car["possibleTractions"].append(traction)
                requests.put("http://localhost:5019/api/Car/", json=car)
                outputFile.write(
                    f"possibleTractions for car with id {car['id']} was updated with traction {{id: {traction['id']}, name: {traction['name']}}}.\n")
                break
    if traction is None:
        response = requests.post("http://localhost:5019/api/Traction", json={"name": listing_details["transmission"]})
        traction = response.json()
        car["possibleTractions"].append(traction)
        requests.put("http://localhost:5019/api/Car/", json=car)
        outputFile.write(f"A new traction has been added {{id: {traction['id']}, name: {traction['name']}}}.\n")
    print(traction)

    # Obtaining the transmission, and adding it to the possibleTransmissions if it doesn't have it,
    # or creating a new one.
    transmission = None
    for possibleTransmission in car["possibleTransmissions"]:
        if possibleTransmission["name"] == listing_details["gearbox"]:
            transmission = possibleTransmission
            break
    if transmission is None:
        transmissions = requests.get("http://localhost:5019/api/Transmission").json()
        for t in transmissions:
            if t["name"] == listing_details["gearbox"]:
                transmission = t
                car["possibleTransmissions"].append(transmission)
                requests.put("http://localhost:5019/api/Car/", json=car)
                outputFile.write(
                    f"possibleTransmissions for car with id {car['id']} was updated with transmission {{id: {transmission['id']}, name: {transmission['name']}}}.\n")
                break
    if transmission is None:
        response = requests.post("http://localhost:5019/api/Transmission", json={"name": listing_details["gearbox"]})
        transmission = response.json()
        car["possibleTransmissions"].append(transmission)
        requests.put("http://localhost:5019/api/Car/", json=car)
        outputFile.write(
            f"A new transmission has been added {{id: {transmission['id']}, name: {transmission['name']}}}.\n")
    print(transmission)

    # Obtaining the color, or creating a new one.
    color = None
    colors = requests.get("http://localhost:5019/api/Color").json()
    for c in colors:
        if c["name"] == listing_details["color"]:
            color = c
            break
    if color is None:
        response = requests.post("http://localhost:5019/api/Color", json={
            "name": listing_details["color"],
            "hexCode": "#000000"
        })
        color = response.json()
        outputFile.write(
            f"New color added: {listing_details["color"]} with id: {str(color["id"])}. Please update the hex code.\n")
    print(color)

    # Obtaining the features, and creating those that doesn't exist.
    list_feature = requests.get("http://localhost:5019/api/Feature").json()
    features = []
    for equipment in listing_equipments:
        feature = None
        for f in list_feature:
            if f["name"] == equipment:
                feature = f
                break
        if feature is None:
            response = requests.post("http://localhost:5019/api/Feature", json={"name": equipment})
            feature = response.json()
            outputFile.write(
                f"A new exterior feature has been added with {{id: {str(feature["id"])}, name: {feature["name"]}}}.\n")
        features.append({
            "Id": feature["id"]
        })

    # Obtaining the description, and cleaning it.
    description = (str(json_data["props"]["pageProps"]["advert"]["description"])
                   .replace(r"\u003c", "<")
                   .replace(r"\u003e", ">"))
    # Removing any div inside the description.
    description = re.sub(r"<div.*?>.*?</div>", "", description, flags=re.DOTALL)
    # Replacing <p></p> and <br> with \n.
    description = re.sub(r"<p>|</p>", "\n", description)
    description = re.sub(r"<br\s*/?>", "\n", description)
    # Replacing lines
    description = re.sub(r"</?ul>", "", description)
    description = re.sub(r"<li>", "-", description)
    description = re.sub(r"</li>", "\n", description)

    # Creating the new listing object.
    newListing = {
        "Car": {
            "Id": car["id"]
        },
        "Category": {
            "Id": category["id"]
        },
        "DoorType": {
            "Id": doorType["id"]
        },
        "Engine": {
            "Id": engine["id"]
        },
        "Traction": {
            "Id": traction["id"]
        },
        "Transmission": {
            "Id": transmission["id"]
        },
        "Color": {
            "Id": color["id"]
        },
        "Features": features,
        "Mileage": int(listing_details["mileage"].replace(" km", "").replace(" ", "")),
        "Price": int(float(json_data["props"]["pageProps"]["advert"]["price"]["value"])),
        "Title": json_data["props"]["pageProps"]["advert"]["title"],
        "Description": translate_text(description),
        "SellerId": random.choice(users)["id"],
        "Year": int(listing_details["year"]),
        "CreatedAt": json_data["props"]["pageProps"]["advert"]["createdAt"],
        "IsSold": random.random() >= 0.8
    }
    print(newListing)

    # Creating the list with the extracted images.
    image_files = []
    for image in listing_images:
        response = requests.get(image)
        if response.status_code == 200:
            content_type = response.headers['Content-Type']
            extension = content_type.split('/')[-1]
            if not extension.startswith('image'):
                extension = 'jpeg'
            filename = image.split("/")[-1].split('?')[0]
            if not filename.endswith(extension):
                filename += f".{extension}"
            image_files.append(
                ('images', (filename, BytesIO(response.content), content_type))
            )

    # Posting the listing.
    multipart_data = {
        "listingDto": (None, json.dumps(newListing), 'application/json')
    }
    response = requests.post(
        "http://localhost:5019/api/Listing",
        headers={
            "Accept": "application/json"
        },
        files=image_files + list(multipart_data.items())
    )
    print(response.status_code)
    print(response.json())
    if response.status_code == 200:
        outputFile.write(f"Listing from URL {URL} added successfully.\n")

    print("\n\n")
