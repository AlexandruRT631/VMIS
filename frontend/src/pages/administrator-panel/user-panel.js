import React, {useEffect, useState} from "react";
import {createUser, deleteUser, getAllUsers, updateUser} from "../../api/user-api";
import {
    Autocomplete,
    Button,
    Dialog, DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    TextField, Typography
} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import CommonSubPaper from "../../common/common-sub-paper";

const roles = [
    {
        id: 1,
        name: 'Client'
    },
    {
        id: 2,
        name: 'Seller'
    },
    {
        id: 3,
        name: 'Admin'
    }
]

const UserPanel = () => {
    const [loading, setLoading] = useState(true);
    const [newUser, setNewUser] = useState({
        email: '',
        password: '',
        name: '',
        role: null,
    });
    const [newProfilePicture, setNewProfilePicture] = useState(null);
    const [uploadingNewProfilePicture, setUploadingNewProfilePicture] = useState(false);
    const [users, setUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [modifiedUser, setModifiedUser] = useState({
        id: null,
        email: '',
        password: '',
        name: '',
        role: 1,
        profilePictureUrl: '',
    })
    const [modifiedProfilePicture, setModifiedProfilePicture] = useState(null);
    const [uploadingModifiedProfilePicture, setUploadingModifiedProfilePicture] = useState(false);
    const [error, setError] = useState({
        newEmail: '',
        newPassword: '',
        newName: '',
        newRole: '',
        modifiedEmail: '',
        modifiedPassword: '',
        modifiedName: '',
        modifiedRole: ''
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
                const fetchedUsers = await getAllUsers();
                setUsers(fetchedUsers);
            }
            catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false))
    }, []);

    const handleNewProfilePicture = async (event) => {
        setUploadingNewProfilePicture(true);
        setNewProfilePicture(event.target.files[0]);
        setUploadingNewProfilePicture(false);
    }

    const handleModifiedProfilePicture = async (event) => {
        setUploadingModifiedProfilePicture(true);
        setModifiedProfilePicture(event.target.files[0]);
        setUploadingModifiedProfilePicture(false);
    }

    const handleAddDialog = () => {
        let isValid = true;
        let currentError = {
            newEmail: '',
            newPassword: '',
            newName: '',
            newRole: '',
        }
        if (newUser.name === '') {
            currentError.newName = 'Name is required';
            isValid = false;
        }
        else if (newUser.name.length < 3) {
            currentError.newName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        if (newUser.email === '') {
            currentError.newEmail = 'Email is required';
            isValid = false;
        }
        else if (!/\S+@\S+\.\S+/.test(newUser.email)) {
            currentError.newEmail = 'Enter a valid email';
            isValid = false;
        }
        if (newUser.password === '') {
            currentError.newPassword = 'Password is required';
            isValid = false;
        }
        else if (newUser.password.length < 8) {
            currentError.newPassword = 'Password should be of minimum 8 characters length';
            isValid = false;
        }
        if (newUser.role === null) {
            currentError.newRole = 'Role is required';
            isValid = false;
        }
        else if (newUser.role.id !== 1 && newUser.role.id !== 2 && newUser.role.id !== 3) {
            currentError.newRole = 'Invalid role';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Add User");
        setDialogText(`Are you sure you want to add the user "${newUser.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleAdd);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleAdd = () => {
        setDialogYesDisabled(true);
        createUser({
            Email: newUser.email,
            Password: newUser.password,
            Name: newUser.name,
            Role: newUser.role.id
        }, newProfilePicture)
            .then((result) => {
                setDialogText(`User "${newUser.name}" added successfully.`);
                setUsers([...users, result]);
                console.log(result);
            })
            .catch((error) => {
                setDialogText(`Error adding user "${newUser.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    const handleModifyDialog = () => {
        let isValid = true;
        let currentError = {
            modifiedEmail: '',
            modifiedPassword: '',
            modifiedName: '',
            modifiedRole: '',
        }
        if (modifiedUser.name === '') {
            currentError.modifiedName = 'Name is required';
            isValid = false;
        }
        else if (modifiedUser.name.length < 3) {
            currentError.modifiedName = 'Name should be of minimum 3 characters length'
            isValid = false;
        }
        if (modifiedUser.email === '') {
            currentError.modifiedEmail = 'Email is required';
            isValid = false;
        }
        else if (!/\S+@\S+\.\S+/.test(modifiedUser.email)) {
            currentError.modifiedEmail = 'Enter a valid email';
            isValid = false;
        }
        if (modifiedUser.password.length < 8 && modifiedUser.password !== '') {
            currentError.modifiedPassword = 'Password should be of minimum 8 characters length';
            isValid = false;
        }
        if (modifiedUser.role === null) {
            currentError.modifiedRole = 'Role is required';
            isValid = false;
        }
        else if (modifiedUser.role.id !== 1 && modifiedUser.role.id !== 2 && modifiedUser.role.id !== 3) {
            currentError.modifiedRole = 'Invalid role';
            isValid = false;
        }
        else if (selectedUser.role.id === 2 && modifiedUser.role.id === 1) {
            currentError.modifiedRole = 'Cannot change Seller to Client';
            isValid = false;
        }
        else if (selectedUser.role.id === 3 && modifiedUser.role.id === 1) {
            currentError.modifiedRole = 'Cannot change Admin to Client';
            isValid = false;
        }
        setError(currentError);
        if (!isValid) {
            return;
        }
        setDialogTitle("Modify User");
        setDialogText(`Are you sure you want to modify the user "${selectedUser.name}"?`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleModify);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleModify = () => {
        setDialogYesDisabled(true);
        let updatedUser = {
            Id: modifiedUser.id,
            Email: modifiedUser.email === selectedUser.email ? null : modifiedUser.email,
            Password: modifiedUser.password === '' ? null : modifiedUser.password,
            Name: modifiedUser.name === selectedUser.name ? null : modifiedUser.name,
            Role: modifiedUser.role.id === selectedUser.role.id ? null : modifiedUser.role.id,
            ProfilePictureUrl: modifiedUser.profilePictureUrl === 'delete' ? 'delete' : null
        }
        updateUser(updatedUser, modifiedProfilePicture)
            .then((result) => {
                setDialogText(`User "${modifiedUser.name}" modified successfully.`);
                setSelectedUser(null);
                setModifiedUser({
                    id: null,
                    email: '',
                    password: '',
                    name: '',
                    role: 1,
                    profilePictureUrl: '',
                })
                setModifiedProfilePicture(null);
                setUsers(users.map((user) => {
                    if (user.id === modifiedUser.id) {
                        return result;
                    }
                    return user;
                }));
            })
            .catch((error) => {
                setDialogText(`Error modifying user "${selectedUser.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    const handleDeleteDialog = () => {
        setDialogTitle("Delete User");
        setDialogText(`Are you sure you want to delete the user "${selectedUser.name}"? This action is irreversible.`);
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleDelete);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleDelete = () => {
        setDialogYesDisabled(true);
        deleteUser(selectedUser.id)
            .then((result) => {
                setDialogText(`User "${modifiedUser.name}" deleted successfully.`);
                setSelectedUser(null);
                setModifiedUser({
                    id: null,
                    email: '',
                    password: '',
                    name: '',
                    role: 1,
                    profilePictureUrl: '',
                })
                setModifiedProfilePicture(null);
                setUsers(users.filter((user) => user.id !== selectedUser.id));
            })
            .catch((error) => {
                setDialogText(`Error deleting user "${selectedUser.name}".`);
                console.error(error);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            })
    }

    console.log(modifiedUser)

    return (
        <React.Fragment>
            <Grid item xs={12}>
                <CommonPaper title={"User Panel"}>
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
                                    label={"Email"}
                                    value={newUser.email}
                                    onChange={(event) => setNewUser({ ...newUser, email: event.target.value})}
                                    fullWidth
                                    error={!!error.newEmail}
                                    helperText={error.newEmail}
                                    autoComplete={"off"}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label={"Password"}
                                    value={newUser.password}
                                    type={"password"}
                                    onChange={(event) => setNewUser({ ...newUser, password: event.target.value})}
                                    fullWidth
                                    error={!!error.newPassword}
                                    helperText={error.newPassword}
                                    autoComplete={"off"}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <TextField
                                    label={"Name"}
                                    value={newUser.name}
                                    onChange={(event) => setNewUser({ ...newUser, name: event.target.value})}
                                    fullWidth
                                    error={!!error.newName}
                                    helperText={error.newName}
                                    autoComplete={"off"}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Autocomplete
                                    options={roles}
                                    getOptionLabel={(option) => option.name}
                                    onChange={(event, newValue) => {
                                        setNewUser({...newUser, role: newValue})
                                    }}
                                    value={newUser.role}
                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                    renderInput={(params) => <TextField
                                        {...params}
                                        label="Role"
                                        error={!!error.newRole}
                                        helperText={error.newRole}
                                    />}
                                />
                            </Grid>
                            <Grid item xs={3}>
                                <Button
                                    variant={"contained"}
                                    component={"label"}
                                    disabled={uploadingNewProfilePicture}
                                >
                                    Upload image
                                    <input
                                        type="file"
                                        hidden
                                        accept="image/*"
                                        onChange={handleNewProfilePicture}
                                    />
                                </Button>
                            </Grid>
                            <Grid item xs={3}>
                                {newProfilePicture && (
                                    <Typography sx={{ ml: 2 }}>{newProfilePicture.name}</Typography>
                                )}
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
                                            options={users.sort((a, b) => -b.name.localeCompare(a.name))}
                                            getOptionLabel={(option) => option.name}
                                            onChange={(event, newValue) => {
                                                if (newValue) {
                                                    setSelectedUser({
                                                        ...newValue,
                                                        role: roles.find((role) => role.id === newValue.role),
                                                        password: ''
                                                    });
                                                    setModifiedUser({
                                                        ...newValue,
                                                        role: roles.find((role) => role.id === newValue.role),
                                                        password: ''
                                                    })
                                                }
                                                else {
                                                    setSelectedUser(null);
                                                    setModifiedUser({
                                                        id: null,
                                                        email: '',
                                                        password: '',
                                                        name: '',
                                                        role: 1,
                                                        profilePictureUrl: '',
                                                    });
                                                }
                                            }}
                                            value={selectedUser}
                                            isOptionEqualToValue={(option, value) => option.id === value.id}
                                            renderInput={(params) => <TextField {...params} label="Select a user" />}
                                        />
                                    </Grid>
                                    <Grid item xs={9} />
                                    {selectedUser && (
                                        <React.Fragment>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Email"}
                                                    value={modifiedUser.email}
                                                    onChange={(event) => setModifiedUser({ ...modifiedUser, email: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedEmail}
                                                    helperText={error.modifiedEmail}
                                                    autoComplete={"off"}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Password"}
                                                    value={modifiedUser.password}
                                                    type={"password"}
                                                    onChange={(event) => setModifiedUser({ ...modifiedUser, password: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedPassword}
                                                    helperText={error.modifiedPassword}
                                                    autoComplete={"off"}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <TextField
                                                    label={"Name"}
                                                    value={modifiedUser.name}
                                                    onChange={(event) => setModifiedUser({...modifiedUser, name: event.target.value})}
                                                    fullWidth
                                                    error={!!error.modifiedName}
                                                    helperText={error.modifiedName}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <Autocomplete
                                                    options={roles}
                                                    getOptionLabel={(option) => option.name}
                                                    onChange={(event, newValue) => {
                                                        setModifiedUser({...modifiedUser, role: newValue})
                                                    }}
                                                    value={modifiedUser.role}
                                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                                    renderInput={(params) => <TextField
                                                        {...params}
                                                        label="Role"
                                                        error={!!error.modifiedRole}
                                                        helperText={error.modifiedRole}
                                                    />}
                                                />
                                            </Grid>
                                            <Grid item xs={3}>
                                                <Button
                                                    variant={"contained"}
                                                    component={"label"}
                                                    disabled={uploadingModifiedProfilePicture || modifiedUser.profilePictureUrl === 'delete'}
                                                >
                                                    Upload image
                                                    <input
                                                        type="file"
                                                        hidden
                                                        accept="image/*"
                                                        onChange={handleModifiedProfilePicture}
                                                    />
                                                </Button>
                                            </Grid>
                                            <Grid item xs={3}>
                                                {modifiedProfilePicture && (
                                                    <Typography sx={{ ml: 2 }}>{modifiedProfilePicture.name}</Typography>
                                                )}
                                            </Grid>
                                            <Grid item xs={3}>
                                                <Button
                                                    variant={"contained"}
                                                    component={"label"}
                                                    onClick = {() => {
                                                        setModifiedUser({...modifiedUser, profilePictureUrl: 'delete'});
                                                    }}
                                                    disabled={uploadingModifiedProfilePicture || modifiedProfilePicture !== null || modifiedUser.profilePictureUrl === 'delete'}
                                                >
                                                    Remove profile picture
                                                </Button>
                                            </Grid>
                                            <Grid item xs={3}>
                                                {modifiedUser.profilePictureUrl === 'delete' && (
                                                    <Typography sx={{ ml: 2 }}>Profile picture removed</Typography>
                                                )}
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
                                                <Button
                                                    variant={"contained"}
                                                    color={"error"}
                                                    onClick={handleDeleteDialog}
                                                >
                                                    Delete user
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

export default UserPanel;