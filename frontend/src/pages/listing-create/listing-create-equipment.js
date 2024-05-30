import CommonPaper from "../../common/common-paper";
import React, {useEffect, useRef, useState} from "react";
import {getAllColors} from "../../api/color-api";
import {Button, Checkbox, Grid} from "@mui/material";
import CommonSubPaper from "../../common/common-sub-paper";
import {getAllFeaturesExterior} from "../../api/features-exterior-api";
import {getAllFeaturesInterior} from "../../api/features-interior-api";

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
    const [exteriorColor, setExteriorColor] = useState(null);
    const [interiorColor, setInteriorColor] = useState(null);
    const [interiorFeatures, setInteriorFeatures] = useState([]);
    const [selectedInteriorFeatures, setSelectedInteriorFeatures] = useState([]);
    const [exteriorFeatures, setExteriorFeatures] = useState([]);
    const [selectedExteriorFeatures, setSelectedExteriorFeatures] = useState([]);
    const exteriorColorRef = useRef(null);
    const interiorColorRef = useRef(null);
    const featuresRef = useRef(null);

    useEffect(() => {
        getAllColors()
            .then(setColors)
            .catch(console.error);
    }, []);

    useEffect(() => {
        if (colors.length > 0) {
            exteriorColorRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [colors]);

    useEffect(() => {
        if (exteriorColor) {
            interiorColorRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [exteriorColor]);

    useEffect(() => {
        if (interiorColor) {
            getAllFeaturesExterior()
                .then(setExteriorFeatures)
                .catch(console.error);
            getAllFeaturesInterior()
                .then(setInteriorFeatures)
                .catch(console.error);
        }
    }, [interiorColor]);

    useEffect(() => {
        if (exteriorFeatures.length > 0) {
            featuresRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [exteriorFeatures]);

    const handleNext = () => {
        setEquipment({
            exteriorColor: exteriorColor,
            interiorColor: interiorColor,
            featuresExterior: selectedExteriorFeatures,
            featuresInterior: selectedInteriorFeatures
        });
    }

    return (
        <CommonPaper title={"Equipment"}>
            <div ref={exteriorColorRef}>
                <CommonSubPaper title={"Exterior Color"}>
                    <SelectColor colors={colors} setColor={setExteriorColor} color={exteriorColor}/>
                </CommonSubPaper>
            </div>
            {exteriorColor !== null ?
                <div ref={interiorColorRef}>
                    <CommonSubPaper title={"Interior Color"}>
                        <SelectColor colors={colors.filter((color) => color.isInteriorCommon)} setColor={setInteriorColor} color={interiorColor}/>
                    </CommonSubPaper>
                </div>
                : null
            }
            {interiorColor !== null ?
                <div ref={featuresRef}>
                    <CommonSubPaper title={"Exterior Features"}>
                        <SelectFeatures features={exteriorFeatures} setFeatures={setSelectedExteriorFeatures} selectedFeatures={selectedExteriorFeatures}/>
                    </CommonSubPaper>
                    <CommonSubPaper title={"Interior Features"}>
                        <SelectFeatures features={interiorFeatures} setFeatures={setSelectedInteriorFeatures} selectedFeatures={selectedInteriorFeatures}/>
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