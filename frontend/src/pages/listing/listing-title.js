import {Avatar, Button, Grid, Link, Paper, Stack, Typography} from "@mui/material";
import React from "react";

const ListingTitle = ({ listing }) => {
    const numberFormat = new Intl.NumberFormat('en-US');

    return (
        <Paper variant="outlined" sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
            <Typography variant="h4" color="primary" gutterBottom sx={{ fontWeight: 'bold' }}>
                {listing.title}
            </Typography>
            <Grid container>
                <Grid item xs={6}>
                    <Typography variant="h6" >Price</Typography>
                    <Typography variant="h4" >{numberFormat.format(listing.price)} €</Typography>
                </Grid>
                <Grid item xs={6}>
                    <Typography variant="h6" >Seller</Typography>
                    <Link href={'/'} sx={{ textDecoration: 'none', color: 'inherit' }}>
                        <Stack direction={"row"} alignItems="center" spacing={2}>
                            <Avatar src={"https://i0.wp.com/sbcf.fr/wp-content/uploads/2018/03/sbcf-default-avatar.png?ssl=1"} />
                            <Typography variant="h6" >Alexandru Radu-Todor</Typography>
                        </Stack>
                    </Link>
                </Grid>
            </Grid>
            <Grid container sx={{mt: 2}}>
                <Grid item xs={6}>
                    <Button
                        variant="contained"
                        sx={{
                            '&:focus': {
                                outline: 'none',
                            },
                        }}
                    >
                        Save listing
                    </Button>
                </Grid>
                <Grid item xs={6}>
                    <Button
                        variant="contained"
                        sx={{
                            '&:focus': {
                                outline: 'none',
                            },
                        }}
                    >
                        Contact seller
                    </Button>
                </Grid>
            </Grid>
        </Paper>
    );

    //TODO: fetch seller information from the user database
}

export default ListingTitle;