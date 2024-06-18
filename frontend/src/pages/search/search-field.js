import {Autocomplete, Button, Checkbox, Grid, Paper, TextField, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {getAllMakes} from "../../api/make-api";
import {getAllModelsByMakeId} from "../../api/model-api";
import {getCarsByModel} from "../../api/car-api";
import {getAllCategories} from "../../api/category-api";
import {getAllFuels} from "../../api/fuel-api";
import {getAllTransmissions} from "../../api/transmission-api";
import {getAllTractions} from "../../api/traction-api";
import {getAllDoorTypes} from "../../api/door-type-api";
import {getAllColors} from "../../api/color-api";
import {getAllFeatures} from "../../api/feature-api";

const GridAutocompleteElement = ({list, setSelect, select, name}) => {
    return (
        <Grid item xs={3}>
            <Autocomplete
                options={list}
                getOptionLabel={(option) => option.name}
                onChange={(event, newValue) => setSelect(newValue)}
                value={select}
                renderInput={(params) => <TextField {...params} label={name} />}
                disabled={list.length === 0}
            />
        </Grid>
    );
}

const GridAutocompleteList = ({ list, setSelect, name }) => {
    const [inputValue, setInputValue] = useState('');
    const [localSelect, setLocalSelect] = useState([]);
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);

    const handleDropdownClose = () => {
        setIsDropdownOpen(false);
        setSelect(localSelect);
    };

    const handleDropdownOpen = () => {
        setIsDropdownOpen(true);
    };

    const handleInputChange = (event, newValue) => {
        setInputValue(newValue);
        if (newValue === '' && !isDropdownOpen) {
            setLocalSelect([]);
            setSelect([]);
        }
    };

    const handleChange = (event, newValue) => {
        setLocalSelect(newValue);
        if (newValue.length === 0 && !isDropdownOpen) {
            setSelect([]);
        }
    };

    return (
        <Grid item xs={3}>
            <Autocomplete
                multiple
                options={list}
                disableCloseOnSelect
                getOptionLabel={(option) => option.name}
                inputValue={inputValue}
                onInputChange={handleInputChange}
                onChange={handleChange}
                value={localSelect}
                onClose={handleDropdownClose}
                onOpen={handleDropdownOpen}
                renderOption={(props, option, { selected }) => {
                    const { key, ...rest } = props;
                    return (
                        <li key={key} {...rest}>
                            <Checkbox
                                sx={{
                                    color: option.hexCode,
                                    '&.Mui-checked': {
                                        color: option.hexCode,
                                    },
                                }}
                                checked={selected}
                                style={{ marginRight: 8 }}
                            />
                            {option.name}
                        </li>
                    );
                }}
                renderTags={(value, getTagProps) => {
                    if (inputValue || value.length === 0) return null;
                    const firstOption = value[0];
                    const additionalCount = value.length - 1;

                    return (
                        <div style={{ display: 'flex', alignItems: 'center' }}>
                            <span>{firstOption.name}</span>
                            {additionalCount > 0 && (
                                <span style={{ marginLeft: 4 }}>
                                    {`+ ${additionalCount}`}
                                </span>
                            )}
                        </div>
                    );
                }}
                renderInput={(params) => <TextField {...params} label={name} />}
                disabled={list.length === 0}
            />
        </Grid>
    );
}

