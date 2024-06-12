import CommonPaper from "../../common/common-paper";
import React, {useEffect, useRef, useState} from "react";
import {getAllColors} from "../../api/color-api";
import {Button, Checkbox, Grid} from "@mui/material";
import CommonSubPaper from "../../common/common-sub-paper";
import {getAllFeatures} from "../../api/feature-api";

const SelectColor = ({colors, setColor, color}) => {
    return (
        <Grid container spacing={2}>
            {colors.map((selectedColor, index) => (
                <Grid item key={index} xs={2}>
                    <Checkbox
                        sx={{
                            color: selectedColor.hexCode,
                            '&.Mui-checked': {
                                color: selectedColor.hexCode,
                            },
                        }}
                        onChange={() => setColor(selectedColor)}
                        checked={selectedColor === color}
                    />
                    {selectedColor.name.charAt(0).toUpperCase() + selectedColor.name.slice(1)}
                </Grid>
            ))}
        </Grid>
    );
}

const SelectFeatures = ({features, setFeatures, selectedFeatures}) => {
    return (
        <Grid container spacing={2}>
            {features.map((feature, index) => (
                <Grid item key={index} xs={2}>
                    <Checkbox
                        onChange={() => {
                            if (selectedFeatures.includes(feature)) {
                                setFeatures(selectedFeatures.filter((selectedFeature) => selectedFeature !== feature));
                            } else {
                                setFeatures([...selectedFeatures, feature]);
                            }
                        }}
                        checked={selectedFeatures.includes(feature)}
                    />
                    {feature.name.charAt(0).toUpperCase() + feature.name.slice(1)}
                </Grid>
            ))}
        </Grid>
    )
}

const ListingCreateEquipment = ({setEquipment}) => {
    const [colors, setColors] = useState([]);
    const [color, setColor] = useState(null);
    const [features, setFeatures] = useState([]);
    const [selectedFeatures, setSelectedFeatures] = useState([]);
    const colorRef = useRef(null);
    const featuresRef = useRef(null);

    useEffect(() => {
        getAllColors()
            .then(setColors)
            .catch(console.error);
    }, []);

    useEffect(() => {
        if (colors.length > 0) {
            colorRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [colors]);

    useEffect(() => {
        if (color) {
            getAllFeatures()
                .then(setFeatures)
                .catch(console.error);
        }
    }, [color]);

    useEffect(() => {
        if (features.length > 0) {
            featuresRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [features]);

    const handleNext = () => {
        setEquipment({
            color: color,
            features: selectedFeatures
        });
    }

    return (
        <CommonPaper title={"Equipment"}>
            <div ref={colorRef}>
                <CommonSubPaper title={"Exterior Color"}>
                    <SelectColor colors={colors} setColor={setColor} color={color}/>
                </CommonSubPaper>
            </div>
            {color !== null ?
                <div ref={featuresRef}>
                    <CommonSubPaper title={"Exterior Features"}>
                        <SelectFeatures features={features} setFeatures={setSelectedFeatures} selectedFeatures={selectedFeatures}/>
                    </CommonSubPaper>
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
                </div>
                : null
            }
        </CommonPaper>
    );
}

export default ListingCreateEquipment;