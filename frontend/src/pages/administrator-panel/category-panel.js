import React, {useEffect, useState} from "react";
import {createCategory, getAllCategories, updateCategory} from "../../api/category-api";
import {
    Autocomplete,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    TextField
} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import CommonSubPaper from "../../common/common-sub-paper";

const CategoryPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newCategory, setNewCategory] = useState({
        name: ''
    });
    const [categories, setCategories] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [modifiedCategory, setModifiedCategory] = useState({
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
                const fetchedCategories = await getAllCategories();
                setCategories(fetchedCategories);
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
        if (newCategory.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newCategory.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add Category");
        setDialogText(`Are you sure you want to add the category "${newCategory.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createCategory(newCategory)
            .then((result) => {
                setDialogText(`Category "${newCategory.name}" added successfully.`);
                setCategories([...categories, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding category "${newCategory.name}".`);
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
        if (modifiedCategory.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedCategory.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify Category");
        setDialogText(`Are you sure you want to modify the category "${selectedCategory.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedCategory = {
            id: modifiedCategory.id,
            name: modifiedCategory.name === selectedCategory.name ? "" : modifiedCategory.name
        }
        updateCategory(updatedCategory)
            .then(() => {
                setDialogText(`Category "${modifiedCategory.name}" modified successfully.`);
                setSelectedCategory(modifiedCategory)
                setCategories(categories.map((category) => {
                    if (category.id === modifiedCategory.id) {
                        return modifiedCategory;
                    }
                    return category;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying category "${selectedCategory.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"Category Panel"}>
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
                                    label={"Category Name"}
                                    value={newCategory.name}
                                    onChange={(event) => setNewCategory({ ...newCategory, name: event.target.value})}
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
                                            options={categories.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                setSelectedCategory(newValue);
                                                setModifiedCategory(newValue)
                                            }}
                                            value={selectedCategory}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a category" />}
                                        />
                                    </Grid>
                                    {selectedCategory && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Category Name"}
                                                    value={modifiedCategory.name}
                                                    onChange={(event) => setModifiedCategory({...modifiedCategory, name: event.target.value})}
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
};

export default CategoryPanel;