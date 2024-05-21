import React, {useRef, useState} from 'react';
import {Box, Stack, Button, useTheme, IconButton} from '@mui/material';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const ListingImages = ({listing}) => {
    const [selectedImage, setSelectedImage] = useState(0);
    const theme = useTheme();
    const refs = useRef([]);

    const handleNext = () => {
        setSelectedImage((prevIndex) => {
            const newIndex = prevIndex === listing.listingImages.length - 1 ? 0 : prevIndex + 1;
            refs.current[newIndex].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
            return newIndex;
        });
    };

    const handlePrev = () => {
        setSelectedImage((prevIndex) => {
            const newIndex = prevIndex === 0 ? listing.listingImages.length - 1 : prevIndex - 1;
            refs.current[newIndex].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
            return newIndex;
        });
    };

    const handleSelectImage = (index) => {
        setSelectedImage(index);
        refs.current[index].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
    };

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
                    src={BASE_URL + listing.listingImages[selectedImage].url}
                    alt={BASE_URL + listing.listingImages[selectedImage].url}
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%',
                        objectFit: 'cover',
                    }}
                />
                <IconButton
                    onClick={handlePrev}
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '10px',
                        transform: 'translateY(-50%)',
                        backgroundColor: 'rgba(0,0,0,0.5)',
                        color: 'white',
                        '&:hover': {
                            backgroundColor: 'rgba(0,0,0,0.7)',
                        },
                    }}
                >
                    <NavigateBeforeIcon />
                </IconButton>
                <IconButton
                    onClick={handleNext}
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        right: '10px',
                        transform: 'translateY(-50%)',
                        backgroundColor: 'rgba(0,0,0,0.5)',
                        color: 'white',
                        '&:hover': {
                            backgroundColor: 'rgba(0,0,0,0.7)',
                        },
                    }}
                >
                    <NavigateNextIcon />
                </IconButton>
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
                {listing.listingImages.map((image, index) => (
                    <Button
                        key={index}
                        onClick={() => handleSelectImage(index)}
                        sx={{
                            padding: 0,
                            '&:focus': { outline: 'none' },
                        }}
                        ref={el => refs.current[index] = el}
                    >
                        <Box
                            component="img"
                            src={BASE_URL + image.url}
                            alt={image.url}
                            height={75}
                            width={75}
                            sx={{
                                objectFit: 'cover',
                                filter: selectedImage === index ? 'none' : 'brightness(0.5)',
                                border: selectedImage === index ? `2px solid ${theme.palette.secondary.main}` : 'none',
                            }}
                        />
                    </Button>
                ))}
            </Stack>
        </React.Fragment>
    );
}

export default ListingImages;