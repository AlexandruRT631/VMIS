import CommonPaper from "../common-paper";
import React, {useEffect, useRef, useState} from "react";
import {
    Button,
    Grid, IconButton,
    ImageList,
    ImageListItem,
    ImageListItemBar,
    Input,
    InputAdornment,
    InputLabel, TextareaAutosize,
    Typography, useTheme
} from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import {createThumbnail} from "../create-thumbnail";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const ListingSelectFinalDetails = ({listing, setFinalDetails}) => {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [mileage, setMileage] = useState("");
    const [description, setDescription] = useState("");
    const [priceError, setPriceError] = useState(false);
    const [mileageError, setMileageError] = useState(false);
    const [images, setImages] = useState([]);
    const [thumbnails, setThumbnails] = useState([]);
    const [uploadingImages, setUploadingImages] = useState(false);
    const [postingError, setPostingError] = useState(null);
    const componentRef = useRef(null);
    const theme = useTheme();

    useEffect(() => {
        const fetchImages = async () => {
            const imageUrls = listing.listingImages.map(img => BASE_URL + img.url);
            const imageFiles = await Promise.all(
                imageUrls.map(async (url) => {
                    const response = await fetch(url, {
                        method: 'GET',
                        mode: 'cors',
                        cache: 'no-store',
                    });
                    const blob = await response.blob();
                    return new File([blob], url.split('/').pop(), { type: blob.type });
                })
            );
            setImages(imageFiles);

            const thumbnailPromises = imageFiles.map(file => createThumbnail(file));
            const newThumbnails = await Promise.all(thumbnailPromises);
            setThumbnails(newThumbnails);
        };
        if (componentRef.current) {
            componentRef.current.scrollIntoView({behavior: "smooth"});
        }
        if (listing) {
            setTitle(listing.title);
            setPrice(listing.price.toString());
            setMileage(listing.mileage.toString());
            setDescription(listing.description);
            fetchImages();
        }
    }, []);

    const handlePriceChange = (event) => {
        const value = event.target.value;
        setPrice(value);
        if (value !== '' && (isNaN(value) || value < 0)) {
            setPriceError(true);
        } else {
            setPriceError(false);
        }
    }

    const handleMileageChange = (event) => {
        const value = event.target.value;
        setMileage(value);
        if (value !== '' && (isNaN(value) || value < 0)) {
            setMileageError(true);
        } else {
            setMileageError(false);
        }
    }

    const handleImageChange = async (event) => {
        setUploadingImages(true);
        const selectedFiles = Array.from(event.target.files);
        setImages((prevImages) => [...prevImages, ...selectedFiles]);

        const thumbnailPromises = selectedFiles.map(file => createThumbnail(file));
        const newThumbnails = await Promise.all(thumbnailPromises);
        setThumbnails((prevThumbnails) => [...prevThumbnails, ...newThumbnails]);
        setUploadingImages(false);
    };

    const handleImageRemove = (index) => {
        setImages((prevImages) => prevImages.filter((_, i) => i !== index));
        setThumbnails((prevThumbnails) => prevThumbnails.filter((_, i) => i !== index));
    };

    const handleNext = () => {
        if (title === '' || price === '' || priceError || mileage === '' || mileageError || images.length === 0) {
            setPostingError("Please fill in all fields");
            return;
        }
        if (uploadingImages) {
            setPostingError("Please wait for images to finish uploading");
            return;
        }
        setPostingError(null);

        setFinalDetails({
            title: title,
            price: parseInt(price, 10),
            mileage: parseInt(mileage, 10),
            images: images,
            description: description
        });
    }

    return (
        <CommonPaper title={"Final Details"}>
            <div ref={componentRef}>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <InputLabel>Title</InputLabel>
                        <Input
                            value={title}
                            fullWidth
                            onChange={(event) => setTitle(event.target.value)}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputLabel >Price</InputLabel>
                        <Input
                            value={price}
                            fullWidth
                            onChange={handlePriceChange}
                            error={priceError}
                            startAdornment={<InputAdornment position="start">€</InputAdornment>}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <InputLabel>Mileage</InputLabel>
                        <Input
                            value={mileage}
                            fullWidth
                            onChange={handleMileageChange}
                            error={mileageError}
                            endAdornment={<InputAdornment position="end">km</InputAdornment>}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <InputLabel>Description</InputLabel>
                        <TextareaAutosize
                            minRows={3}
                            value={description}
                            style={{
                                width: "100%",
                                backgroundColor: theme.palette.background.paper,
                                color: theme.palette.text.primary,
                                resize: "none"
                            }}
                            onChange={(event) => setDescription(event.target.value)}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Button variant="contained" component="label">
                            Select Images
                            <input
                                type="file"
                                hidden
                                multiple
                                accept="image/*"
                                onChange={handleImageChange}
                            />
                        </Button>
                    </Grid>
                    <Grid item xs={12}>
                        <ImageList cols={9} rowHeight={80}>
                            {thumbnails.map((thumbnail, index) => (
                                <ImageListItem key={index}>
                                    <img
                                        src={thumbnail}
                                        alt={`Selected ${index + 1}`}
                                        loading="lazy"
                                        style={{ objectFit: 'cover', width: '100%', height: '100%' }}
                                    />
                                    <ImageListItemBar
                                        position="top"
                                        actionIcon={
                                            <IconButton
                                                sx={{ color: 'white' }}
                                                onClick={() => handleImageRemove(index)}
                                            >
                                                <DeleteIcon />
                                            </IconButton>
                                        }
                                    />
                                </ImageListItem>
                            ))}
                        </ImageList>
                    </Grid>
                </Grid>
                {postingError !== null ?
                    <Typography sx={{color: "red"}}>{postingError}</Typography>
                    : null
                }
                <Button
                    variant="contained"
                    onClick={handleNext}
                    fullWidth
                    sx={{
                        '&:focus': {
                            outline: 'none',
                        },
                    }}
                >
                    {listing ? "Update Listing" : "Post Listing"}
                </Button>
            </div>
        </CommonPaper>
    )
}

export default ListingSelectFinalDetails;