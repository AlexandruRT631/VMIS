import React from "react";
import {
    Box,
    Button,
    Dialog, DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    TextField,
    Typography
} from "@mui/material";
import CommonPaper from "../../common/common-paper";
import {useEffect, useState} from "react";
import {getUserId, removeToken} from "../../common/token";
import {deleteUser, getUserDetails, updateUser} from "../../api/user-api";

const BASE_URL = process.env.REACT_APP_USER_API_URL;

const Account = () => {
    const [userId, setUserId] = useState(null);
    const [loading, setLoading] = useState(true);
    const [name, setName] = useState('');
    const [nameError, setNameError] = useState('');
    const [profilePicture, setProfilePicture] = useState('');
    const [uploadingImage, setUploadingImage] = useState(false);
    const [image, setImage] = useState(null);
    const [role, setRole] = useState('');
    const [password, setPassword] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [dialogOpen, setDialogOpen] = useState(false);
    const [dialogTitle, setDialogTitle] = useState('');
    const [dialogText, setDialogText] = useState('');
    const [dialogYesAction, setDialogYesAction] = useState(null);
    const [dialogYesDisabled, setDialogYesDisabled] = useState(false);
    const [dialogAcknowledgement, setDialogAcknowledgement] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedUserId = getUserId();
                setUserId(fetchedUserId);
                if (fetchedUserId) {
                    const fetchedUser = await getUserDetails(fetchedUserId);
                    setName(fetchedUser.name);
                    setProfilePicture(fetchedUser.profilePictureUrl);
                    setRole(fetchedUser.role);
                }
            } catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false));
    }, []);

    const handleNameDialog = () => {
        if (!name) {
            setNameError('Name is required');
            return;
        }
        if (name.length < 3) {
            setNameError('Name should be of minimum 3 characters length');
            return;
        }
        setNameError('');
        setDialogTitle("Name Change");
        setDialogText("Are you sure you want to change your name?");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleNameChange);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleNameChange = () => {
        setDialogYesDisabled(true);
        updateUser({
            Id: parseInt(userId),
            Name: name
        }, null)
            .then(() => {
                setDialogText("Name changed successfully");
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
            });
    }

    const handlePictureRemoveDialog = () => {
        setDialogTitle("Picture Change");
        setDialogText("Are you sure you want to remove your profile picture?");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handlePictureRemove);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handlePictureRemove = () => {
        setDialogYesDisabled(true);
        updateUser({
            Id: parseInt(userId),
            ProfilePictureUrl: 'delete'
        }, null)
            .then(() => {
                window.location.reload();
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
                setDialogAcknowledgement(true);
            });
    }

    const handleImageChange = async (event) => {
        setUploadingImage(true);
        setImage(event.target.files[0]);
        setUploadingImage(false);
    }

    const handlePictureChangeDialog = () => {
        setDialogTitle("Picture Change");
        setDialogText("Are you sure you want to change your profile picture?");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handlePictureChange);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handlePictureChange = () => {
        setDialogYesDisabled(true);
        updateUser({
            Id: parseInt(userId)
        }, image)
            .then(() => {
                window.location.reload();
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
                setDialogAcknowledgement(true);
            });
    }

    const handleRoleUpgradeDialog = () => {
        setDialogTitle("Role Upgrade");
        setDialogText("Are you sure you want to upgrade your role to Seller? This action is irreversible.");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleRoleUpgrade);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleRoleUpgrade = () => {
        setDialogYesDisabled(true);
        updateUser({
            Id: parseInt(userId),
            Role: 2
        }, null)
            .then(() => {
                window.location.reload();
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
                setDialogAcknowledgement(true);
            });
    }

    const handlePasswordDialog = () => {
        if (!password) {
            setPasswordError('Password is required');
            return;
        }
        if (password.length < 8) {
            setPasswordError('Password must be at least 8 characters long');
            return;
        }
        setPasswordError('');
        setDialogTitle("Password Change");
        setDialogText("Are you sure you want to change your password?");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handlePasswordChange);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handlePasswordChange = () => {
        setDialogYesDisabled(true);
        updateUser({
            Id: parseInt(userId),
            Password: password
        }, null)
            .then(() => {
                setDialogText("Password changed successfully");
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
            })
            .finally(() => {
                setDialogAcknowledgement(true);
                setPassword('');
            });
    }

    const handleDeleteAccountDialog = () => {
        setDialogTitle("Delete Account");
        setDialogText("Are you sure you want to delete your account? This action is irreversible.");
        setDialogAcknowledgement(false);
        setDialogYesAction(() => handleDeleteAccount);
        setDialogYesDisabled(false);
        setDialogOpen(true);
    }

    const handleDeleteAccount = () => {
        setDialogYesDisabled(true);
        deleteUser(parseInt(userId))
            .then(() => {
                removeToken();
                window.location.href = '/';
            })
            .catch((error) => {
                console.error(error);
                setDialogText(error.message);
                setDialogAcknowledgement(true);
            });
    }

    if (loading) {
        return;
    }

    if (!userId) {
        return (
            <Typography variant={"h4"}>You must be logged in to change account details</Typography>
        );
    }

    return (
        <Box>
            <Typography variant={"h2"}>Account</Typography>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={2}
                mb={2}
            >
                <Grid item xs={12}>
                    <CommonPaper title={"Change Name"}>
                        <Grid
                            container
                            spacing={2}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center'
                            }}
                        >
                            <Grid item xs={6}>
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        minHeight: '106px',
                                        justifyContent: 'space-between'
                                    }}
                                >
                                    <TextField
                                        margin="normal"
                                        fullWidth
                                        label={"Name"}
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        error={!!nameError}
                                        helperText={nameError}
                                    />
                                </Box>
                            </Grid>
                            <Grid item xs={6}>
                                <Button
                                    variant="contained"
                                    onClick={handleNameDialog}
                                >
                                    Change name
                                </Button>
                            </Grid>
                        </Grid>
                    </CommonPaper>
                </Grid>
                <Grid item xs={12}>
                    <CommonPaper title={"Change Profile Picture"}>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                minHeight: '106px',
                            }}
                        >
                            <Box
                                component={"img"}
                                src={BASE_URL + profilePicture}
                                alt={profilePicture}
                                sx={{
                                    width: "106px",
                                    height: "106px",
                                }}
                            />
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    minHeight: '106px',
                                    justifyContent: 'space-between',
                                    marginLeft: 2,
                                    flexGrow: 1
                                }}
                            >
                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                    <Button
                                        variant={"contained"}
                                        component={"label"}
                                        disabled={uploadingImage}
                                    >
                                        Upload image
                                        <input
                                            type="file"
                                            hidden
                                            accept="image/*"
                                            onChange={handleImageChange}
                                        />
                                    </Button>
                                    {image && (
                                        <React.Fragment>
                                            <Typography sx={{ ml: 2 }}>{image.name}</Typography>
                                            <Button
                                                variant={"contained"}
                                                sx={{ ml: 2, whiteSpace: 'nowrap' }}
                                                onClick={handlePictureChangeDialog}
                                            >
                                                Upload
                                            </Button>
                                        </React.Fragment>
                                    )}
                                </Box>
                                <Box sx={{ flexGrow: 1}} />
                                <Button
                                    variant={"contained"}
                                    onClick={handlePictureRemoveDialog}
                                    sx={{ alignSelf: 'flex-start' }}
                                >
                                    Remove profile picture
                                </Button>
                            </Box>
                        </Box>
                    </CommonPaper>
                </Grid>
                {role === "Client" && (
                    <Grid item xs={12}>
                        <CommonPaper title={"Change Role"}>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    minHeight: '106px',
                                    alignItems: 'flex-start',
                                }}
                            >
                                <Button
                                    variant={"contained"}
                                    sx={{ alignSelf: 'flex-start' }}
                                    onClick={handleRoleUpgradeDialog}
                                >
                                    Upgrade to Seller
                                </Button>
                            </Box>

                        </CommonPaper>
                    </Grid>
                )}
                <Grid item xs={12}>
                    <CommonPaper title={"Change Password"}>
                        <Grid
                            container
                            spacing={2}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center'
                            }}
                        >
                            <Grid item xs={6}>
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        minHeight: '106px',
                                        justifyContent: 'space-between'
                                    }}
                                >
                                    <TextField
                                        margin="normal"
                                        fullWidth
                                        label={"Password"}
                                        type={"password"}
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        error={!!passwordError}
                                        helperText={passwordError}
                                    />
                                </Box>
                            </Grid>
                            <Grid item xs={6}>
                                <Button
                                    variant="contained"
                                    onClick={handlePasswordDialog}
                                >
                                    Change password
                                </Button>
                            </Grid>
                        </Grid>
                    </CommonPaper>
                </Grid>
                <Grid item xs={12}>
                    <CommonPaper title={"Delete Account"}>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                minHeight: '106px',
                                justifyContent: 'space-between'
                            }}
                        >
                            <Button
                                variant={"contained"}
                                color={"error"}
                                sx={{ alignSelf: 'flex-start' }}
                                onClick={handleDeleteAccountDialog}
                            >
                                Delete account
                            </Button>
                        </Box>
                    </CommonPaper>
                </Grid>
            </Grid>

            <Dialog
                open={dialogOpen}
                onClose={() => setDialogOpen(false)}
            >
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
                            <Button onClick={dialogYesAction} disabled={dialogYesDisabled} color={dialogTitle === "Delete Account" ? "error" : "primary"}>
                                Yes
                            </Button>
                        </React.Fragment>
                    )}
                </DialogActions>
            </Dialog>
        </Box>
    )
}

export default Account;