import React, {useEffect, useState} from "react";
import {createModel, getAllModelsByMakeId, updateModel} from "../../api/model-api";
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

const ModelPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newModel, setNewModel] = useState({
        name: '',
        make: null,
    });
    const [models, setModels] = useState([]);
    const [makes, setMakes] = useState([]);
    const [selectedMake, setSelectedMake] = useState(null);
    const [selectedModel, setSelectedModel] = useState(null);
    const [modifiedModel, setModifiedModel] = useState({
        id: null,
        name: '',
        make: null,
    })
    const [error, setError] = useState({
        newName: '',
        newMake: '',
        modifiedName: '',
        modifiedMake: ''
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
            }
            catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false))
    }, []);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedModels = await getAllModelsByMakeId(selectedMake.id);
                setModels(fetchedModels);
            }
            catch (error) {
                console.error(error);
            }
        }

        if (selectedMake) {
            fetchData()
                .then(() => setLoading(false));
        }
        setSelectedModel(null);
        setModifiedModel(null);
    }, [selectedMake]);

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newName: ''
        }
        if (newModel.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        if (newModel.make === null) {
            currentError.newMake = 'Make is required';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Model");
        setDialogText(`Are you sure you want to add the model "${newModel.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createModel(newModel)
            .then((result) => {
                setDialogText(`Model "${newModel.name}" added successfully.`);
                setModels([...models, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding model "${newModel.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    const handleModifyDialog = () => {
        let isValid = true;
        let currentError = {
            modifiedName: ''
        }
        if (modifiedModel.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        if (modifiedModel.make === null) {
            currentError.modifiedMake = 'Make is required';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Model");
        setDialogText(`Are you sure you want to modify the model "${selectedModel.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedModel = {
            id: modifiedModel.id,
            name: modifiedModel.name === selectedModel.name ? "" : modifiedModel.name,
            make: modifiedModel.make.id === selectedModel.make.id ? null : modifiedModel.make
        }
        updateModel(updatedModel)
            .then(() => {
                setDialogText(`Model "${modifiedModel.name}" modified successfully.`);
                setSelectedMake(null);
            })
            .catch((error) => {
                setDialogText(`Error modifying model "${selectedModel.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Model Panel"}>
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
                                        setNewModel({ ...newModel, make: newValue })
                                    }}
                                    value={newModel.make}
                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                    renderInput={(params) => <TextField
                                        {...params}
                                        label="Select a make"
                                        error={!!error.newMake}
                                        helperText={error.newMake}
                                    />}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label={"Model Name"}
                                    value={newModel.name}
                                    onChange={(event) => setNewModel({ ...newModel, name: event.target.value})}
                                    fullWidth
                                    error={!!error.newName}
                                    helperText={error.newName}
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
                        {!loading && (
                            <React.Fragment>
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
                                                setSelectedMake(newValue);
                                            }}
                                            value={selectedMake}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a Make" />}
                                        />
                                    </Grid>
                                    {selectedMake && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={models.sort((a, b) => -b.name.localeCompare(a.name))}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setSelectedModel(newValue);
                                                        setModifiedModel(newValue)
                                                    }}
                                                    value={selectedModel}
                                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                                    renderInput={(params) => <TextField {...params} label="Select a model" />}
                                                />
                                            </Grid>
                                            <Grid item xs={6} />
                                            {selectedModel && (
                                                <React.Fragment>
                                                    <Grid item xs={3}>
                                                        <Autocomplete
                                                            options={makes.sort((a, b) => -b.name.localeCompare(a.name))}
                                                            getOptionLabel={(option) => option.name}
                                                            onChange={(event, newValue) => {
                                                                setModifiedModel({...modifiedModel, make: newValue})
                                                            }}
                                                            value={modifiedModel.make}
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
                                                            label={"Model Name"}
                                                            value={modifiedModel.name}
                                                            onChange={(event) => setModifiedModel({...modifiedModel, name: event.target.value})}
                                                            fullWidth
                                                            error={!!error.modifiedName}
                                                            helperText={error.modifiedName}
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
                                        </React.Fragment>
                                    )}
                                </Grid>
                            </React.Fragment>
                        )}
                    </CommonSubPaper>
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

export default ModelPanel;