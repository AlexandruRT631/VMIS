import {useParams} from "react-router-dom";
import {getListingById} from "../../api/listing-api";
import {useEffect, useState} from "react";
import {
    Box,
    Grid
} from "@mui/material";
import ListingTechnicalData from "./listing-technical-data";
import ListingImages from "./listing-images";
import ListingTitle from "./listing-title";
import ListingFeatures from "./listing-features";
import ListingDescription from "./listing-description";

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
        if (error.response && error.response.status === 404) {
            return <div>Error: listing not found</div>;
        }
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
                <Grid item xs={8} sx={{ position: 'sticky', top: 96, alignSelf: 'flex-start' }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <ListingImages listing={listing} />
                        </Grid>
                        <Grid item xs={12}>
                            <ListingTitle listing={listing} />
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={4}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <ListingTechnicalData listing={listing} />
                        </Grid>
                        <Grid item xs={12}>
                            <ListingFeatures listing={listing} />
                        </Grid>
                        <Grid item xs={12}>
                            <ListingDescription listing={listing} />
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    );
};

export default Listing;
