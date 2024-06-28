import React, {useEffect, useState} from "react";
import {createCar, getAllCars, getCarsByModel, updateCar} from "../../api/car-api";
import {
    Autocomplete,
    Button,
    Dialog, DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    TextField
} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import CommonSubPaper from "../../common/common-sub-paper";
import {getAllMakes} from "../../api/make-api";
import {getAllCategories} from "../../api/category-api";
import {getAllDoorTypes} from "../../api/door-type-api";
import {getAllTractions} from "../../api/traction-api";
import {getAllEngines} from "../../api/engine-api";
import {getAllTransmissions} from "../../api/transmission-api";
import {getAllModelsByMakeId} from "../../api/model-api";
import GridAutocompleteList from "../../common/grid/grid-autocomplete-list";

const CarPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newCar, setNewCar] = useState({
        model: null,
        startYear: '',
        endYear: '',
        possibleCategories: [],
        possibleDoorTypes: [],
        possibleTransmissions: [],
        possibleTractions: [],
        possibleEngines: [],
    });
    const [makes, setMakes] = useState([]);
    const [selectedNewMake, setSelectedNewMake] = useState(null);
    const [selectedModifiedMake, setSelectedModifiedMake] = useState(null);
    const [selectedModifiedModel, setSelectedModifiedModel] = useState(null);
    const [modifiedMake, setModifiedMake] = useState(null);
    const [modifiedMakeModels, setModifiedMakeModels] = useState([]);
    const [firstModifiedModelLoad, setFirstModifiedModelLoad] = useState(true);
    const [newModels, setNewModels] = useState([]);
    const [modifiedModels, setModifiedModels] = useState([]);
    const [modifiedGenerations, setModifiedGenerations] = useState([]);
    const [categories, setCategories] = useState([]);
    const [doorTypes, setDoorTypes] = useState([]);
    const [transmissions, setTransmissions] = useState([]);
    const [tractions, setTractions] = useState([]);
    const [engines, setEngines] = useState([]);
    const [selectedCar, setSelectedCar] = useState(null);
    const [modifiedCar, setModifiedCar] = useState({
        id: null,
        model: null,
        startYear: '',
        endYear: '',
        possibleCategories: [],
        possibleDoorTypes: [],
        possibleTransmissions: [],
        possibleTractions: [],
        possibleEngines: [],
    })
    const [error, setError] = useState({
        newMake: '',
        newModel: '',
        newStartYear: '',
        newEndYear: '',
        newPossibleCategories: '',
        newPossibleDoorTypes: '',
        newPossibleTransmissions: '',
        newPossibleTractions: '',
        newPossibleEngines: '',
        modifiedMake: '',
        modifiedModel: '',
        modifiedStartYear: '',
        modifiedEndYear: '',
        modifiedPossibleCategories: '',
        modifiedPossibleDoorTypes: '',
        modifiedPossibleTransmissions: '',
        modifiedPossibleTractions: '',
        modifiedPossibleEngines: '',
    });
    const [dialogOpen, setDialogOpen] = useState(false);
    const [dialogTitle, setDialogTitle] = useState('');
    const [dialogText, setDialogText] = useState('');
    const [dialogYesAction, setDialogYesAction] = useState(null);
    const [dialogYesDisabled, setDialogYesDisabled] = useState(false);
    const [dialogAcknowledgement, setDialogAcknowledgement] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedMakes = await getAllMakes();
                setMakes(fetchedMakes);
                const fetchedCategories = await getAllCategories();
                setCategories(fetchedCategories);
                const fetchedDoorTypes = await getAllDoorTypes();
                setDoorTypes(fetchedDoorTypes);
                const fetchedTransmissions = await getAllTransmissions();
                setTransmissions(fetchedTransmissions);
                const fetchedTractions = await getAllTractions();
                setTractions(fetchedTractions);
                const fetchedEngines = await getAllEngines();
                setEngines(fetchedEngines);
            }
            catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false))
    }, []);

    useEffect(() => {
        if (selectedNewMake) {
            getAllModelsByMakeId(selectedNewMake.id)
                .then(setNewModels)
                .catch(console.error);
        }
        setNewCar({...newCar, model: null})
    }, [selectedNewMake]);

    useEffect(() => {
        if (selectedModifiedMake) {
            getAllModelsByMakeId(selectedModifiedMake.id)
                .then(setModifiedModels)
                .catch(console.error);
        }
        setSelectedCar(null);
        setModifiedCar({
            id: null,
            model: null,
            startYear: '',
            endYear: '',
            possibleCategories: [],
            possibleDoorTypes: [],
            possibleTransmissions: [],
            possibleTractions: [],
            possibleEngines: [],
        })
        setSelectedModifiedModel(null);
        setModifiedGenerations([]);
    }, [selectedModifiedMake]);

    useEffect(() => {
        if (selectedModifiedModel) {
            getCarsByModel(selectedModifiedModel.id)
                .then(setModifiedGenerations)
                .catch(console.error);
        }
        setSelectedCar(null);
        setModifiedCar({
            id: null,
            model: null,
            startYear: '',
            endYear: '',
            possibleCategories: [],
            possibleDoorTypes: [],
            possibleTransmissions: [],
            possibleTractions: [],
            possibleEngines: [],
        })
    }, [selectedModifiedModel]);

    useEffect(() => {
        setFirstModifiedModelLoad(true);
        setModifiedMake(selectedCar ? selectedCar.model.make : null);
    }, [selectedCar]);

    useEffect(() => {
        if (modifiedMake) {
            getAllModelsByMakeId(modifiedMake.id)
                .then(setModifiedMakeModels)
                .catch(console.error);
        }
        else {
            setModifiedMakeModels([]);
        }
        if (!firstModifiedModelLoad) {
            setModifiedCar({...modifiedCar, model: null})
        }
        setFirstModifiedModelLoad(false);
    }, [modifiedMake])

    const handleNewCarPossibleCategoriesSelect = (selectedCategories) => {
        setNewCar({...newCar, possibleCategories: selectedCategories});
    }

    const handleNewCarPossibleDoorTypesSelect = (selectedDoorTypes) => {
        setNewCar({...newCar, possibleDoorTypes: selectedDoorTypes});
    }

    const handleNewCarPossibleTransmissionsSelect = (selectedTransmissions) => {
        setNewCar({...newCar, possibleTransmissions: selectedTransmissions});
    }

    const handleNewCarPossibleTractionsSelect = (selectedTractions) => {
        setNewCar({...newCar, possibleTractions: selectedTractions});
    }

    const handleNewCarPossibleEnginesSelect = (selectedEngines) => {
        setNewCar({...newCar, possibleEngines: selectedEngines});
    }

    const handleModifiedCarPossibleCategoriesSelect = (selectedCategories) => {
        setModifiedCar({...modifiedCar, possibleCategories: selectedCategories})
    }

    const handleModifiedCarPossibleDoorTypesSelect = (selectedDoorTypes) => {
        setModifiedCar({...modifiedCar, possibleDoorTypes: selectedDoorTypes})
    }

    const handleModifiedCarPossibleTransmissionsSelect = (selectedTransmissions) => {
        setModifiedCar({...modifiedCar, possibleTransmissions: selectedTransmissions})
    }

    const handleModifiedCarPossibleTractionsSelect = (selectedTractions) => {
        setModifiedCar({...modifiedCar, possibleTractions: selectedTractions})
    }

    const handleModifiedCarPossibleEnginesSelect = (selectedEngines) => {
        setModifiedCar({...modifiedCar, possibleEngines: selectedEngines})
    }

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newMake: '',
            newModel: '',
            newStartYear: '',
            newEndYear: '',
            newPossibleCategories: '',
            newPossibleDoorTypes: '',
            newPossibleTransmissions: '',
            newPossibleTractions: '',
            newPossibleEngines: '',
        }
        if (selectedNewMake === null) {
            currentError.newMake = 'Make is required';
            isValid = false;
        }
        if (newCar.model === null) {
            currentError.newModel = 'Model is required';
            isValid = false;
        }
        if (newCar.startYear === '') {
            currentError.newStartYear = 'Start year is required';
            isValid = false;
        }
        else if (isNaN(newCar.startYear)) {
            currentError.newStartYear = 'Invalid start year';
            isValid = false;
        }
        else if (newCar.startYear < 1900) {
            currentError.newStartYear = 'Start year should be greater than 1900';
            isValid = false;
        }
        if (newCar.endYear !== '') {
            if (isNaN(newCar.endYear)) {
                currentError.newEndYear = 'Invalid end year';
                isValid = false;
            }
            else if (newCar.endYear < 1900) {
                currentError.newEndYear = 'End year should be greater than 1900';
                isValid = false;
            }
            else if (newCar.endYear < newCar.startYear) {
                currentError.newEndYear = 'End year should be greater than start year';
                isValid = false;
            }
        }
        if (newCar.possibleCategories.length === 0) {
            currentError.newPossibleCategories = 'At least one category is required';
            isValid = false;
        }
        if (newCar.possibleDoorTypes.length === 0) {
            currentError.newPossibleDoorTypes = 'At least one door type is required';
            isValid = false;
        }
        if (newCar.possibleTransmissions.length === 0) {
            currentError.newPossibleTransmissions = 'At least one transmission is required';
            isValid = false;
        }
        if (newCar.possibleTractions.length === 0) {
            currentError.newPossibleTractions = 'At least one traction is required';
            isValid = false;
        }
        if (newCar.possibleEngines.length === 0) {
            currentError.newPossibleEngines = 'At least one engine is required';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Car");
        setDialogText(`Are you sure you want to add the car?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createCar({
            model: newCar.model,
            startYear: parseInt(newCar.startYear),
            endYear: newCar.endYear === "" ? 1 : parseInt(newCar.endYear),
            possibleCategories: newCar.possibleCategories,
            possibleDoorTypes: newCar.possibleDoorTypes,
            possibleTransmissions: newCar.possibleTransmissions,
            possibleTractions: newCar.possibleTractions,
            possibleEngines: newCar.possibleEngines,
        })
            .then((result) => {
                setDialogText(`Car added successfully.`);
                setNewCar({
                    model: null,
                    startYear: '',
                    endYear: '',
                    possibleCategories: [],
                    possibleDoorTypes: [],
                    possibleTransmissions: [],
                    possibleTractions: [],
                    possibleEngines: [],
                });
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding car.`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    const handleModifyDialog = () => {
        let isValid = true;
        let currentError = {
            modifiedMake: '',
            modifiedModel: '',
            modifiedStartYear: '',
            modifiedEndYear: '',
            modifiedPossibleCategories: '',
            modifiedPossibleDoorTypes: '',
            modifiedPossibleTransmissions: '',
            modifiedPossibleTractions: '',
            modifiedPossibleEngines: '',
        }
        if (modifiedMake === null) {
            currentError.modifiedMake = 'Make is required';
            isValid = false;
        }
        if (modifiedCar.model === null) {
            currentError.modifiedModel = 'Model is required';
            isValid = false;
        }
        if (modifiedCar.startYear === '') {
            currentError.modifiedStartYear = 'Start year is required';
            isValid = false;
        }
        else if (isNaN(modifiedCar.startYear)) {
            currentError.modifiedStartYear = 'Invalid start year';
            isValid = false;
        }
        else if (modifiedCar.startYear < 1900) {
            currentError.modifiedStartYear = 'Start year should be greater than 1900';
            isValid = false;
        }
        if (modifiedCar.endYear !== '') {
            if (isNaN(modifiedCar.endYear)) {
                currentError.modifiedEndYear = 'Invalid end year';
                isValid = false;
            }
            else if (modifiedCar.endYear < 1900) {
                currentError.modifiedEndYear = 'End year should be greater than 1900';
                isValid = false;
            }
            else if (modifiedCar.endYear < modifiedCar.startYear) {
                currentError.modifiedEndYear = 'End year should be greater than start year';
                isValid = false;
            }
        }
        if (modifiedCar.possibleCategories.length === 0) {
            currentError.modifiedPossibleCategories = 'At least one category is required';
            isValid = false;
        }
        if (modifiedCar.possibleDoorTypes.length === 0) {
            currentError.modifiedPossibleDoorTypes = 'At least one door type is required';
            isValid = false;
        }
        if (modifiedCar.possibleTransmissions.length === 0) {
            currentError.modifiedPossibleTransmissions = 'At least one transmission is required';
            isValid = false;
        }
        if (modifiedCar.possibleTractions.length === 0) {
            currentError.modifiedPossibleTractions = 'At least one traction is required';
            isValid = false;
        }
        if (modifiedCar.possibleEngines.length === 0) {
            currentError.modifiedPossibleEngines = 'At least one engine is required';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Car");
        setDialogText(`Are you sure you want to modify the car?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedCar = {
            id: modifiedCar.id,
            model: modifiedCar.model.id === selectedCar.model.id ? null : modifiedCar.model,
            startYear: modifiedCar.startYear === selectedCar.startYear ? 0 : parseInt(modifiedCar.startYear),
            endYear: modifiedCar.endYear === selectedCar.endYear ? 0 : modifiedCar.endYear === "" ? 1 : parseInt(modifiedCar.endYear),
            possibleCategories: modifiedCar.possibleCategories === selectedCar.possibleCategories ? null : modifiedCar.possibleCategories,
            possibleDoorTypes: modifiedCar.possibleDoorTypes === selectedCar.possibleDoorTypes ? null : modifiedCar.possibleDoorTypes,
            possibleTransmissions: modifiedCar.possibleTransmissions === selectedCar.possibleTransmissions ? null : modifiedCar.possibleTransmissions,
            possibleTractions: modifiedCar.possibleTractions === selectedCar.possibleTractions ? null : modifiedCar.possibleTractions,
            possibleEngines: modifiedCar.possibleEngines === selectedCar.possibleEngines ? null : modifiedCar.possibleEngines,
        }
        updateCar(updatedCar)
            .then(() => {
                setDialogText(`Car modified successfully.`);
                setSelectedModifiedMake(null);
            })
            .catch((error) => {
                setDialogText(`Error modifying car "${selectedCar.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    console.log(modifiedCar)

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Car Panel"}>
                    {!loading && (
                        <React.Fragment>
                            <CommonSubPaper title={"Add"}>
                                <Grid
                                    container
                                    spacing={2}
                                    sx={{
                                        justifyContent: 'left',
                                        alignItems: 'center'
                                    }}
                                >
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedNewMake(newValue)
                                            }}
                                            value={selectedNewMake}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField
                                                {...params}
                                                label="Make"
                                                error={!!error.newMake}
                                                helperText={error.newMake}
                                            />}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={newModels.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setNewCar({ ...newCar, model: newValue })
                                            }}
                                            value={newCar.model}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField
                                                {...params}
                                                label="Model"
                                                error={!!error.newModel}
                                                helperText={error.newModel}
                                            />}
                                            disabled={!selectedNewMake}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"Start Year"}
                                            type={"number"}
                                            value={newCar.startYear}
                                            onChange={(event) => setNewCar({ ...newCar, startYear: event.target.value})}
                                            fullWidth
                                            error={!!error.newStartYear}
                                            helperText={error.newStartYear}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"End Year"}
                                            type={"number"}
                                            value={newCar.endYear}
                                            onChange={(event) => setNewCar({ ...newCar, endYear: event.target.value})}
                                            fullWidth
                                            error={!!error.newEndYear}
                                            helperText={error.newEndYear}
                                        />
                                    </Grid>
                                    <GridAutocompleteList
                                        name={"Categories"}
                                        list={categories}
                                        setSelect={handleNewCarPossibleCategoriesSelect}
                                        error={error.newPossibleCategories}
                                    />
                                    <GridAutocompleteList
                                        name={"Door Types"}
                                        list={doorTypes}
                                        setSelect={handleNewCarPossibleDoorTypesSelect}
                                        error={error.newPossibleDoorTypes}
                                    />
                                    <GridAutocompleteList
                                        name={"Transmissions"}
                                        list={transmissions}
                                        setSelect={handleNewCarPossibleTransmissionsSelect}
                                        error={error.newPossibleTransmissions}
                                    />
                                    <GridAutocompleteList
                                        name={"Tractions"}
                                        list={tractions}
                                        setSelect={handleNewCarPossibleTractionsSelect}
                                        error={error.newPossibleTractions}
                                    />
                                    <GridAutocompleteList
                                        name={"Engines"}
                                        list={engines.map(engine => ({...engine, name: engine.engineCode}))}
                                        setSelect={handleNewCarPossibleEnginesSelect}
                                        error={error.newPossibleEngines}
                                    />
                                    <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'flex-start' }}>
                                        <Button
                                            variant={"contained"}
                                            onClick={handleAddDialog}
                                        >
                                            Add
                                        </Button>
                                    </Grid>
                                </Grid>
                            </CommonSubPaper>
                            <CommonSubPaper title={"Modify"}>
                                <Grid
                                    container
                                    spacing={2}
                                    sx={{
                                        justifyContent: 'left',
                                        alignItems: 'center'
                                    }}
                                >
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedModifiedMake(newValue)
                                            }}
                                            value={selectedModifiedMake}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a make" />}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={modifiedModels.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedModifiedModel(newValue)
                                            }}
                                            value={selectedModifiedModel}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a model" />}
                                            disabled={!selectedModifiedMake}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={modifiedGenerations.sort((a, b) => a.startYear - b.startYear)}
                                            getOptionLabel={(option) => (option.startYear + ' - ' + (option.endYear !== 0 ? option.endYear : 'current'))}
                                            onChange={(event, newValue) => {
                                                if (newValue) {
                                                    setSelectedCar({
                                                        ...newValue,
                                                        endYear: newValue.endYear === 0 ? '' : newValue.endYear
                                                    })
                                                    setModifiedCar({
                                                        ...newValue,
                                                        endYear: newValue.endYear === 0 ? '' : newValue.endYear
                                                    })
                                                }
                                                else {
                                                    setSelectedCar(null);
                                                    setModifiedCar({
                                                        id: null,
                                                        model: null,
                                                        startYear: '',
                                                        endYear: '',
                                                        possibleCategories: [],
                                                        possibleDoorTypes: [],
                                                        possibleTransmissions: [],
                                                        possibleTractions: [],
                                                        possibleEngines: [],
                                                    })
                                                }
                                            }}
                                            value={selectedCar}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a generation" />}
                                            disabled={!selectedModifiedModel}
                                        />
                                    </Grid>
                                    <Grid item xs={3} />
                                    {selectedCar && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setModifiedMake(newValue)
                                                    }}
                                                    value={modifiedMake}
                                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                                    renderInput={(params) => <TextField
                                                        {...params}
                                                        label="Make"
                                                        error={!!error.modifiedMake}
                                                        helperText={error.modifiedMake}
                                                    />}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={modifiedMakeModels.sort((a, b) => -b.name.localeCompare(a.name))}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setModifiedCar({ ...modifiedCar, model: newValue })
                                                    }}
                                                    value={modifiedCar.model}
                                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                                    renderInput={(params) => <TextField
                                                        {...params}
                                                        label="Model"
                                                        error={!!error.modifiedModel}
                                                        helperText={error.modifiedModel}
                                                    />}
                                                    disabled={!modifiedMake}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Start Year"}
                                                    type={"number"}
                                                    value={modifiedCar.startYear}
                                                    onChange={(event) => setModifiedCar({ ...modifiedCar, startYear: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedStartYear}
                                                    helperText={error.modifiedStartYear}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"End Year"}
                                                    type={"number"}
                                                    value={modifiedCar.endYear}
                                                    onChange={(event) => setModifiedCar({ ...modifiedCar, endYear: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedEndYear}
                                                    helperText={error.modifiedEndYear}
                                                />
                                            </Grid>
                                            <GridAutocompleteList
                                                name={"Categories"}
                                                list={categories}
                                                setSelect={handleModifiedCarPossibleCategoriesSelect}
                                                select={modifiedCar.possibleCategories}
                                                error={error.modifiedPossibleCategories}
                                            />
                                            <GridAutocompleteList
                                                name={"Door Types"}
                                                list={doorTypes}
                                                setSelect={handleModifiedCarPossibleDoorTypesSelect}
                                                select={modifiedCar.possibleDoorTypes}
                                                error={error.modifiedPossibleDoorTypes}
                                            />
                                            <GridAutocompleteList
                                                name={"Transmissions"}
                                                list={transmissions}
                                                setSelect={handleModifiedCarPossibleTransmissionsSelect}
                                                select={modifiedCar.possibleTransmissions}
                                                error={error.modifiedPossibleTransmissions}
                                            />
                                            <GridAutocompleteList
                                                name={"Tractions"}
                                                list={tractions}
                                                setSelect={handleModifiedCarPossibleTractionsSelect}
                                                select={modifiedCar.possibleTractions}
                                                error={error.modifiedPossibleTractions}
                                            />
                                            <GridAutocompleteList
                                                name={"Engines"}
                                                list={engines.map(engine => ({...engine, name: engine.engineCode}))}
                                                setSelect={handleModifiedCarPossibleEnginesSelect}
                                                select={modifiedCar.possibleEngines.map(engine => ({...engine, name: engine.engineCode}))}
                                                error={error.modifiedPossibleEngines}
                                            />
                                            <Grid
                                                item
                                                xs={12}
                                                sx={{
                                                    display: 'flex',
                                                    justifyContent: 'flex-start',
                                                    gap: 2
                                                }}
                                            >
                                                <Button
                                                    variant={"contained"}
                                                    onClick={handleModifyDialog}
                                                >
                                                    Modify
                                                </Button>
                                            </Grid>
                                        </React.Fragment>
                                    )}
                                </Grid>
                            </CommonSubPaper>
                        </React.Fragment>
                    )}
                </CommonPaper>
            </Grid>

            <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
                <DialogTitle>{dialogTitle}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {dialogText}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    {dialogAcknowledgement ? (
                        <Button onClick={() => setDialogOpen(false)}>
                            Close
                        </Button>
                    ) : (
                        <React.Fragment>
                            <Button onClick={() => setDialogOpen(false)}>
                                No
                            </Button>
                            <Button onClick={dialogYesAction} disabled={dialogYesDisabled}>
                                Yes
                            </Button>
                        </React.Fragment>
                    )}
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}

export default CarPanel;