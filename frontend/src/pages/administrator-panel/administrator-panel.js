import React, {useEffect, useState} from "react";
import {getUserId} from "../../common/token";
import {getUserDetails} from "../../api/user-api";
import {
    Autocomplete,
    Box,
    Grid,
    TextField,
    Typography
} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import CategoryPanel from "./category-panel";
import ColorPanel from "./color-panel";
import DoorTypePanel from "./door-type-panel";
import FeaturePanel from "./feature-panel";
import FuelPanel from "./fuel-panel";
import MakePanel from "./make-panel";
import TractionPanel from "./traction-panel";
import TransmissionPanel from "./transmission-panel";
import ModelPanel from "./model-panel";
import EnginePanel from "./engine-panel";
import CarPanel from "./car-panel";
import UserPanel from "./user-panel";

const options = [
    "Category",
    "Color",
    "Door Type",
    "Feature",
    "Fuel",
    "Make",
    "Traction",
    "Transmission",
    "Model",
    "Engine",
    "Car",
    "User",
];

const AdministratorPanel = () => {
    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState(null);
    const [selectedOption, setSelectedOption] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            const fetchedUserId = getUserId();
            const fetchedUser = await getUserDetails(fetchedUserId);
            setUser(fetchedUser);
        }

        fetchData()
            .then(() => setLoading(false))
            .catch(console.error);
    }, []);

    if (loading) {
        return;
    }

    if (user === null || user.role !== 'Admin') {
        return (
            <Typography variant={"h4"}>You don't have permission to access this page.</Typography>
        );
    }

    return (
        <Box>
            <Typography variant={"h2"}>Administrator Panel</Typography>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={2}
                mb={2}
            >
                <Grid item xs={12}>
                    <CommonPaper title={"Select"}>
                        <Autocomplete
                            renderInput={(params) => <TextField {...params} label="Select an option to manage:" />}
                            options={options.sort((a, b) => -b.localeCompare(a))}
                            onChange={(event, newValue) => setSelectedOption(newValue)}
                        />
                    </CommonPaper>
                </Grid>
                {selectedOption && selectedOption === "Category" && <CategoryPanel />}
                {selectedOption && selectedOption === "Color" && <ColorPanel />}
                {selectedOption && selectedOption === "Door Type" && <DoorTypePanel />}
                {selectedOption && selectedOption === "Feature" && <FeaturePanel />}
                {selectedOption && selectedOption === "Fuel" && <FuelPanel />}
                {selectedOption && selectedOption === "Make" && <MakePanel />}
                {selectedOption && selectedOption === "Traction" && <TractionPanel />}
                {selectedOption && selectedOption === "Transmission" && <TransmissionPanel />}
                {selectedOption && selectedOption === "Model" && <ModelPanel />}
                {selectedOption && selectedOption === "Engine" && <EnginePanel />}
                {selectedOption && selectedOption === "Car" && <CarPanel />}
                {selectedOption && selectedOption === "User" && <UserPanel />}
            </Grid>
        </Box>
    );
}

export default AdministratorPanel;