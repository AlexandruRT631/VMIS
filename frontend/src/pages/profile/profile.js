import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {getUserDetails} from "../../api/user-api";
import {Avatar, Box, Button, Grid, Paper, Tab, Tabs, Typography} from "@mui/material";
import {getActiveListingsByUserId, getInactiveListingsByUserId} from "../../api/listing-api";
import ListingThumbnail from "../../common/listing/listing-thumbnail";
import NavigateBeforeIcon from "@mui/icons-material/NavigateBefore";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import {getUserId} from "../../common/token";

const BASE_URL = process.env.REACT_APP_USER_API_URL;

const Profile = () => {
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
    const [userId, setUserId] = useState(null);
    const [user, setUser] = useState(null);
    const [pageActive, setPageActive] = useState(1);
    const [listingsActive, setListingsActive] = useState([]);
    const [pageInactive, setPageInactive] = useState(1);
    const [listingsInactive, setListingsInactive] = useState([]);
    const [tab, setTab] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setUserId(getUserId());
                const user = await getUserDetails(id);
                setUser(user);
                const activeListings = await getActiveListingsByUserId(id, pageActive);
                setListingsActive(activeListings);
                const inactiveListings = await getInactiveListingsByUserId(id, pageInactive);
                setListingsInactive(inactiveListings);
            } catch (error) {
                console.error(error);
            }
        };
        fetchData()
            .then(() => setLoading(false));
    }, [id]);

    useEffect(() => {
        if (pageActive >= 1) {
            getActiveListingsByUserId(id, pageActive)
                .then(setListingsActive)
                .catch(console.error);
        }
    }, [pageActive]);

    useEffect(() => {
        if (pageInactive >= 1) {
            getInactiveListingsByUserId(id, pageInactive)
                .then(setListingsInactive)
                .catch(console.error);
        }
    }, [pageInactive]);

    console.log(tab);
    console.log(user);
    console.log(listingsActive);
    console.log(listingsInactive);

    if (loading) {
        return;
    }

    return (
        <Grid
            container
            spacing={4}
            mt={4}
        >
            <Grid item xs={3}>
                <Paper>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            justifyContent: 'center',
                            alignItems: 'center',
                            height: '100%',
                            p: 2,
                        }}
                    >
                        <Avatar
                            src={BASE_URL + user.profilePictureUrl}
                            sx={{
                                width: "80%",
                                height: "auto",
                            }}
                        />
                        <Typography
                            variant={"h4"}
                            align={"center"}
                            mt={2}
                        >
                            {user.name}
                        </Typography>
                        {userId && userId === id && (
                            <Button
                                variant="contained"
                            >
                                Add to favourites
                            </Button>
                        )}
                    </Box>
                </Paper>
            </Grid>
            <Grid item xs={9}>
                <Box>
                    <Typography variant={"h2"}>Listings</Typography>
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
                            <Tab label="Active Listings" />
                            <Tab label="Inactive Listings" />
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
                    {(tab === 0 ? listingsActive : listingsInactive).listings.map((listing, index) => (
                        <Grid key={index} item xs={12}>
                            <ListingThumbnail listing={listing} />
                        </Grid>
                    ))}
                    <Grid
                        container
                        direction={"row"}
                        justifyContent={"center"}
                        alignItems={"stretch"}
                        spacing={2}
                        mt={2}
                        mb={2}
                    >
                        <Grid item>
                            <Button
                                variant="contained"
                                onClick={() => {
                                    if (tab === 0 && pageActive > 1) {
                                        setPageActive(pageActive - 1);
                                    }
                                    else if (tab === 1 && pageInactive > 1) {
                                        setPageInactive(pageInactive - 1);
                                    }
                                }}
                                disabled={(tab === 0 && pageActive <= 1) || (tab === 1 && pageInactive <= 1)}
                            >
                                <NavigateBeforeIcon />
                            </Button>
                        </Grid>
                        <Grid item>
                            <Typography>
                                Page {tab === 0 ? pageActive : pageInactive} of {tab === 0 ? listingsActive.totalPages : listingsInactive.totalPages}
                            </Typography>
                        </Grid>
                        <Grid item>
                            <Button
                                variant="contained"
                                onClick={() => {
                                    if (tab === 0 && pageActive < listingsActive.totalPages) {
                                        setPageActive(pageActive + 1);
                                    }
                                    else if (tab === 1 && pageInactive < listingsInactive.totalPages) {
                                        setPageInactive(pageInactive + 1);
                                    }
                                }}
                                disabled={(tab === 0 && pageActive >= listingsActive.totalPages) || (tab === 1 && pageInactive >= listingsInactive.totalPages)}
                            >
                                <NavigateNextIcon />
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}

export default Profile;