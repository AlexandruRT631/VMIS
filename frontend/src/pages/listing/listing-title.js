import {
    Avatar,
    Button,
    Dialog, DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    Link,
    Paper,
    Stack, TextField,
    Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import {getUserDetails} from "../../api/user-api";
import {getUserId} from "../../common/token";
import {deleteListing, updateListing} from "../../api/listing-api";
import {addFavouriteListing, removeFavouriteListing} from "../../api/favourite-api";
import {sendMessage} from "../../api/messages-api";

const BASE_URL = process.env.REACT_APP_USER_API_URL;

const ListingTitle = ({ listing }) => {
    const [loading, setLoading] = useState(true);
    const [userDetails, setUserDetails] = useState(null);
    const [currentUserDetails, setCurrentUserDetails] = useState(null);
    const [userId, setUserId] = useState(null);
    const [userRole, setUserRole] = useState(null);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [markDialogOpen, setMarkDialogOpen] = useState(false);
    const numberFormat = new Intl.NumberFormat('en-US');
    const [isSaveDisabled, setIsSaveDisabled] = useState(false);
    const [contactDialogOpen, setContactDialogOpen] = useState(false);
    const [contactMessage, setContactMessage] = useState('');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedUserId = getUserId();
                setUserId(fetchedUserId);
                const user = await getUserDetails(listing.sellerId);
                setUserDetails(user);
                if (fetchedUserId) {
                    const currentUser = await getUserDetails(fetchedUserId);
                    setCurrentUserDetails(currentUser);
                    setUserRole(currentUser.role)
                }
            } catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false));
    }, [listing.sellerId]);

    const handleDeleteConfirm = () => {
        deleteListing(listing.id)
            .then(() => {
                window.location.href = '/';
            })
            .catch(console.error);
    }

    const handleListingMark = () => {
        updateListing({
            Id: listing.id,
            IsSold: !listing.isSold
        }, null)
            .then(() => {
                window.location.reload();
            })
            .catch(console.error);
    }

    console.log(userDetails);

    const handleSave = () => {
        setIsSaveDisabled(true);
        if (currentUserDetails.favouriteListings.includes(listing.id)) {
            removeFavouriteListing(currentUserDetails.id, listing.id)
                .then(() => {
                    const index = currentUserDetails.favouriteListings.indexOf(listing.id);
                    currentUserDetails.favouriteListings.splice(index, 1);
                })
                .catch(console.error)
                .finally(() => setIsSaveDisabled(false));
        }
        else {
            addFavouriteListing(currentUserDetails.id, listing.id)
                .then(() => {
                    currentUserDetails.favouriteListings.push(listing.id);
                })
                .catch(console.error)
                .finally(() => setIsSaveDisabled(false));
        }
    }

    const handleContactSeller = () => {
        sendMessage({
            senderId: currentUserDetails.id,
            content: contactMessage,
        }, listing.id)
            .then(() => {
                window.location.href = '/messages';
            })
            .catch(console.error);
    }

    if (loading) {
        return;
    }

    return (
        <Paper variant="outlined" sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
            {listing.isSold && (
                <Typography variant="h6" color="error" gutterBottom>
                    {"(Inactive) "}
                </Typography>
            )}
            <Typography variant="h4" color="primary" gutterBottom sx={{ fontWeight: 'bold' }}>
                {listing.title}
            </Typography>
            <Grid container>
                <Grid item xs={6}>
                    <Typography variant="h6" >Price</Typography>
                    <Typography variant="h4" >{numberFormat.format(listing.price)} €</Typography>
                </Grid>
                <Grid item xs={6}>
                    <Typography variant="h6" >Seller</Typography>
                    <Link href={'/profile/' + userDetails.id} sx={{ textDecoration: 'none', color: 'inherit' }}>
                        <Stack direction={"row"} alignItems="center" spacing={2}>
                            <Avatar src={BASE_URL + userDetails.profilePictureUrl} />
                            <Typography variant="h6" >{userDetails.name}</Typography>
                        </Stack>
                    </Link>
                </Grid>
            </Grid>
            <Grid container sx={{mt: 2}} spacing={2}>
                {userId && userId !== listing.sellerId && (
                    <React.Fragment>
                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                sx={{
                                    '&:focus': {
                                        outline: 'none',
                                    },
                                }}
                                onClick={handleSave}
                                disabled={isSaveDisabled}
                            >
                                {currentUserDetails.favouriteListings.includes(listing.id) ? "Unsave listing" : "Save listing"}
                            </Button>
                        </Grid>
                        {!listing.isSold && (
                            <Grid item xs={6}>
                                {!currentUserDetails.conversations.some(conversation => conversation.listingId === listing.id) && (
                                    <Button
                                        variant="contained"
                                        sx={{
                                            '&:focus': {
                                                outline: 'none',
                                            },
                                        }}
                                        onClick={() => setContactDialogOpen(true)}
                                    >
                                        Contact Seller
                                    </Button>
                                )}
                            </Grid>
                        )}
                    </React.Fragment>
                )}

                {userId && (userId === listing.sellerId || userRole === "Admin") && (
                    <React.Fragment>
                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                sx={{
                                    '&:focus': {
                                        outline: 'none',
                                    },
                                }}
                                onClick={() => setMarkDialogOpen(true)}
                            >
                                {listing.isSold ? "Mark as available" : "Mark as sold"}
                            </Button>
                        </Grid>
                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                sx={{
                                    '&:focus': {
                                        outline: 'none',
                                    },
                                }}
                                onClick={() => window.location.href = '/modifyListing/' + listing.id}
                            >
                                Modify Listing
                            </Button>
                        </Grid>
                        <Grid item xs={6}>
                            <Button
                                variant="contained"
                                color="error"
                                sx={{
                                    '&:focus': {
                                        outline: 'none',
                                    },
                                }}
                                onClick={() => setDeleteDialogOpen(true)}
                            >
                                Delete Listing
                            </Button>
                        </Grid>
                    </React.Fragment>
                )}
            </Grid>

            <Dialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
            >
                <DialogTitle>Confirm Delete</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Are you sure you want to delete this listing? This action cannot be undone.
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialogOpen(false)} color="primary">
                        No
                    </Button>
                    <Button onClick={handleDeleteConfirm} color="error" variant="contained">
                        Yes
                    </Button>
                </DialogActions>
            </Dialog>

            <Dialog
                open={markDialogOpen}
                onClose={() => setMarkDialogOpen(false)}
            >
                <DialogTitle>Confirm Mark</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Are you sure you want to mark this listing as {listing.isSold ? "available" : "sold"}?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setMarkDialogOpen(false)} color="primary">
                        No
                    </Button>
                    <Button onClick={handleListingMark} color="primary" variant="contained">
                        Yes
                    </Button>
                </DialogActions>
            </Dialog>

            <Dialog
                open={contactDialogOpen}
                onClose={() => setContactDialogOpen(false)}
            >
                <DialogTitle>Contact Seller</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        required
                        margin="dense"
                        label="Message"
                        fullWidth
                        variant="standard"
                        onChange={(e) => setContactMessage(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setContactDialogOpen(false)} color="primary">
                        Cancel
                    </Button>
                    <Button
                        onClick={handleContactSeller}
                        disabled={contactMessage.length === 0}
                    >
                        Send
                    </Button>
                </DialogActions>
            </Dialog>
        </Paper>
    );
}

export default ListingTitle;