import React, {useState} from 'react';
import {Box, Stack, Button, useTheme} from '@mui/material';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const ListingImages = ({listing}) => {
    const [selectedImage, setSelectedImage] = useState(listing.listingImages[0].url);
    const theme = useTheme();

    return (
        <React.Fragment>
            <Box
                sx={{
                    width: '100%',
                    position: 'relative',
                    paddingTop: '70%',
                }}
            >
                <Box
                    component="img"
                    src={BASE_URL + selectedImage}
                    alt={BASE_URL + selectedImage}
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%',
                        objectFit: 'cover',
                    }}
                />
            </Box>
            <Stack
                direction={"row"}
                spacing={2}
                sx={{
                    overflowX: 'auto',
                    maxWidth: '100%',
                    mt: 2,
                    px: 2,
                }}
            >
                {listing.listingImages.map((image) => (
                    <Button
                        key={image.id}
                        onClick={() => setSelectedImage(image.url)}
                        sx={{
                            padding: 0,
                            '&:focus': { outline: 'none' },
                        }}
                    >
                        <Box
                            component="img"
                            src={BASE_URL + image.url}
                            alt={image.url}
                            height={75}
                            width={75}
                            sx={{
                                objectFit: 'cover',
                                filter: selectedImage === image.url ? 'none' : 'brightness(0.5)',
                                border: selectedImage === image.url ? `2px solid ${theme.palette.secondary.main}` : 'none',
                            }}
                        />
                    </Button>
                ))}
            </Stack>
        </React.Fragment>
    );
}

export default ListingImages;