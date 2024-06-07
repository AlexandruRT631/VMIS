import {Box, Grid} from "@mui/material";
import ListingCreateCar from "./listing-create-car";
import {useEffect, useState} from "react";
import ListingCreateTechnical from "./listing-create-technical";
import ListingCreateEquipment from "./listing-create-equipment";
import ListingCreateGeneration from "./listing-create-generation";
import ListingCreateFinalDetails from "./listing-create-final-details";
import {createListing} from "../../api/listing-api";

const ListingCreate = () => {
    const [possibleCars, setPossibleCars] = useState([]);
    const [year, setYear] = useState(null);
    const [car, setCar] = useState(null);
    const [technical, setTechnical] = useState(null);
    const [equipment, setEquipment] = useState(null);
    const [finalDetails, setFinalDetails] = useState(null);

    useEffect(() => {
        if (possibleCars.length === 1) {
            setCar(possibleCars[0]);
        }
        else {
            setCar(null);
        }
    }, [possibleCars]);

    useEffect(() => {
        if (car === null) {
            setTechnical(null);
        }
    }, [car]);

    useEffect(() => {
        if (technical === null) {
            setEquipment(null);
        }
    }, [technical]);

    useEffect(() => {
        if (equipment === null) {
            setFinalDetails(null);
        }
    }, [equipment]);

    useEffect(() => {
        if (finalDetails !== null) {
            createListing({
                Car: {Id: car.id},
                Category: {Id: technical.category.id},
                DoorType: {Id: technical.doorType.id},
                Engine: {Id: technical.engine.id},
                Traction: {Id: technical.traction.id},
                Transmission: {Id: technical.transmission.id},
                ExteriorColor: {Id: equipment.exteriorColor.id},
                InteriorColor: {Id: equipment.interiorColor.id},
                FeaturesExterior: equipment.featuresExterior.map(feature => ({Id: feature.id})),
                FeaturesInterior: equipment.featuresInterior.map(feature => ({Id: feature.id})),
                Mileage: finalDetails.mileage,
                Price: finalDetails.price,
                Title: finalDetails.title,
                SellerId: finalDetails.sellerId,
                Year: year,
                CreatedAt: new Date(),
            }, finalDetails.images)
                .then(console.log)
                .catch(console.error);
        }
    }, [finalDetails]);

    console.log(possibleCars);
    console.log(year);
    console.log(car);
    console.log(technical);
    console.log(equipment);
    console.log(finalDetails);

    return (
        <Box>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={4}
                mb={4}
            >
                <Grid item xs={12}>
                    <ListingCreateCar setPossibleCars={setPossibleCars} setYear={setYear} possibleCars={possibleCars}/>
                </Grid>
                {possibleCars.length > 1?
                    <Grid item xs={12}>
                        <ListingCreateGeneration setCar={setCar} possibleCars={possibleCars} car={car}/>
                    </Grid>
                    : null
                }
                {car !== null?
                    <Grid item xs={12}>
                        <ListingCreateTechnical setTechnical={setTechnical} car={car} />
                    </Grid>
                    : null
                }
                {technical !== null?
                    <Grid item xs={12}>
                        <ListingCreateEquipment setEquipment={setEquipment}/>
                    </Grid>
                    : null
                }
                {equipment !== null?
                    <Grid item xs={12}>
                        <ListingCreateFinalDetails setFinalDetails={setFinalDetails}/>
                    </Grid>
                    : null
                }
            </Grid>
        </Box>
    );
}

export default ListingCreate;