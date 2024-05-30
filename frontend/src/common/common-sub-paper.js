import {Box, Typography} from "@mui/material";
import React from "react";

const CommonSubPaper = ({ title, children }) => {
    return (
        <Box sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
            <Typography mb={2} variant="h5" gutterBottom>
                {title}
            </Typography>
            {children}
        </Box>
    )
}

export default CommonSubPaper;