import React, {useEffect, useState} from "react";
import {getAllMakes} from "../../api/make-api";
import {getAllModelsByMakeId} from "../../api/model-api";
import {Autocomplete, Box, Button, Grid, TextField} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import {getCarByMakeModelYear} from "../../api/car-api";

const ListingCreateCar = ({setCar}) => {
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
        setCar(null);
        getAllModelsByMakeId(newMake.id)
            .then(setModels)
            .catch(console.error);
    }

    const changeModel = (newModel) => {
        setSelectedModel(newModel);
        setSelectedYear('');
        setCar(null);
    }

    const handleYearChange = (event) => {
        const value = event.target.value;
        setSelectedYear(value);
        setCar(null);
        if (value !== '' && (isNaN(value) || value < 1900)) {
            setYearError(true);
        } else {
            setYearError(false);
        }
    }

    const handleNext = () => {
        getCarByMakeModelYear(selectedMake.id, selectedModel.id, selectedYear)
            .then(setCar)
            .catch(console.error);
    }

    return (
        <CommonPaper title={"Car"}>
            <Box style={{ height: '120px', display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}>
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
                                InputProps={{
                                    style: { overflow: 'hidden' }
                                }}
                            />
                            : null
                        }
                    </Grid>
                </Grid>
                <Grid container style={{ flexGrow: 1 }}>
                    <Grid mt={2} item xs={12}>
                        {selectedYear !== '' && !yearError ?
                            <Button variant="contained" onClick={handleNext}>
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