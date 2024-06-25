import {Box, Grid, Typography} from "@mui/material";
import ListingSelectCar from "../../common/listing/listing-select-car";
import {useEffect, useState} from "react";
import ListingSelectTechnical from "../../common/listing/listing-select-technical";
import ListingSelectEquipment from "../../common/listing/listing-select-equipment";
import ListingSelectGeneration from "../../common/listing/listing-select-generation";
import ListingSelectFinalDetails from "../../common/listing/listing-select-final-details";
import {createListing} from "../../api/listing-api";
import {useNavigate} from "react-router-dom";
import {getUserId} from "../../common/token";
import {getUserDetails} from "../../api/user-api";

const ListingCreate = () => {
    const [loading, setLoading] = useState(true);
    const [userId, setUserId] = useState(null);
    const [userRole, setUserRole] = useState(null);
    const [possibleCars, setPossibleCars] = useState([]);
    const [year, setYear] = useState(null);
    const [car, setCar] = useState(null);
    const [technical, setTechnical] = useState(null);
    const [equipment, setEquipment] = useState(null);
    const [finalDetails, setFinalDetails] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const fetchedUserId = getUserId();
            setUserId(fetchedUserId);
            if (fetchedUserId) {
                const user = await getUserDetails(fetchedUserId);
                setUserRole(user.role);
            }
        }

        fetchData()
            .then(() => setLoading(false))
            .catch(console.error);
    }, []);

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
                Color: {Id: equipment.color.id},
                Features: equipment.features.map(feature => ({Id: feature.id})),
                Mileage: finalDetails.mileage,
                Price: finalDetails.price,
                Title: finalDetails.title,
                Description: finalDetails.description,
                SellerId: parseInt(userId),
                Year: year,
                CreatedAt: new Date(),
            }, finalDetails.images)
                .then(r => {
                    console.log(r);
                    navigate(`/listing/${r.id}`);
                })
                .catch(console.error);
        }
    }, [finalDetails]);

    console.log(possibleCars);
    console.log(year);
    console.log(car);
    console.log(technical);
    console.log(equipment);
    console.log(finalDetails);
    console.log(userRole);

    if (loading) {
        return;
    }

    if (!userId) {
        return (
            <Typography variant={"h4"}>You must be logged in to create a listing</Typography>
        );
    }

    if (userRole !== "Seller" && userRole !== "Admin") {
        return (
            <Typography variant={"h4"}>You must be a seller to create a listing</Typography>
        );
    }

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
                    <ListingSelectCar setPossibleCars={setPossibleCars} setYear={setYear} possibleCars={possibleCars}/>
                </Grid>
                {possibleCars.length > 1?
                    <Grid item xs={12}>
                        <ListingSelectGeneration setCar={setCar} possibleCars={possibleCars} car={car}/>
                    </Grid>
                    : null
                }
                {car !== null?
                    <Grid item xs={12}>
                        <ListingSelectTechnical setTechnical={setTechnical} car={car} />
                    </Grid>
                    : null
                }
                {technical !== null?
                    <Grid item xs={12}>
                        <ListingSelectEquipment setEquipment={setEquipment}/>
                    </Grid>
                    : null
                }
                {equipment !== null?
                    <Grid item xs={12}>
                        <ListingSelectFinalDetails setFinalDetails={setFinalDetails}/>
                    </Grid>
                    : null
                }
            </Grid>
        </Box>
    );
}

export default ListingCreate;