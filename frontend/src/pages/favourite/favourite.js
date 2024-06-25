import {Box, Button, Grid, Tab, Tabs, Typography} from "@mui/material";
import ListingThumbnail from "../../common/listing/listing-thumbnail";
import NavigateBeforeIcon from "@mui/icons-material/NavigateBefore";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import React, {useEffect, useState} from "react";
import {getUserDetails, getUsersDetailByIds} from "../../api/user-api";
import {getListingsByIds} from "../../api/listing-api";
import {getUserId} from "../../common/token";
import ProfileThumbnail from "./profile-thumbnail";

const Favourite = () => {
    const [userId, setUserId] = useState(null);
    const [loading, setLoading] = useState(true);
    const [tab, setTab] = useState(0);
    const [userDetails, setUserDetails] = useState(null);
    const [favouriteListings, setFavouriteListings] = useState(null);
    const [pageFavouriteListings, setPageFavouriteListings] = useState(1);
    const [favouriteUsers, setFavouriteUsers] = useState(null);
    const [pageFavouriteUsers, setPageFavouriteUsers] = useState(1);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedUserId = getUserId();
                setUserId(fetchedUserId);
                if (fetchedUserId) {
                    const fetchedUserDetails = await getUserDetails(fetchedUserId);
                    setUserDetails(fetchedUserDetails);
                    const fetchedFavoriteListings = await getListingsByIds(fetchedUserDetails.favouriteListings, pageFavouriteListings);
                    setFavouriteListings(fetchedFavoriteListings);
                    if (fetchedFavoriteListings.totalPages === 0) {
                        setPageFavouriteListings(0);
                    }
                    const fetchedFavouriteUsers = await getUsersDetailByIds(fetchedUserDetails.favouriteUsers, pageFavouriteUsers);
                    setFavouriteUsers(fetchedFavouriteUsers);
                    if (fetchedFavouriteUsers.totalPages === 0) {
                        setPageFavouriteUsers(0);
                    }
                }
            } catch (error) {
                console.error(error);
            }
        }

        fetchData()
            .then(() => setLoading(false));
    }, []);

    if (!userId) {
        return (
            <Typography variant={"h4"}>You must be logged in view favourites</Typography>
        );
    }

    if (loading) {
        return;
    }

    return (
        <Grid item xs={9}>
            <Box>
                <Typography variant={"h2"}>Favourites</Typography>
            </Box>
            <Box sx={{ width: '100%' }}>
                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <Tabs
                        value={tab}
                        onChange={(event, newValue) => setTab(newValue)}
                        sx={{
                            '& .MuiTab-root:focus': {
                                outline: 'none',
                            },
                        }}
                    >
                        <Tab label="Listings" />
                        <Tab label="Users" />
                    </Tabs>
                </Box>
            </Box>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={4}
                mb={4}
            >
                {(tab === 0 ? (
                    favouriteListings.listings.map((listing, index) => (
                        <Grid key={index} item xs={12}>
                            <ListingThumbnail listing={listing} />
                        </Grid>
                    ))
                ) : (
                    favouriteUsers.users.map((user, index) => (
                        <Grid key={index} item xs={12}>
                            <ProfileThumbnail profile={user} />
                        </Grid>
                    ))
                ))}
                <Grid
                    container
                    direction={"row"}
                    justifyContent={"center"}
                    alignItems={"center"}
                    spacing={2}
                    mt={2}
                    mb={2}
                >
                    <Grid item>
                        <Button
                            variant="contained"
                            onClick={() => {
                                if (tab === 0 && pageFavouriteListings > 1) {
                                    setPageFavouriteListings(pageFavouriteListings - 1);
                                }
                                else if (tab === 1 && pageFavouriteUsers > 1) {
                                    setPageFavouriteUsers(pageFavouriteUsers - 1);
                                }
                            }}
                            disabled={(tab === 0 && pageFavouriteListings <= 1) || (tab === 1 && pageFavouriteUsers <= 1)}
                        >
                            <NavigateBeforeIcon />
                        </Button>
                    </Grid>
                    <Grid item>
                        <Typography>
                            Page {tab === 0 ? pageFavouriteListings : pageFavouriteUsers} of {tab === 0 ? favouriteListings.totalPages : favouriteUsers.totalPages}
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Button
                            variant="contained"
                            onClick={() => {
                                if (tab === 0 && pageFavouriteListings < favouriteListings.totalPages) {
                                    setPageFavouriteListings(pageFavouriteListings + 1);
                                }
                                else if (tab === 1 && pageFavouriteUsers < favouriteUsers.totalPages) {
                                    setPageFavouriteUsers(pageFavouriteUsers + 1);
                                }
                            }}
                            disabled={(tab === 0 && pageFavouriteListings >= favouriteListings.totalPages) || (tab === 1 && pageFavouriteUsers >= favouriteUsers.totalPages)}
                        >
                            <NavigateNextIcon />
                        </Button>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default Favourite;