import React, {useEffect, useState} from "react";
import {createTransmission, getAllTransmissions, updateTransmission} from "../../api/transmission-api";
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

const TransmissionPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newTransmission, setNewTransmission] = useState({
        name: ''
    });
    const [transmissions, setTransmissions] = useState([]);
    const [selectedTransmission, setSelectedTransmission] = useState(null);
    const [modifiedTransmission, setModifiedTransmission] = useState({
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
                const fetchedTransmissions = await getAllTransmissions();
                setTransmissions(fetchedTransmissions);
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
        if (newTransmission.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newTransmission.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Transmission");
        setDialogText(`Are you sure you want to add the transmission "${newTransmission.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createTransmission(newTransmission)
            .then((result) => {
                setDialogText(`Transmission "${newTransmission.name}" added successfully.`);
                setTransmissions([...transmissions, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding transmission "${newTransmission.name}".`);
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
        if (modifiedTransmission.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedTransmission.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Transmission");
        setDialogText(`Are you sure you want to modify the transmission "${selectedTransmission.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedTransmission = {
            id: modifiedTransmission.id,
            name: modifiedTransmission.name === selectedTransmission.name ? "" : modifiedTransmission.name
        }
        updateTransmission(updatedTransmission)
            .then(() => {
                setDialogText(`Transmission "${modifiedTransmission.name}" modified successfully.`);
                setSelectedTransmission(modifiedTransmission)
                setTransmissions(transmissions.map((transmission) => {
                    if (transmission.id === modifiedTransmission.id) {
                        return modifiedTransmission;
                    }
                    return transmission;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying transmission "${selectedTransmission.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Transmission Panel"}>
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
                                    label={"Transmission Name"}
                                    value={newTransmission.name}
                                    onChange={(event) => setNewTransmission({ ...newTransmission, name: event.target.value})}
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
                                            options={transmissions.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedTransmission(newValue);
                                                setModifiedTransmission(newValue)
                                            }}
                                            value={selectedTransmission}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a transmission" />}
                                        />
                                    </Grid>
                                    {selectedTransmission && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Transmission Name"}
                                                    value={modifiedTransmission.name}
                                                    onChange={(event) => setModifiedTransmission({...modifiedTransmission, name: event.target.value})}
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

export default TransmissionPanel;