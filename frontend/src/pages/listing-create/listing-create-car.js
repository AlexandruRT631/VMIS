import React, {useEffect, useState} from "react";
import {getAllMakes} from "../../api/make-api";
import {getAllModelsByMakeId} from "../../api/model-api";
import {Autocomplete, Box, Button, Grid, TextField} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import {getCarByMakeModelYear} from "../../api/car-api";

const ListingCreateCar = ({setPossibleCars, setYear, possibleCars}) => {
    const [loading, setLoading] = useState(true);
    const [makes, setMakes] = useState(null);
    const [selectedMake, setSelectedMake] = useState(null);
    const [models, setModels] = useState(null);
    const [selectedModel, setSelectedModel] = useState(null);
    const [selectedYear, setSelectedYear] = useState('');
    const [yearError, setYearError] = useState(false);

    useEffect(() => {
        getAllMakes()
            .then(setMakes)
            .catch(console.error);
        setLoading(false);
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    const changeMake = (newMake) => {
        setSelectedMake(newMake);
        setSelectedModel(null);
        setSelectedYear('');
        setPossibleCars([]);
        setYear(null);
        getAllModelsByMakeId(newMake.id)
            .then(setModels)
            .catch(console.error);
    }

    const changeModel = (newModel) => {
        setSelectedModel(newModel);
        setSelectedYear('');
        setPossibleCars([]);
        setYear(null);
    }

    const handleYearChange = (event) => {
        const value = event.target.value;
        setSelectedYear(value);
        setPossibleCars([]);
        setYear(null);
        if (value !== '' && (isNaN(value) || value < 1900)) {
            setYearError(true);
        } else {
            setYearError(false);
        }
    }

    const handleNext = () => {
        if (possibleCars.length === 0) {
            getCarByMakeModelYear(selectedMake.id, selectedModel.id, selectedYear)
                .then(setPossibleCars)
                .catch(console.error);
            setYear(parseInt(selectedYear, 10));
        }
    }

    return (
        <CommonPaper title={"Car"}>
            <Box>
                <Grid container spacing={2} style={{ flexGrow: 1 }}>
                    <Grid item xs={4}>
                        {makes ?
                            <Autocomplete
                                options={makes}
                                getOptionLabel={(option) => option.name}
                                onChange={(event, newValue) => changeMake(newValue)}
                                value={selectedMake}
                                renderInput={(params) => <TextField {...params} label="Make" />}
                            />
                            : null
                        }
                    </Grid>
                    <Grid item xs={4}>
                        {models ?
                            <Autocomplete
                                options={models}
                                getOptionLabel={(option) => option.name}
                                onChange={(event, newValue) => changeModel(newValue)}
                                value={selectedModel}
                                renderInput={(params) => <TextField {...params} label="Model" />}
                            />
                            : null
                        }
                    </Grid>
                    <Grid item xs={4}>
                        {selectedModel ?
                            <TextField
                                label="Year"
                                type="number"
                                value={selectedYear}
                                onChange={handleYearChange}
                                error={yearError}
                                helperText={yearError ? "Year must be a number greater than or equal to 1900" : ""}
                                fullWidth
                                sx={{
                                    '& input[type=number]::-webkit-outer-spin-button, & input[type=number]::-webkit-inner-spin-button': {
                                        WebkitAppearance: 'none',
                                        margin: 0,
                                    },
                                    '& input[type=number]': {
                                        MozAppearance: 'textfield',
                                    },
                                }}
                            />
                            : null
                        }
                    </Grid>
                </Grid>
                <Grid container style={{ flexGrow: 1 }}>
                    <Grid mt={2} item xs={12}>
                        {selectedYear !== '' && !yearError ?
                            <Button
                                variant="contained"
                                onClick={handleNext}
                                sx={{
                                    '&:focus': {
                                        outline: 'none',
                                    },
                                }}
                            >
                                Next
                            </Button>
                            : null
                        }
                    </Grid>
                </Grid>
            </Box>
        </CommonPaper>
    )
}

export default ListingCreateCar;