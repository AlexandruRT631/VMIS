import React, {useEffect, useState} from "react";
import {createEngine, getAllEngines, updateEngine} from "../../api/engine-api";
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
import {getAllFuels} from "../../api/fuel-api";

const EnginePanel = () => {
    const [loading, setLoading] = useState(true);
    const [newEngine, setNewEngine] = useState({
        make: null,
        engineCode: '',
        displacement: '',
        fuel: null,
        power: '',
        torque: '',
    });
    const [engines, setEngines] = useState([]);
    const [makes, setMakes] = useState([]);
    const [fuels, setFuels] = useState([]);
    const [powerFilter, setPowerFilter] = useState('');
    const [torqueFilter, setTorqueFilter] = useState('');
    const [makeFilter, setMakeFilter] = useState(null);
    const [powerFilterError, setPowerFilterError] = useState('');
    const [torqueFilterError, setTorqueFilterError] = useState('');
    const [filteredEngines, setFilteredEngines] = useState([]);
    const [selectedEngine, setSelectedEngine] = useState(null);
    const [modifiedEngine, setModifiedEngine] = useState({
        id: null,
        make: null,
        engineCode: '',
        displacement: '',
        fuel: null,
        power: '',
        torque: '',
    })
    const [error, setError] = useState({
        newMake: '',
        newEngineCode: '',
        newDisplacement: '',
        newFuel: '',
        newPower: '',
        newTorque: '',
        modifiedMake: '',
        modifiedEngineCode: '',
        modifiedDisplacement: '',
        modifiedFuel: '',
        modifiedPower: '',
        modifiedTorque: '',
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
                const fetchedEngines = await getAllEngines();
                setEngines(fetchedEngines);
                setFilteredEngines(fetchedEngines);
                const fetchedMakes = await getAllMakes();
                setMakes(fetchedMakes);
                const fetchedFuels = await getAllFuels();
                setFuels(fetchedFuels);
            }
            catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false))
    }, []);

    useEffect(() => {
        let enginesFiltered = engines;
        if (powerFilter) {
            if (!isNaN(powerFilter)) {
                if (parseInt(powerFilter) > 0) {
                    enginesFiltered = enginesFiltered.filter((engine) => engine.power === parseInt(powerFilter));
                }
                else {
                    setPowerFilterError('Power should be greater than 0');
                }
            }
            else {
                setPowerFilterError('Power should be a number');
            }
        }
        else {
            setPowerFilterError('');
        }
        if (torqueFilter) {
            if (!isNaN(torqueFilter)) {
                if (parseInt(torqueFilter) > 0) {
                    enginesFiltered = enginesFiltered.filter((engine) => engine.torque === parseInt(torqueFilter));
                }
                else {
                    setTorqueFilterError('Torque should be greater than 0');
                }
            }
            else {
                setTorqueFilterError('Torque should be a number');
            }
        }
        else {
            setTorqueFilterError('');
        }
        if (makeFilter) {
            enginesFiltered = enginesFiltered.filter((engine) => engine.make.id === makeFilter.id);
        }
        setFilteredEngines(enginesFiltered);
        setSelectedEngine(null);
    }, [powerFilter, torqueFilter, makeFilter, engines]);

    useEffect(() => {
        setError({
            newMake: '',
            newEngineCode: '',
            newDisplacement: '',
            newFuel: '',
            newPower: '',
            newTorque: '',
            modifiedMake: '',
            modifiedEngineCode: '',
            modifiedDisplacement: '',
            modifiedFuel: '',
            modifiedPower: '',
            modifiedTorque: '',
        });
    }, [selectedEngine]);

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newMake: '',
            newEngineCode: '',
            newDisplacement: '',
            newFuel: '',
            newPower: '',
            newTorque: '',
        }
        if (newEngine.make === null) {
            currentError.newMake = 'Make is required';
            isValid = false;
        }
        if (newEngine.engineCode === '') {
            currentError.newEngineCode = 'Name is required';
            isValid = false;
        }
        if (newEngine.displacement === '') {
            currentError.newDisplacement = 'Displacement is required';
            isValid = false;
        }
        else if (isNaN(newEngine.displacement)) {
            currentError.newDisplacement = 'Displacement should be a number';
            isValid = false;
        }
        else if (parseInt(newEngine.displacement) <= 0) {
            currentError.newDisplacement = 'Displacement should be greater than 0';
            isValid = false;
        }
        if (newEngine.fuel === null) {
            currentError.newFuel = 'Fuel is required';
            isValid = false;
        }
        if (newEngine.power === '') {
            currentError.newPower = 'Power is required';
            isValid = false;
        }
        else if (isNaN(newEngine.power)) {
            currentError.newPower = 'Power should be a number';
            isValid = false;
        }
        else if (parseInt(newEngine.power) <= 0) {
            currentError.newPower = 'Power should be greater than 0';
            isValid = false;
        }
        if (newEngine.torque === '') {
            currentError.newTorque = 'Torque is required';
            isValid = false;
        }
        else if (isNaN(newEngine.torque)) {
            currentError.newTorque = 'Torque should be a number';
            isValid = false;
        }
        else if (parseInt(newEngine.torque) <= 0) {
            currentError.newTorque = 'Torque should be greater than 0';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Engine");
        setDialogText(`Are you sure you want to add the engine "${newEngine.engineCode}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createEngine({
            make: newEngine.make,
            engineCode: newEngine.engineCode,
            displacement: parseInt(newEngine.displacement),
            fuel: newEngine.fuel,
            power: parseInt(newEngine.power),
            torque: parseInt(newEngine.torque)
        })
            .then((result) => {
                setDialogText(`Engine "${newEngine.engineCode}" added successfully.`);
                setEngines([...engines, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding engine "${newEngine.engineCode}".`);
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
            modifiedEngineCode: '',
            modifiedDisplacement: '',
            modifiedFuel: '',
            modifiedPower: '',
            modifiedTorque: '',
        }
        if (modifiedEngine.make === null) {
            currentError.modifiedMake = 'Make is required';
            isValid = false;
        }
        if (modifiedEngine.engineCode === '') {
            currentError.modifiedEngineCode = 'Name is required';
            isValid = false;
        }
        if (modifiedEngine.displacement === '') {
            currentError.modifiedDisplacement = 'Displacement is required';
            isValid = false;
        }
        else if (isNaN(modifiedEngine.displacement)) {
            currentError.modifiedDisplacement = 'Displacement should be a number';
            isValid = false;
        }
        else if (parseInt(modifiedEngine.displacement) <= 0) {
            currentError.modifiedDisplacement = 'Displacement should be greater than 0';
            isValid = false;
        }
        if (modifiedEngine.fuel === null) {
            currentError.modifiedFuel = 'Fuel is required';
            isValid = false;
        }
        if (modifiedEngine.power === '') {
            currentError.modifiedPower = 'Power is required';
            isValid = false;
        }
        else if (isNaN(modifiedEngine.power)) {
            currentError.modifiedPower = 'Power should be a number';
            isValid = false;
        }
        else if (parseInt(modifiedEngine.power) <= 0) {
            currentError.modifiedPower = 'Power should be greater than 0';
            isValid = false;
        }
        if (modifiedEngine.torque === '') {
            currentError.modifiedTorque = 'Torque is required';
            isValid = false;
        }
        else if (isNaN(modifiedEngine.torque)) {
            currentError.modifiedTorque = 'Torque should be a number';
            isValid = false;
        }
        else if (parseInt(modifiedEngine.torque) <= 0) {
            currentError.modifiedTorque = 'Torque should be greater than 0';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Engine");
        setDialogText(`Are you sure you want to modify the engine "${selectedEngine.engineCode}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedEngine = {
            id: modifiedEngine.id,
            make: modifiedEngine.make.id === selectedEngine.make.id ? null : modifiedEngine.make,
            engineCode: modifiedEngine.engineCode === selectedEngine.engineCode ? '' : modifiedEngine.engineCode,
            displacement: modifiedEngine.displacement === selectedEngine.displacement ? 0 : parseInt(modifiedEngine.displacement),
            fuel: modifiedEngine.fuel.id === selectedEngine.fuel.id ? null : modifiedEngine.fuel,
            power: modifiedEngine.power === selectedEngine.power ? 0 : parseInt(modifiedEngine.power),
            torque: modifiedEngine.torque === selectedEngine.torque ? 0 : parseInt(modifiedEngine.torque),
        }
        updateEngine(updatedEngine)
            .then(() => {
                setDialogText(`Engine "${modifiedEngine.engineCode}" modified successfully.`);
                setSelectedEngine(modifiedEngine)
                setEngines(engines.map((engine) => {
                    if (engine.id === modifiedEngine.id) {
                        return modifiedEngine;
                    }
                    return engine;
                }));
                setPowerFilter('');
                setTorqueFilter('');
                setMakeFilter(null);
            })
            .catch((error) => {
                setDialogText(`Error modifying engine "${selectedEngine.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    console.log(modifiedEngine);

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Engine Panel"}>
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
                                                setNewEngine({ ...newEngine, make: newValue })
                                            }}
                                            value={newEngine.make}
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
                                        <TextField
                                            label={"Engine Code"}
                                            value={newEngine.engineCode}
                                            onChange={(event) => setNewEngine({ ...newEngine, engineCode: event.target.value})}
                                            fullWidth
                                            error={!!error.newEngineCode}
                                            helperText={error.newEngineCode}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"Displacement"}
                                            type={"number"}
                                            value={newEngine.displacement}
                                            onChange={(event) => setNewEngine({ ...newEngine, displacement: event.target.value})}
                                            fullWidth
                                            error={!!error.newDisplacement}
                                            helperText={error.newDisplacement}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={fuels.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setNewEngine({ ...newEngine, fuel: newValue })
                                            }}
                                            value={newEngine.fuel}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField
                                                {...params}
                                                label="Fuel"
                                                error={!!error.newFuel}
                                                helperText={error.newFuel}
                                            />}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"Power"}
                                            type={"number"}
                                            value={newEngine.power}
                                            onChange={(event) => setNewEngine({ ...newEngine, power: event.target.value})}
                                            fullWidth
                                            error={!!error.newPower}
                                            helperText={error.newPower}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"Torque"}
                                            type={"number"}
                                            value={newEngine.torque}
                                            onChange={(event) => setNewEngine({ ...newEngine, torque: event.target.value})}
                                            fullWidth
                                            error={!!error.newTorque}
                                            helperText={error.newTorque}
                                        />
                                    </Grid>
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
                                        <TextField
                                            label={"Power Filter"}
                                            type={"number"}
                                            value={powerFilter}
                                            onChange={(event) => setPowerFilter(event.target.value)}
                                            fullWidth
                                            error={!!powerFilterError}
                                            helperText={powerFilterError}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField
                                            label={"Torque Filter"}
                                            type={"number"}
                                            value={torqueFilter}
                                            onChange={(event) => setTorqueFilter(event.target.value)}
                                            fullWidth
                                            error={!!torqueFilterError}
                                            helperText={torqueFilterError}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setMakeFilter(newValue)
                                            }}
                                            value={makeFilter}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField
                                                {...params}
                                                label="Make Filter"
                                            />}
                                        />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <Autocomplete
                                            options={filteredEngines.sort((a, b) => -b.engineCode.localeCompare(a.engineCode))}
                                            getOptionLabel={(option) => option.engineCode}
                                            onChange={(event, newValue) => {
                                                setSelectedEngine(newValue);
                                                setModifiedEngine(newValue)
                                            }}
                                            value={selectedEngine}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select an engine" />}
                                        />
                                    </Grid>
                                    {selectedEngine && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setModifiedEngine({ ...modifiedEngine, make: newValue })
                                                    }}
                                                    value={modifiedEngine.make}
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
                                                <TextField
                                                    label={"Engine Code"}
                                                    value={modifiedEngine.engineCode}
                                                    onChange={(event) => setModifiedEngine({...modifiedEngine, engineCode: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedEngineCode}
                                                    helperText={error.modifiedEngineCode}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Displacement"}
                                                    type={"number"}
                                                    value={modifiedEngine.displacement}
                                                    onChange={(event) => setModifiedEngine({ ...modifiedEngine, displacement: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedDisplacement}
                                                    helperText={error.modifiedDisplacement}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={fuels.sort((a, b) => -b.name.localeCompare(a.name))}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setModifiedEngine({ ...modifiedEngine, fuel: newValue })
                                                    }}
                                                    value={modifiedEngine.fuel}
                                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                                    renderInput={(params) => <TextField
                                                        {...params}
                                                        label="Fuel"
                                                        error={!!error.modifiedFuel}
                                                        helperText={error.modifiedFuel}
                                                    />}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Power"}
                                                    type={"number"}
                                                    value={modifiedEngine.power}
                                                    onChange={(event) => setModifiedEngine({ ...modifiedEngine, power: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedPower}
                                                    helperText={error.modifiedPower}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Torque"}
                                                    type={"number"}
                                                    value={modifiedEngine.torque}
                                                    onChange={(event) => setModifiedEngine({ ...modifiedEngine, torque: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedTorque}
                                                    helperText={error.modifiedTorque}
                                                />
                                            </Grid>
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
};

export default EnginePanel;