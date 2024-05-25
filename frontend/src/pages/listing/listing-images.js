import React, {useCallback, useEffect, useRef, useState} from 'react';
import {Box, Stack, Button, useTheme, IconButton, Dialog, Paper} from '@mui/material';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import CloseIcon from '@mui/icons-material/Close';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const NavigationButtons = ({handleNext, handlePrev}) => {
    return (
        <React.Fragment>
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
        </React.Fragment>
    )
}

const ImageList = ({listing, selectedImage, handleSelectImage, refs, theme}) => {
    return (
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
    )
}

const ListingImages = ({listing}) => {
    const [selectedImage, setSelectedImage] = useState(0);
    const [open, setOpen] = useState(false);
    const theme = useTheme();
    const refs = useRef([]);

    const handleNext = useCallback(() => {
        setSelectedImage((prevIndex) => {
            const newIndex = prevIndex === listing.listingImages.length - 1 ? 0 : prevIndex + 1;
            if (refs.current[newIndex]) {
                refs.current[newIndex].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
            }
            return newIndex;
        });
    }, [listing.listingImages.length]);

    const handlePrev = useCallback(() => {
        setSelectedImage((prevIndex) => {
            const newIndex = prevIndex === 0 ? listing.listingImages.length - 1 : prevIndex - 1;
            if (refs.current[newIndex]) {
                refs.current[newIndex].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
            }
            return newIndex;
        });
    }, [listing.listingImages.length]);

    const handleSelectImage = (index) => {
        setSelectedImage(index);
        if (refs.current[index]) {
            refs.current[index].scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
        }
    };

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleKeyDown = useCallback((event) => {
        if (event.key === 'ArrowRight') {
            handleNext();
        } else if (event.key === 'ArrowLeft') {
            handlePrev();
        }
    }, [handleNext, handlePrev]);

    useEffect(() => {
        if (open) {
            window.addEventListener('keydown', handleKeyDown);
        } else {
            window.removeEventListener('keydown', handleKeyDown);
        }
        return () => {
            window.removeEventListener('keydown', handleKeyDown);
        };
    }, [open, handleKeyDown]);

    return (
        <Paper variant={"outlined"} sx={{p: 2}}>
            <Box
                sx={{
                    width: '100%',
                    position: 'relative',
                    paddingTop: '70%',
                }}
            >
                <Box
                    component="img"
                    onClick={handleOpen}
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
                <NavigationButtons handleNext={handleNext} handlePrev={handlePrev} />
            </Box>
            <ImageList listing={listing} selectedImage={selectedImage} handleSelectImage={handleSelectImage} refs={refs} theme={theme} />
            <Dialog
                open={open}
                onClose={handleClose}
                maxWidth={"xl"}
                PaperProps={{
                    style: {
                        margin: 0,
                        width: '85%',
                        height: '85%',
                        backgroundColor: '#000000',
                    },
                }}
            >
                <Box
                    sx={{
                        width: '100%',
                        height: '100%',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        alignItems: 'center',
                        position: 'relative',
                    }}
                >
                    <Box
                        component="img"
                        src={BASE_URL + listing.listingImages[selectedImage].url}
                        alt={BASE_URL + listing.listingImages[selectedImage].url}
                        sx={{
                            maxWidth: '100%',
                            maxHeight: '100%',
                            width: '85%',
                            height: '85%',
                            objectFit: 'contain',
                            border: `2px solid grey`,
                        }}
                    />
                    <ImageList listing={listing} selectedImage={selectedImage} handleSelectImage={handleSelectImage} refs={refs} theme={theme} />
                    <NavigationButtons handleNext={handleNext} handlePrev={handlePrev} />
                    <IconButton
                        onClick={handleClose}
                        sx={{
                            position: 'absolute',
                            top: '5%',
                            right: '10px',
                            transform: 'translateY(-50%)',
                            backgroundColor: 'rgba(0,0,0,0.5)',
                            color: 'white',
                            '&:hover': {
                                backgroundColor: 'rgba(0,0,0,0.7)',
                            },
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                </Box>
            </Dialog>
        </Paper>
    );
}

export default ListingImages;
