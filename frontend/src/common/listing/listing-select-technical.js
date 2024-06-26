import CommonPaper from "../common-paper";
import {alpha, Button, ButtonGroup, Grid} from "@mui/material";
import React, {useEffect, useRef, useState} from "react";
import CommonSubPaper from "../common-sub-paper";

const SelectTechnical = ({title, listDetails, setDetail, detail}) => {
    const maxButtonGroupSize = 6;

    return (
        <CommonSubPaper title={title}>
            <ButtonGroup sx={{ width: '100%' }}>
                {listDetails.map((newDetail, index) => (
                    <Button
                        key={index}
                        onClick={() => setDetail(newDetail)}
                        variant="contained"
                        sx={{
                            flexBasis: `${100 / maxButtonGroupSize}%`,
                            maxWidth: `${100 / maxButtonGroupSize}%`,
                            backgroundColor: detail !== null && detail.id === newDetail.id
                                ? (theme) => alpha(theme.palette.primary.main, 0.7)
                                : 'primary.main',
                            '&:hover': {
                                backgroundColor: detail !== null && detail.id === newDetail.id
                                    ? (theme) => alpha(theme.palette.primary.main, 0.8)
                                    : 'primary.dark',
                            },
                            '&:focus': {
                                outline: 'none',
                            },
                        }}
                    >
                        {newDetail.name}
                    </Button>
                ))}
            </ButtonGroup>
        </CommonSubPaper>
    );
}

const ListingSelectTechnical = ({listing, technical, setTechnical, car}) => {
    const [category, setCategory] = useState(null);
    const [doorType, setDoorType] = useState(null);
    const [fuel, setFuel] = useState(null);
    const [engine, setEngine] = useState(null);
    const [transmission, setTransmission] = useState(null);
    const [traction, setTraction] = useState(null);
    const categoryRef = useRef(null);
    const doorTypeRef = useRef(null);
    const fuelRef = useRef(null);
    const powerRef = useRef(null);
    const transmissionRef = useRef(null);
    const tractionRef = useRef(null);
    const nextButtonRef = useRef(null);
    const firstLoadRef = useRef(true);

    useEffect(() => {
        if (listing && technical) {
            setTechnical(technical);
            setCategory(technical.category);
            setDoorType(technical.doorType);
            setFuel(technical.engine.fuel);
            setEngine(technical.engine);
            setTransmission(technical.transmission);
            setTraction(technical.traction);
        }
        firstLoadRef.current = false;
    }, []);

    useEffect(() => {
        if (car) {
            categoryRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, [car]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (category) {
                doorTypeRef.current.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            else if (category === null) {
                setDoorType(null);
            }
        }
    }, [category]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (doorType) {
                fuelRef.current.scrollIntoView({behavior: 'smooth', block: 'center'});
            } else if (doorType === null) {
                setFuel(null);
            }
        }
    }, [doorType]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (fuel) {
                powerRef.current.scrollIntoView({behavior: 'smooth', block: 'center'});
            } else if (fuel === null) {
                setEngine(null);
            }
        }
    }, [fuel]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (engine) {
                transmissionRef.current.scrollIntoView({behavior: 'smooth', block: 'center'});
            } else if (engine === null) {
                setTransmission(null);
            }
        }
    }, [engine]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (transmission) {
                tractionRef.current.scrollIntoView({behavior: 'smooth', block: 'center'});
            } else if (transmission === null) {
                setTraction(null);
            }
        }
    }, [transmission]);

    useEffect(() => {
        if (!firstLoadRef.current) {
            if (traction) {
                if (listing) {
                    handleNext();
                }
                else {
                    nextButtonRef.current.scrollIntoView({behavior: 'smooth', block: 'center'});
                }
            }
        }
    }, [traction, listing]);

    const handleChange = (setCurrent, newCurrent, setNext) => {
        setTechnical(null);
        setNext(null);
        setCurrent(newCurrent);
    }

    const handleNext = () => {
        setTechnical({
            category: category,
            doorType: doorType,
            engine: engine,
            transmission: transmission,
            traction: traction
        });
    }

    return (
        <CommonPaper title={"Technical Details"}>
            <div ref={categoryRef}>
                <SelectTechnical
                    title={"Category"}
                    listDetails={car.possibleCategories}
                    setDetail={(newCategory) => handleChange(setCategory, newCategory, setDoorType)}
                    detail={category}
                />
            </div>
            {category !== null ?
                <div ref={doorTypeRef}>
                    <SelectTechnical
                        title={"Door Type"}
                        listDetails={car.possibleDoorTypes}
                        setDetail={(newDoorType) => handleChange(setDoorType, newDoorType, setFuel)}
                        detail={doorType}
                    />
                </div>
                : null
            }
            {doorType !== null ?
                <div ref={fuelRef}>
                    <SelectTechnical
                        title={"Fuel"}
                        listDetails={Array.from(new Map(car.possibleEngines.map(engine => [engine.fuel.id, engine.fuel])).values())}
                        setDetail={(newFuel) => handleChange(setFuel, newFuel, setEngine)}
                        detail={fuel}
                    />
                </div>
                : null
            }
            {fuel !== null ?
                <div ref={powerRef}>
                    <SelectTechnical
                        title={"Power"}
                        listDetails={
                            car.possibleEngines
                                .filter(engine => engine.fuel.id === fuel.id)
                                .map(engine => ({
                                    id: engine.id,
                                    name: String(engine.power) + " hp"
                                }))
                        }
                        setDetail={(newEngine) => handleChange(setEngine, {id: newEngine.id}, setTransmission)}
                        detail={engine}
                    />
                </div>
                : null
            }
            {engine !== null ?
                <div ref={transmissionRef}>
                    <SelectTechnical
                        title={"Transmission"}
                        listDetails={car.possibleTransmissions}
                        setDetail={(newTransmission) => handleChange(setTransmission, newTransmission, setTraction)}
                        detail={transmission}
                    />
                </div>
                : null
            }
            {transmission !== null ?
                <div ref={tractionRef}>
                    <SelectTechnical
                        title={"Traction"}
                        listDetails={car.possibleTractions}
                        setDetail={setTraction}
                        detail={traction}
                    />
                </div>
                : null
            }
            {traction !== null && !listing ?
                <Grid container style={{ flexGrow: 1 }}>
                    <Grid mt={2} item xs={12}>
                        <Button
                            ref={nextButtonRef}
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
                    </Grid>
                </Grid>
                : null
            }
        </CommonPaper>
    );
}

export default ListingSelectTechnical;