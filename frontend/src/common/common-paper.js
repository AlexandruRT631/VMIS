import React from 'react';
import { Paper, Typography } from '@mui/material';

const ReusablePaper = ({ title, children }) => {
    return (
        <Paper variant="outlined" sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
            <Typography mb={2} variant="h6" color="primary" gutterBottom>
                {title}
            </Typography>
            {children}
        </Paper>
    );
};

export default ReusablePaper;
