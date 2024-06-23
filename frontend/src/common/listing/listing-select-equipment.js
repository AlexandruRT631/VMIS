import CommonPaper from "../common-paper";
import React, {useEffect, useRef, useState} from "react";
import {getAllColors} from "../../api/color-api";
import {Button, Checkbox, Grid, Typography, Box} from "@mui/material";
import CommonSubPaper from "../common-sub-paper";
import {getAllFeatures} from "../../api/feature-api";

const SelectColor = ({colors, setColor, color}) => {
    return (
        <Grid container spacing={2}>
            {colors.map((selectedColor, index) => (
                <Grid item key={index} xs={2}>
                    <Grid container>
                        <Grid item xs={4}>
                            <Checkbox
                                sx={{
                                    color: selectedColor.hexCode,
                                    '&.Mui-checked': {
                                        color: selectedColor.hexCode,
                                    },
                                }}
                                onChange={() => setColor(selectedColor)}
                                checked={color ? selectedColor.id === color.id : false}
                            />
                        </Grid>
                        <Grid item xs={8}>
                            <Box
                                sx={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    height: '100%'
                                }}
                            >
                                <Typography sx={{ lineHeight: 'normal' }}>
                                    {selectedColor.name.charAt(0).toUpperCase() + selectedColor.name.slice(1)}
                                </Typography>
                            </Box>
                        </Grid>
                    </Grid>
                </Grid>
            ))}
        </Grid>
    );
}

const SelectFeatures = ({features, setFeatures, selectedFeatures}) => {
    return (
        <Grid container spacing={2}>
            {features.sort((a, b) => a.name.localeCompare(b.name)).map((feature, index) => (
                <Grid item key={index} xs={4}>
                    <Grid container>
                        <Grid item xs={2}>
                            <Checkbox
                                onChange={() => {
                                    if (selectedFeatures.some(selectedFeature => selectedFeature.id === feature.id)) {
                                        setFeatures(selectedFeatures.filter(selectedFeature => selectedFeature.id !== feature.id));
                                    } else {
                                        setFeatures([...selectedFeatures, feature]);
                                    }
                                }}
                                checked={selectedFeatures.some(selectedFeature => selectedFeature.id === feature.id)}
                            />
                        </Grid>
                        <Grid item xs={10}>
                            <Box
                                sx={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    height: '100%'
                                }}
                            >
                                <Typography sx={{ lineHeight: 'normal' }}>
                                    {feature.name.charAt(0).toUpperCase() + feature.name.slice(1)}
                                </Typography>
                            </Box>
                        </Grid>
                    </Grid>
                </Grid>
            ))}
        </Grid>
    )
}

const ListingSelectEquipment = ({listing, setEquipment}) => {
    const [colors, setColors] = useState([]);
    const [color, setColor] = useState(null);
    const [features, setFeatures] = useState([]);
    const [selectedFeatures, setSelectedFeatures] = useState([]);
    const colorRef = useRef(null);
    const featuresRef = useRef(null);
    const [error, setError] = useState(false);
    const errorRef = useRef(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const fetchedColors = await getAllColors();
                setColors(fetchedColors);
                const fetchedFeatures = await getAllFeatures();
                setFeatures(fetchedFeatures);
                if (listing) {
                    setColor(listing.color);
                    setSelectedFeatures(listing.features);
                }
            } catch (error) {
                console.error(error);
            }
        }
        fetchData();
    }, []);

    useEffect(() => {
        if (colors.length > 0) {
            colorRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [colors]);

    useEffect(() => {
        if (color) {
            featuresRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [color]);

    useEffect(() => {
        if (listing) {
            handleNext();
        }
    }, [selectedFeatures])

    useEffect(() => {
        if (error && errorRef.current) {
            errorRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [error]);

    const handleNext = () => {
        if (selectedFeatures.length === 0) {
            setError(true);
            setEquipment(null);
        }
        else {
            setError(false);
            setEquipment({
                color: color,
                features: selectedFeatures
            });
        }
    }

    return (
        <CommonPaper title={"Equipment"}>
            <div ref={colorRef}>
                <CommonSubPaper title={"Color"}>
                    <SelectColor colors={colors} setColor={setColor} color={color}/>
                </CommonSubPaper>
            </div>
            {color !== null ?
                <div ref={featuresRef}>
                    <CommonSubPaper title={"Exterior Features"}>
                        <SelectFeatures features={features} setFeatures={setSelectedFeatures} selectedFeatures={selectedFeatures}/>
                    </CommonSubPaper>
                    {error &&
                        <div ref={errorRef}>
                            <Typography variant="body2" color="error">
                                Please select at least one feature.
                            </Typography>
                        </div>
                    }
                    {!listing &&
                        <Button
                            variant="contained"
                            onClick={handleNext}
                            sx={{
                                '&:focus': {
                                    outline: 'none',
                                },
                            }}
                        >
                            Next
                        </Button>
                    }
                </div>
                : null
            }
        </CommonPaper>
    );
}

export default ListingSelectEquipment;