const GridRange = ({ numberMin, numberMax, setNumberMin, setNumberMax, nameMin, nameMax }) => {
    const [localNumberMin, setLocalNumberMin] = useState("");
    const [localNumberMax, setLocalNumberMax] = useState("");
    const [errorMin, setErrorMin] = useState("");
    const [errorMax, setErrorMax] = useState("");

    useEffect(() => {
        const numericValueMin = Number(localNumberMin);
        const numericValueMax = Number(localNumberMax);

        if (localNumberMin === "") {
            setErrorMin("");
            setNumberMin(null);
        } else if (isNaN(numericValueMin) || numericValueMin <= 0) {
            setErrorMin("Value must be greater than 0");
            setNumberMin(null);
        } else if (!isNaN(numericValueMax) && numericValueMax > 0 && numericValueMin > numericValueMax) {
            setErrorMin("Value must be less or equal to " + numericValueMax);
            setNumberMin(null);
        } else {
            setErrorMin("");
            setNumberMin(numericValueMin);
        }

        if (localNumberMax === "") {
            setErrorMax("");
            setNumberMax(null);
        } else if (isNaN(numericValueMax) || numericValueMax <= 0) {
            setErrorMax("Value must be greater than 0");
            setNumberMax(null);
        } else if (!isNaN(numericValueMin) && numericValueMin > 0 && numericValueMax < numericValueMin) {
            setErrorMax("Value must be greater or equal to " + numericValueMin);
            setNumberMax(null);
        } else {
            setErrorMax("");
            setNumberMax(numericValueMax);
        }
    }, [localNumberMin, localNumberMax, setNumberMin, setNumberMax]);

    return (
        <React.Fragment>
            <Grid item xs={3}>
                <TextField
                    label={nameMin}
                    type="number"
                    value={localNumberMin}
                    onChange={(event) => { setLocalNumberMin(event.target.value) }}
                    error={errorMin !== ""}
                    helperText={errorMin}
                    fullWidth
                    sx={{
                        '& input[type=number]::-webkit-outer-spin-button, & input[type=number]::-webkit-inner-spin-button': {
                            WebkitAppearance: 'none',
                            margin: 0,
                        },
                        '& input[type=number]': {
                            MozAppearance: 'textfield',
                        },
                    }}
                />
            </Grid>
            <Grid item xs={3}>
                <TextField
                    label={nameMax}
                    type="number"
                    value={localNumberMax}
                    onChange={(event) => { setLocalNumberMax(event.target.value) }}
                    error={errorMax !== ""}
                    helperText={errorMax}
                    fullWidth
                    sx={{
                        '& input[type=number]::-webkit-outer-spin-button, & input[type=number]::-webkit-inner-spin-button': {
                            WebkitAppearance: 'none',
                            margin: 0,
                        },
                        '& input[type=number]': {
                            MozAppearance: 'textfield',
                        },
                    }}
                />
            </Grid>
        </React.Fragment>
    );
};

const GridText = ({text, setText, name, size}) => {
    return (
        <Grid item xs={size}>
            <TextField
                label={name}
                value={text}
                onChange={(event) => { setText(event.target.value) }}
                fullWidth
                sx={{
                    '& input[type=number]::-webkit-outer-spin-button, & input[type=number]::-webkit-inner-spin-button': {
                        WebkitAppearance: 'none',
                        margin: 0,
                    },
                    '& input[type=number]': {
                        MozAppearance: 'textfield',
                    },
                }}
            />
        </Grid>
    )
}

const GridImageUpload = () => {
    return (
        <Grid item xs={3}>
            <Typography>IMAGE UPLOAD TO DO</Typography>
        </Grid>
    )
}

