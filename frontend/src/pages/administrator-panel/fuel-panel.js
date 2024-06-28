import React, {useEffect, useState} from "react";
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
import {createFuel, getAllFuels, updateFuel} from "../../api/fuel-api";

const FuelPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newFuel, setNewFuel] = useState({
        name: ''
    });
    const [fuels, setFuels] = useState([]);
    const [selectedFuel, setSelectedFuel] = useState(null);
    const [modifiedFuel, setModifiedFuel] = useState({
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

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newName: ''
        }
        if (newFuel.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newFuel.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Fuel");
        setDialogText(`Are you sure you want to add the fuel "${newFuel.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createFuel(newFuel)
            .then((result) => {
                setDialogText(`Fuel "${newFuel.name}" added successfully.`);
                setFuels([...fuels, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding fuel "${newFuel.name}".`);
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
        if (modifiedFuel.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedFuel.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Fuel");
        setDialogText(`Are you sure you want to modify the fuel "${selectedFuel.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedFuel = {
            id: modifiedFuel.id,
            name: modifiedFuel.name === selectedFuel.name ? "" : modifiedFuel.name
        }
        updateFuel(updatedFuel)
            .then(() => {
                setDialogText(`Fuel "${modifiedFuel.name}" modified successfully.`);
                setSelectedFuel(modifiedFuel)
                setFuels(fuels.map((fuel) => {
                    if (fuel.id === modifiedFuel.id) {
                        return modifiedFuel;
                    }
                    return fuel;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying fuel "${selectedFuel.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Fuel Panel"}>
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
                                    label={"Fuel Name"}
                                    value={newFuel.name}
                                    onChange={(event) => setNewFuel({ ...newFuel, name: event.target.value})}
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
                                            options={fuels.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedFuel(newValue);
                                                setModifiedFuel(newValue)
                                            }}
                                            value={selectedFuel}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a fuel" />}
                                        />
                                    </Grid>
                                    {selectedFuel && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Fuel Name"}
                                                    value={modifiedFuel.name}
                                                    onChange={(event) => setModifiedFuel({...modifiedFuel, name: event.target.value})}
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

export default FuelPanel;