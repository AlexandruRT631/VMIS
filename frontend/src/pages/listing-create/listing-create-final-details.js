import CommonPaper from "../../common/common-paper";
import React, {useEffect, useRef, useState} from "react";
import {
    Button,
    Grid, IconButton,
    ImageList,
    ImageListItem,
    ImageListItemBar,
    Input,
    InputAdornment,
    InputLabel,
    Typography
} from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';

const ListingCreateFinalDetails = ({setFinalDetails}) => {
    const [title, setTitle] = useState("");
    const [price, setPrice] = useState("");
    const [mileage, setMileage] = useState("");
    const [priceError, setPriceError] = useState(false);
    const [mileageError, setMileageError] = useState(false);
    const [images, setImages] = useState([]);
    const [thumbnails, setThumbnails] = useState([]);
    const [uploadingImages, setUploadingImages] = useState(false);
    const [postingError, setPostingError] = useState(null);
    const sellerId = 1;
    const componentRef = useRef(null);

    useEffect(() => {
        if (componentRef.current) {
            componentRef.current.scrollIntoView({behavior: "smooth"});
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

    const createThumbnail = (file) => {
        return new Promise((resolve) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = (event) => {
                const img = new Image();
                img.src = event.target.result;
                img.onload = () => {
                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    const maxSize = 100; // set the size of the thumbnail
                    let width = img.width;
                    let height = img.height;

                    if (width > height) {
                        if (width > maxSize) {
                            height *= maxSize / width;
                            width = maxSize;
                        }
                    } else {
                        if (height > maxSize) {
                            width *= maxSize / height;
                            height = maxSize;
                        }
                    }

                    canvas.width = width;
                    canvas.height = height;
                    ctx.drawImage(img, 0, 0, width, height);
                    resolve(canvas.toDataURL());
                };
            };
        });
    };

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
            sellerId: sellerId,
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
                    Post Listing
                </Button>
            </div>
        </CommonPaper>
    )
}

export default ListingCreateFinalDetails;