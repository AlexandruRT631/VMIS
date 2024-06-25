import {Link} from "react-router-dom";
import {Box, Grid, Paper, Typography} from "@mui/material";
import React from "react";

const BASE_URL = process.env.REACT_APP_USER_API_URL;

const ProfileThumbnail = ({ profile }) => {
    console.log(profile)

    return (
        <Link to={`/profile/${profile.id}`} style={{textDecoration: 'none'}}>
            <Box
                sx={{
                    cursor: 'pointer',
                    overflow: 'hidden',
                    position: 'relative',
                    width: '100%',
                    paddingTop: '150px',
                }}
            >
                <Paper sx={{position: 'absolute', top: 0, left: 0, bottom: 0, width: '100%', height: '100%'}}>
                    <Grid
                        container
                        direction={"row"}
                        justifyContent={"center"}
                        alignItems={"stretch"}
                        sx={{height: '100%', margin: 0}}
                    >
                        <Grid item xs={2} sx={{height: '100%'}}>
                            <Box
                                sx={{
                                    height: '100%',
                                    width: '100%',
                                    position: 'relative'
                                }}
                            >
                                <Box
                                    component="img"
                                    src={BASE_URL + profile.profilePictureUrl}
                                    alt={profile.profilePictureUrl}
                                    sx={{
                                        objectFit: 'cover',
                                        height: '100%',
                                        width: '100%'
                                    }}
                                />
                            </Box>
                        </Grid>
                        <Grid item xs={10} sx={{display: 'flex', flexDirection: 'column', padding: 2}}>
                            <Typography
                                sx={{
                                    fontWeight: 'bold',
                                    fontSize: 'calc(2em + 0.1vw)'
                                }}
                            >
                                {profile.name}
                            </Typography>
                        </Grid>
                    </Grid>
                </Paper>
            </Box>
        </Link>
    );
}

export default ProfileThumbnail;