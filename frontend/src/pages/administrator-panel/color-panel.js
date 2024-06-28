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
import React, {useEffect, useState} from "react";
import CommonSubPaper from "../../common/common-sub-paper";
import {createColor, getAllColors, updateColor} from "../../api/color-api";

const ColorPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newColor, setNewColor] = useState({
        name: '',
        hexCode: '',
    });
    const [colors, setColors] = useState([]);
    const [selectedColor, setSelectedColor] = useState(null);
    const [modifiedColor, setModifiedColor] = useState({
        id: null,
        name: '',
        hexCode: '',
    })
    const [error, setError] = useState({
        newName: '',
        newHexCode: '',
        modifiedName: '',
        modifiedHexCode: '',
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
                const fetchedColors = await getAllColors();
                setColors(fetchedColors);
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
            newName: '',
            newHexCode: '',
        };
        if (newColor.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newColor.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        if (newColor.hexCode === '') {
            currentError.newHexCode = 'Hex code is required';
            isValid = false;
        }
        else if (!/^#[0-9A-F]{6}$/i.test(newColor.hexCode)) {
            currentError.newHexCode = 'Invalid hex code, it should have the form #FFFFFF';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Color");
        setDialogText(`Are you sure you want to add the color "${newColor.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createColor(newColor)
            .then((result) => {
                setDialogText(`Color "${newColor.name}" added successfully.`);
                setColors([...colors, result]);
            })
            .catch((error) => {
                setDialogText(`Error adding color "${newColor.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    const handleModifyDialog = () => {
        let isValid = true;
        let currentError = {
            modifiedName: '',
            modifiedHexCode: '',
        };
        if (modifiedColor.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedColor.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        if (modifiedColor.hexCode === '') {
            currentError.modifiedHexCode = 'Hex code is required';
            isValid = false;
        }
        else if (!/^#[0-9A-F]{6}$/i.test(modifiedColor.hexCode)) {
            currentError.modifiedHexCode = 'Invalid hex code, it should have the form #FFFFFF';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Color");
        setDialogText(`Are you sure you want to modify the color "${selectedColor.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedColor = {
            id: modifiedColor.id,
            name: modifiedColor.name === selectedColor.name ? "" : modifiedColor.name,
            hexCode: modifiedColor.hexCode === selectedColor.hexCode ? "" : modifiedColor.hexCode,
        }
        updateColor(updatedColor)
            .then(() => {
                setDialogText(`Color "${modifiedColor.name}" modified successfully.`);
                setSelectedColor(modifiedColor)
                setColors(colors.map((color) => {
                    if (color.id === modifiedColor.id) {
                        return modifiedColor;
                    }
                    return color;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying color "${selectedColor.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Color Panel"}>
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
                                    label={"Color Name"}
                                    value={newColor.name}
                                    onChange={(event) => setNewColor({ ...newColor, name: event.target.value})}
                                    fullWidth
                                    error={!!error.newName}
                                    helperText={error.newName}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label={"Color Hex Code"}
                                    value={newColor.hexCode}
                                    onChange={(event) => setNewColor({ ...newColor, hexCode: event.target.value})}
                                    fullWidth
                                    error={!!error.newHexCode}
                                    helperText={error.newHexCode}
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
                                            options={colors.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedColor(newValue);
                                                setModifiedColor(newValue)
                                            }}
                                            value={selectedColor}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a color" />}
                                        />
                                    </Grid>
                                    {selectedColor && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Color Name"}
                                                    value={modifiedColor.name}
                                                    onChange={(event) => setModifiedColor({...modifiedColor, name: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedName}
                                                    helperText={error.modifiedName}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Color Hex Code"}
                                                    value={modifiedColor.hexCode}
                                                    onChange={(event) => setModifiedColor({...modifiedColor, hexCode: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedHexCode}
                                                    helperText={error.modifiedHexCode}
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

export default ColorPanel;