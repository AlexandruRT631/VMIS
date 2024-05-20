import {useParams} from "react-router-dom";
import {getListingById} from "./api/listing-api";
import {useEffect, useState} from "react";
import {
    Box,
    Grid,
    Paper, Typography
} from "@mui/material";
import ListingTechnicalData from "./listing-technical-data";
import ListingImages from "./listing-images";

const Listing = () => {
    const {id} = useParams();
    const [listing, setListing] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        getListingById(id)
            .then(setListing)
            .catch(setError);
    }, [id]);

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    if (!listing) {
        return <div>Loading...</div>;
    }

    console.log(listing);

    return (
        <Box>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={4}
                mb={4}
            >
                <Grid item xs={8}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <Paper variant={"outlined"} sx={{p: 2}}>
                                <ListingImages listing={listing} />
                            </Paper>
                        </Grid>
                            <Grid item xs={12}>
                            <Paper variant={"outlined"}>
                                <Typography>TEST</Typography>
                                <Typography>TEST</Typography>
                                <Typography>TEST</Typography>
                                <Typography>TEST</Typography>
                                <Typography>TEST</Typography>
                                <Typography>TEST</Typography>
                            </Paper>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={4}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <ListingTechnicalData listing={listing} />
                        </Grid>
                        <Grid item xs={12}>
                            <ListingTechnicalData listing={listing} />
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    );
};

export default Listing;
