import React, {useEffect, useState} from "react";
import {createMake, getAllMakes, updateMake} from "../../api/make-api";
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

const MakePanel = () => {
    const [loading, setLoading] = useState(true);
    const [newMake, setNewMake] = useState({
        name: ''
    });
    const [makes, setMakes] = useState([]);
    const [selectedMake, setSelectedMake] = useState(null);
    const [modifiedMake, setModifiedMake] = useState({
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

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newName: ''
        }
        if (newMake.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newMake.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Make");
        setDialogText(`Are you sure you want to add the make "${newMake.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createMake(newMake)
            .then((result) => {
                setDialogText(`Make "${newMake.name}" added successfully.`);
                setMakes([...makes, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding make "${newMake.name}".`);
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
        if (modifiedMake.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedMake.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Make");
        setDialogText(`Are you sure you want to modify the make "${selectedMake.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedMake = {
            id: modifiedMake.id,
            name: modifiedMake.name === selectedMake.name ? "" : modifiedMake.name
        }
        updateMake(updatedMake)
            .then(() => {
                setDialogText(`Make "${modifiedMake.name}" modified successfully.`);
                setSelectedMake(modifiedMake)
                setMakes(makes.map((make) => {
                    if (make.id === modifiedMake.id) {
                        return modifiedMake;
                    }
                    return make;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying make "${selectedMake.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Make Panel"}>
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
                                    label={"Make Name"}
                                    value={newMake.name}
                                    onChange={(event) => setNewMake({ ...newMake, name: event.target.value})}
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
                                                setModifiedMake(newValue)
                                            }}
                                            value={selectedMake}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a make" />}
                                        />
                                    </Grid>
                                    {selectedMake && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Make Name"}
                                                    value={modifiedMake.name}
                                                    onChange={(event) => setModifiedMake({...modifiedMake, name: event.target.value})}
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

export default MakePanel;