const SearchField = ({ setListingSearchDto }) => {
    const [makes, setMakes] = useState([]);
    const [selectedMake, setSelectedMake] = useState(null);
    const [models, setModels] = useState([]);
    const [selectedModel, setSelectedModel] = useState(null);
    const [generations, setGenerations] = useState([]);
    const [selectedGeneration, setSelectedGeneration] = useState(null);
    const [categories, setCategories] = useState([]);
    const [selectedCategories, setSelectedCategories] = useState([]);
    const [minPrice, setMinPrice] = useState(null);
    const [maxPrice, setMaxPrice] = useState(null);
    const [minYear, setMinYear] = useState(null);
    const [maxYear, setMaxYear] = useState(null);
    const [minMileage, setMinMileage] = useState(null);
    const [maxMileage, setMaxMileage] = useState(null);
    const [fuels, setFuels] = useState([]);
    const [selectedFuels, setSelectedFuels] = useState([]);
    const [keywords, setKeywords] = useState("");
    const [more, setMode] = useState(false);
    const [transmissions, setTransmissions] = useState([]);
    const [selectedTransmissions, setSelectedTransmissions] = useState([]);
    const [tractions, setTractions] = useState([]);
    const [selectedTractions, setSelectedTractions] = useState([]);
    const [minPower, setMinPower] = useState(null);
    const [maxPower, setMaxPower] = useState(null);
    const [minDisplacement, setMinDisplacement] = useState(null);
    const [maxDisplacement, setMaxDisplacement] = useState(null);
    const [minTorque, setMinTorque] = useState(null);
    const [maxTorque, setMaxTorque] = useState(null);
    const [engineCode, setEngineCode] = useState("");
    const [doorTypes, setDoorTypes] = useState([]);
    const [selectedDoorTypes, setSelectedDoorTypes] = useState([]);
    const [colors, setColors] = useState([]);
    const [selectedColors, setSelectedColors] = useState([]);
    const [features, setFeatures] = useState([]);
    const [selectedFeatures, setSelectedFeatures] = useState([]);

    useEffect(() => {
        getAllMakes()
            .then(setMakes)
            .catch(console.error);
        getAllCategories()
            .then(setCategories)
            .catch(console.error);
        getAllFuels()
            .then(setFuels)
            .catch(console.error);
        getAllTransmissions()
            .then(setTransmissions)
            .catch(console.error);
        getAllTractions()
            .then(setTractions)
            .catch(console.error);
        getAllDoorTypes()
            .then(setDoorTypes)
            .catch(console.error);
        getAllColors()
            .then(setColors)
            .catch(console.error);
        getAllFeatures()
            .then(setFeatures)
            .catch(console.error);
    }, []);

    useEffect(() => {
        if (selectedMake !== null) {
            getAllModelsByMakeId(selectedMake.id)
                .then(setModels)
                .catch(console.error);
            setListingSearchDto(prevDto => ({ ...prevDto, MakeId: selectedMake.id }));
        } else {
            setModels([]);
            setGenerations([]);
            setListingSearchDto(prevDto => ({ ...prevDto, MakeId: null }));
        }
        setSelectedModel(null);
        setSelectedGeneration(null);
    }, [selectedMake, setListingSearchDto]);

    useEffect(() => {
        if (selectedModel !== null) {
            setListingSearchDto(prevDto => ({ ...prevDto, ModelId: selectedModel.id }));
            getCarsByModel(selectedModel.id)
                .then((result) => {
                    setGenerations(result.map(car => ({
                        startYear: car.startYear,
                        name: car.startYear + ' - ' + car.endYear,
                    })).sort((a, b) => a.startYear - b.startYear));
                })
                .catch(console.error);
        } else {
            setGenerations([]);
            setListingSearchDto(prevDto => ({ ...prevDto, ModelId: null }));
        }
        setSelectedGeneration(null);
    }, [selectedModel, setListingSearchDto]);

    useEffect(() => {
        if (selectedGeneration !== null) {
            setListingSearchDto(prevDto => ({ ...prevDto, StartYear: selectedGeneration.startYear }));
        } else {
            setListingSearchDto(prevDto => ({ ...prevDto, StartYear: null }));
        }
    }, [selectedGeneration, setListingSearchDto]);

    useEffect(() => {
        setListingSearchDto(prevDto => ({
            ...prevDto,
            Categories: selectedCategories.map(category => category.id),
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            MinYear: minYear,
            MaxYear: maxYear,
            MinMileage: minMileage,
            MaxMileage: maxMileage,
            Fuels: selectedFuels.map(fuel => fuel.id),
            Keywords: keywords,
            Transmissions: selectedTransmissions.map(transmission => transmission.id),
            Tractions: selectedTractions.map(tractions => tractions.id),
            MinPower: minPower,
            MaxPower: maxPower,
            MinDisplacement: minDisplacement,
            MaxDisplacement: maxDisplacement,
            MinTorque: minTorque,
            MaxTorque: maxTorque,
            EngineCode: engineCode,
            DoorTypes: selectedDoorTypes.map(doorType => doorType.id),
            Colors: selectedColors.map(color => color.id),
            Features: selectedFeatures.map(feature => feature.id)
        }));
    }, [
        selectedCategories,
        minPrice,
        maxPrice,
        minYear,
        maxYear,
        minMileage,
        maxMileage,
        selectedFuels,
        keywords,
        selectedTransmissions,
        selectedTractions,
        minPower,
        maxPower,
        minDisplacement,
        maxDisplacement,
        minTorque,
        maxTorque,
        engineCode,
        selectedDoorTypes,
        selectedColors,
        selectedFeatures,
        setListingSearchDto
    ]);

    return (
        <Paper
            variant="outlined"
            sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexDirection: 'column',
                padding: 2,
            }}
        >
            <Grid container spacing={3}>
                <GridAutocompleteElement list={makes} select={selectedMake} setSelect={setSelectedMake} name={"Make"} />
                <GridAutocompleteElement list={models} select={selectedModel} setSelect={setSelectedModel} name={"Model"} />
                <GridAutocompleteElement list={generations} select={selectedGeneration} setSelect={setSelectedGeneration} name={"Generation"} />
                <GridAutocompleteList list={categories} setSelect={setSelectedCategories} name={"Categories"} />
                <GridRange numberMin={minPrice} numberMax={maxPrice} setNumberMin={setMinPrice} setNumberMax={setMaxPrice} nameMin={"Min Price"} nameMax={"Max Price"} />
                <GridRange numberMin={minYear} numberMax={maxYear} setNumberMin={setMinYear} setNumberMax={setMaxYear} nameMin={"Min Year"} nameMax={"Max Year"} />
                <GridRange numberMin={minMileage} numberMax={maxMileage} setNumberMin={setMinMileage} setNumberMax={setMaxMileage} nameMin={"Min Mileage"} nameMax={"Max Mileage"} />
                <GridAutocompleteList list={fuels} setSelect={setSelectedFuels} name={"Fuels"} />
                <GridText text={keywords} setText={setKeywords} name={"Search by words"} size={9} />
                <GridImageUpload />
                {more?
                    <React.Fragment>
                        <GridAutocompleteList list={transmissions} setSelect={setSelectedTransmissions} name={"Transmissions"} />
                        <GridAutocompleteList list={tractions} setSelect={setSelectedTractions} name={"Tractions"} />
                        <GridRange numberMin={minPower} numberMax={maxPower} setNumberMin={setMinPower} setNumberMax={setMaxPower} nameMin={"Min Power"} nameMax={"Max Power"} />
                        <GridRange numberMin={minDisplacement} numberMax={maxDisplacement} setNumberMin={setMinDisplacement} setNumberMax={setMaxDisplacement} nameMin={"Min Displacement"} nameMax={"Max Displacement"} />
                        <GridRange numberMin={minTorque} numberMax={maxTorque} setNumberMin={setMinTorque} setNumberMax={setMaxTorque} nameMin={"Min Torque"} nameMax={"Max Torque"} />
                        <GridText text={engineCode} setText={setEngineCode} name={"Engine Code"} size={3} />
                        <GridAutocompleteList list={doorTypes} setSelect={setSelectedDoorTypes} name={"Door Types"} />
                        <GridAutocompleteList list={colors} setSelect={setSelectedColors} name={"Colors"} />
                        <GridAutocompleteList list={features} setSelect={setSelectedFeatures} name={"Features"} />
                    </React.Fragment>
                    : null
                }
            </Grid>
            <Grid container mt={2}>
            <Button
                variant="contained"
                onClick={() => setMode(!more)}
                sx={{
                    '&:focus': {
                        outline: 'none',
                    },
                }}
            >
                {more? "Show Less" : "Show More"}
            </Button>
            </Grid>
        </Paper>
    );
};

export default SearchField;