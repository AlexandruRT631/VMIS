import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useRef, useState} from "react";
import {getUserId} from "../../common/token";
import {Box, Grid, Typography} from "@mui/material";
import {getListingById, updateListing} from "../../api/listing-api";
import ListingSelectCar from "../../common/listing/listing-select-car";
import ListingSelectGeneration from "../../common/listing/listing-select-generation";
import {getCarByModelYear} from "../../api/car-api";
import ListingSelectTechnical from "../../common/listing/listing-select-technical";
import ListingSelectEquipment from "../../common/listing/listing-select-equipment";
import ListingSelectFinalDetails from "../../common/listing/listing-select-final-details";
import {getUserDetails} from "../../api/user-api";

const ListingModify = () => {
    const { id } = useParams();
    const [loading, setLoading] = useState(true);
    const [userId, setUserId] = useState(null);
    const [userRole, setUserRole] = useState(null);
    const [listing, setListing] = useState(null);
    const [possibleCars, setPossibleCars] = useState([]);
    const [year, setYear] = useState(null);
    const [car, setCar] = useState(null);
    const [technical, setTechnical] = useState(null);
    const [equipment, setEquipment] = useState(null);
    const [finalDetails, setFinalDetails] = useState(null);
    const [firstLoad, setFirstLoad] = useState(true);
    const firstLoadRef = useRef(firstLoad);
    const navigate = useNavigate();
    const [validationErrors, setValidationErrors] = useState({
        car: '',
        technical: '',
        equipment: '',
        year: '',
    })

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedUserId = getUserId()
                setUserId(fetchedUserId);
                if (fetchedUserId) {
                    const user = await getUserDetails(fetchedUserId);
                    setUserRole(user.role);
                }
                const fetchedListing = await getListingById(id);
                setListing(fetchedListing);
                const fetchedPossibleCars = await getCarByModelYear(fetchedListing.car.model.id, fetchedListing.year);
                setPossibleCars(fetchedPossibleCars);
                setYear(fetchedListing.year);
                setCar(fetchedListing.car);
                setTechnical({
                    category: fetchedListing.category,
                    doorType: fetchedListing.doorType,
                    engine: fetchedListing.engine,
                    transmission: fetchedListing.transmission,
                    traction: fetchedListing.traction
                });
            } catch (error) {
                console.error(error);
            }
        };
        fetchData()
            .then(() => setLoading(false));
    }, [id]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (possibleCars.length === 1) {
                setCar(possibleCars[0]);
            }
            else {
                setCar(null);
            }
        }
    }, [possibleCars]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (car === null) {
                setTechnical(null);
            }
        }
    }, [car]);

    useEffect(() => {
        if (technical) {
            setFirstLoad(false);
        }
    }, [technical]);

    useEffect(() => {
        if (
            car != null
            && technical !== null
            && equipment !== null
            && finalDetails !== null
            && year !== null
        ) {
            setValidationErrors({
                car: '',
                technical: '',
                equipment: '',
                year: '',
            })
            updateListing({
                Id: listing.id,
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
                SellerId: listing.sellerId,
                Year: year,
                CreatedAt: listing.createdAt,
            }, finalDetails.images)
                .then(r => {
                    console.log(r);
                    navigate(`/listing/${r.id}`);
                })
                .catch(console.error);
        }
        else {
            setValidationErrors({
                car: car ? '' : 'Car must be selected',
                technical: technical ? '' : 'Technical details must be selected',
                equipment: equipment ? '' : 'Equipment must be selected',
                year: year ? '' : 'Year must be selected',
            });
        }
    }, [car, technical, equipment, finalDetails, year, listing, navigate]);

    console.log(car)
    console.log(technical)
    console.log(equipment)
    console.log(finalDetails)
    console.log(year)
    console.log("------")

    if (loading) {
        return;
    }

    if (!userId) {
        return (
            <Typography variant={"h4"}>You must be logged in to modify a listing</Typography>
        );
    }

    if (userRole !== "Admin" && listing.seller.id !== parseInt(userId)) {
        return (
            <Typography variant={"h4"}>You must be the seller to modify this listing</Typography>
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
                    <ListingSelectCar year={year} listing={listing} setPossibleCars={setPossibleCars} setYear={setYear} possibleCars={possibleCars}/>
                </Grid>
                {possibleCars.length > 1?
                    <Grid item xs={12}>
                        <ListingSelectGeneration listing={listing} setCar={setCar} possibleCars={possibleCars} car={car}/>
                    </Grid>
                    : null
                }
                {car !== null?
                    <Grid item xs={12}>
                        <ListingSelectTechnical listing={listing} technical={technical} setTechnical={setTechnical} car={car} />
                    </Grid>
                    : null
                }
                <Grid item xs={12}>
                    <ListingSelectEquipment listing={listing} setEquipment={setEquipment}/>
                </Grid>
                <Grid item xs={12}>
                    <ListingSelectFinalDetails listing={listing} setFinalDetails={setFinalDetails}/>
                </Grid>
                {validationErrors.car && (
                    <Grid item xs={12}>
                        <Typography variant={"h6"} color={"error"}>{validationErrors.car}</Typography>
                    </Grid>
                )}
                {validationErrors.technical && (
                    <Grid item xs={12}>
                        <Typography variant={"h6"} color={"error"}>{validationErrors.technical}</Typography>
                    </Grid>
                )}
                {validationErrors.equipment && (
                    <Grid item xs={12}>
                        <Typography variant={"h6"} color={"error"}>{validationErrors.equipment}</Typography>
                    </Grid>
                )}
                {validationErrors.year && (
                    <Grid item xs={12}>
                        <Typography variant={"h6"} color={"error"}>{validationErrors.year}</Typography>
                    </Grid>
                )}
            </Grid>
        </Box>
    )
}

export default ListingModify;