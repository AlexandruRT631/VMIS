import {Box, Grid, IconButton, Paper, Typography} from "@mui/material";
import {Link} from "react-router-dom";
import React from "react";
import AddRoadIcon from '@mui/icons-material/AddRoad';
import LocalGasStationIcon from '@mui/icons-material/LocalGasStation';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import NavigationButtons from "../../common/navigation-buttons";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const ListingThumbnail = ({listing}) => {
    const numberFormat = new Intl.NumberFormat('en-US');
    const [selectedImage, setSelectedImage] = React.useState(0);

    const handleNext = () => {
        if (selectedImage === Math.min(listing.listingImages.length - 1, 5)) {
            setSelectedImage(0);
        }
        else {
            setSelectedImage(selectedImage + 1);
        }
    }

    const handlePrev = () => {
        if (selectedImage === 0) {
            setSelectedImage(Math.min(listing.listingImages.length - 1, 5));
        }
        else {
            setSelectedImage(selectedImage - 1);
        }
    }

    return (
        <Link to={`/listing/${listing.id}`} style={{textDecoration: 'none'}}>
            <Box
                sx={{
                    cursor: 'pointer',
                    overflow: 'hidden',
                    position: 'relative',
                    width: '100%',
                    paddingTop: '20%',
                }}
            >
                <Paper sx={{position: 'absolute', top: 0, left: 0, bottom: 0, width: '100%', height: '100%'}}>
                    <Grid
                        container
                        direction={"row"}
                        justifyContent={"center"}
                        alignItems={"stretch"}
                        sx={{height: '100%', margin: 0}}
                    >
                        <Grid item xs={3} sx={{height: '100%'}}>
                            <Box
                                sx={{
                                    height: '100%',
                                    width: '100%',
                                    position: 'relative'
                                }}
                            >
                            <Box
                                component="img"
                                src={BASE_URL + listing.listingImages[selectedImage].url}
                                alt={listing.listingImages[selectedImage].url}
                                sx={{
                                    objectFit: 'cover',
                                    height: '100%',
                                    width: '100%'
                                }}
                            />
                                <NavigationButtons handleNext={handleNext} handlePrev={handlePrev}/>
                            </Box>
                        </Grid>
                        <Grid item xs={6} sx={{display: 'flex', flexDirection: 'column', padding: 2}}>
                            <Typography
                                sx={{
                                    fontWeight: 'bold',
                                    fontSize: 'calc(0.6em + 0.75vw)'
                                }}
                            >
                                {listing.title}
                            </Typography>
                            <Typography
                                sx={{
                                    fontSize: 'calc(0.3em + 0.75vw)'
                                }}
                            >
                                {numberFormat.format(listing.engine.power)} hp
                                • {numberFormat.format(listing.engine.displacement)} cm<sup>3</sup> • {numberFormat.format(listing.engine.torque)} nm
                            </Typography>
                            <Box sx={{ flexGrow: 1 }}></Box>
                            <Box sx={{display: 'flex', alignItems: 'center', mt: 1}}>
                                <AddRoadIcon sx={{mr: 0.5, fontSize: 'calc(0.3em + 0.75vw)' }}/>
                                <Typography sx={{ fontSize: 'calc(0.3em + 0.75vw)' }}>
                                    {numberFormat.format(listing.mileage)}
                                </Typography>
                            </Box>
                            <Box sx={{display: 'flex', alignItems: 'center', mt: 1}}>
                                <LocalGasStationIcon sx={{mr: 0.5, fontSize: 'calc(0.3em + 0.75vw)' }}/>
                                <Typography sx={{ fontSize: 'calc(0.3em + 0.75vw)' }}>
                                    {listing.engine.fuel.name}
                                </Typography>
                            </Box>
                            <Box sx={{display: 'flex', alignItems: 'center', mt: 1}}>
                                <CalendarMonthIcon sx={{mr: 0.5, fontSize: 'calc(0.3em + 0.75vw)' }}/>
                                <Typography sx={{ fontSize: 'calc(0.3em + 0.75vw)' }}>
                                    {listing.year}
                                </Typography>
                            </Box>
                        </Grid>
                        <Grid item xs={3} sx={{display: 'flex', padding: 2, justifyContent: 'flex-end'}}>
                            <Typography
                                sx={{
                                    fontWeight: 'bold',
                                    fontSize: 'calc(1em + 0.75vw)'
                                }}
                            >
                                {numberFormat.format(listing.price)} €
                            </Typography>
                        </Grid>
                    </Grid>
                </Paper>
            </Box>
        </Link>
    );
}

export default ListingThumbnail;