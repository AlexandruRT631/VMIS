import React, {useEffect, useState} from "react";
import {createTraction, getAllTractions, updateTraction} from "../../api/traction-api";
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

const TractionPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newTraction, setNewTraction] = useState({
        name: ''
    });
    const [tractions, setTractions] = useState([]);
    const [selectedTraction, setSelectedTraction] = useState(null);
    const [modifiedTraction, setModifiedTraction] = useState({
        id: null,
        name: ''
    })
    const [error, setError] = useState({
        newName: '',
        modifiedName: ''
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
                const fetchedTractions = await getAllTractions();
                setTractions(fetchedTractions);
            }
            catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false))
    }, []);

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newName: ''
        }
        if (newTraction.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newTraction.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Traction");
        setDialogText(`Are you sure you want to add the traction "${newTraction.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createTraction(newTraction)
            .then((result) => {
                setDialogText(`Traction "${newTraction.name}" added successfully.`);
                setTractions([...tractions, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding traction "${newTraction.name}".`);
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
        if (modifiedTraction.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedTraction.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Traction");
        setDialogText(`Are you sure you want to modify the traction "${selectedTraction.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedTraction = {
            id: modifiedTraction.id,
            name: modifiedTraction.name === selectedTraction.name ? "" : modifiedTraction.name
        }
        updateTraction(updatedTraction)
            .then(() => {
                setDialogText(`Traction "${modifiedTraction.name}" modified successfully.`);
                setSelectedTraction(modifiedTraction)
                setTractions(tractions.map((traction) => {
                    if (traction.id === modifiedTraction.id) {
                        return modifiedTraction;
                    }
                    return traction;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying traction "${selectedTraction.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Traction Panel"}>
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
                                <TextField
                                    label={"Traction Name"}
                                    value={newTraction.name}
                                    onChange={(event) => setNewTraction({ ...newTraction, name: event.target.value})}
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
                                            options={tractions.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedTraction(newValue);
                                                setModifiedTraction(newValue)
                                            }}
                                            value={selectedTraction}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a traction" />}
                                        />
                                    </Grid>
                                    {selectedTraction && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Traction Name"}
                                                    value={modifiedTraction.name}
                                                    onChange={(event) => setModifiedTraction({...modifiedTraction, name: event.target.value})}
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

export default TractionPanel;