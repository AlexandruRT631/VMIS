import React from "react";
import {IconButton} from "@mui/material";
import NavigateBeforeIcon from "@mui/icons-material/NavigateBefore";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";

const NavigationButtons = ({handleNext, handlePrev}) => {
    const handleClick = (event) => {
        event.preventDefault();
    }

    return (
        <React.Fragment>
            <IconButton
                onClick={(event) => {
                    handleClick(event);
                    handlePrev();
                }}
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
                onClick={(event) => {
                    handleClick(event);
                    handleNext();
                }}
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

export default NavigationButtons;