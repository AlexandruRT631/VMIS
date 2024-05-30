import CommonPaper from "../../common/common-paper";
import {alpha, Button, ButtonGroup, Grid} from "@mui/material";
import React, {useState} from "react";

const ListingCreateGeneration = ({setCar, possibleCars, car}) => {
    const [selectedCar, setSelectedCar] = useState(null);
    const maxButtonGroupSize = 6;

    const handleSelect = (selectCar) => {
        setSelectedCar(selectCar);
        setCar(null);
    }

    const handleNext = () => {
        setCar(selectedCar);
    }

    return (
        <CommonPaper title={"Generation"}>
            <ButtonGroup sx={{ width: '100%' }}>
                {possibleCars.sort((a, b) => a.startYear - b.startYear).map((selectCar, index) => (
                    <Button
                        key={index}
                        onClick={() => handleSelect(selectCar)}
                        variant="contained"
                        sx={{
                            flexBasis: `${100 / maxButtonGroupSize}%`,
                            maxWidth: `${100 / maxButtonGroupSize}%`,
                            backgroundColor: selectedCar !== null && selectedCar.id === selectCar.id
                                ? (theme) => alpha(theme.palette.primary.main, 0.7)
                                : 'primary.main',
                            '&:hover': {
                                backgroundColor: selectedCar !== null && selectedCar.id === selectCar.id
                                    ? (theme) => alpha(theme.palette.primary.main, 0.8)
                                    : 'primary.dark',
                            },
                            '&:focus': {
                                outline: 'none',
                            },
                        }}
                    >
                        {selectCar.startYear + ' - ' + selectCar.endYear}
                    </Button>
                ))}
            </ButtonGroup>
            {selectedCar !== null ?

                <Grid container style={{ flexGrow: 1 }}>
                    <Grid mt={2} item xs={12}>
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
                    </Grid>
                </Grid>
                : null
            }
        </CommonPaper>
    )
}

export default ListingCreateGeneration;