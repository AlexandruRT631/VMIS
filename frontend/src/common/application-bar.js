import React, {useEffect, useState} from "react";
import {AppBar, Avatar, Box, Button, IconButton, Menu, MenuItem, Toolbar, Tooltip} from "@mui/material";
import {getUserId, removeToken} from "./token";
import LoginIcon from '@mui/icons-material/Login';
import {Link} from "react-router-dom";
import {getUserDetails} from "../api/user-api";

const BASE_URL = process.env.REACT_APP_USER_API_URL;

const ApplicationBar = () => {
    const [loading, setLoading] = useState(true);
    const [userId, setUserId] = useState(null);
    const [userProfilePictureUrl, setUserProfilePictureUrl] = useState(null);
    const [pages, setPages] = useState([]);
    const [userMenu, setUserMenu] = useState([]);
    const [anchorEl, setAnchorEl] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            setUserId(getUserId());
            setPages([{
                title: 'Search',
                url: '/search'
            }]);
            if (userId) {
                const user = await getUserDetails(userId);
                setUserProfilePictureUrl(user.profilePictureUrl);
                if (user.role === 'Seller' || user.role === 'Admin') {
                    setPages((prevPages) => {
                        return [...prevPages, {
                            title: 'Create Listing',
                            url: '/createListing'
                        }];
                    });
                }
                if (user.role === 'Admin') {
                    setPages((prevPages) => {
                        return [...prevPages, {
                            title: 'Admin',
                            url: '/admin'
                        }];
                    });
                }
                setUserMenu([
                    {
                        title: 'Profile',
                        action: () => {
                            window.location.href = '/profile/' + userId;
                        }
                    },
                    {
                        title: 'Account',
                        action: () => {
                            window.location.href = '/account';
                        }
                    },
                    {
                        title: 'Favourites',
                        action: () => {
                            window.location.href = '/favourites';
                        }
                    },
                    {
                        title: 'Log out',
                        action: () => {
                            removeToken();
                            window.location.href = '/';
                        }
                    }
                ])
            } else {
                setUserMenu([
                    {
                        title: 'Log in',
                        action: () => {
                            window.location.href = '/login';
                        }
                    },
                    {
                        title: 'Register',
                        action: () => {
                            window.location.href = '/register';
                        }
                    }
                ])
            }
        }

        fetchData()
            .then(() => setLoading(false))
            .catch(console.error);
    }, [userId]);

    if (loading) {
        return;
    }

    return (
        <AppBar position="fixed" sx={{ background: '#1E1E1E' }}>
            <Toolbar sx={{ flexWrap: 'wrap' }}>
                <Button
                    component={Link}
                    to={"/"}
                    disableRipple
                    sx={{
                        "&:hover": {
                            backgroundColor: "transparent"
                        },
                        "&:focus": {
                            outline: "none"
                        }
                    }}
                >
                    <Box
                        component="img"
                        src="/icon.png"
                        alt="logo"
                        sx={{
                            height: '50px',
                        }}
                    />
                </Button>
                <Box sx={{ flexGrow: 1, display: 'flex' }}>
                    {pages.map((page) => (
                        <Button
                            key={page.title}
                            component={Link}
                            to={page.url}
                            disableRipple
                            sx={{
                                my: 2,
                                color: 'white',
                                display: 'block',
                                "&:hover": {
                                    backgroundColor: "transparent"
                                },
                                "&:focus": {
                                    outline: "none"
                                }
                            }}
                        >
                            {page.title}
                        </Button>
                    ))}
                </Box>
                <Box sx={{ flexGrow: 0 }}>
                    <Tooltip title={"Open menu"}>
                        <IconButton onClick={(event) => {setAnchorEl(event.currentTarget);}}>
                            {userId ?
                                <Avatar src={BASE_URL + userProfilePictureUrl} />
                                :
                                <Avatar sx={{ bgcolor: 'primary.main' }}>
                                    <LoginIcon />
                                </Avatar>
                            }
                        </IconButton>
                    </Tooltip>
                    <Menu
                        anchorEl={anchorEl}
                        open={Boolean(anchorEl)}
                        onClose={() => {setAnchorEl(null);}}
                    >
                        {userMenu.map((menuItem) => (
                            <MenuItem
                                key={menuItem.title}
                                onClick={() => {
                                    menuItem.action();
                                    setAnchorEl(null);
                                }}
                            >
                                {menuItem.title}
                            </MenuItem>
                        ))}
                    </Menu>
                </Box>
            </Toolbar>
        </AppBar>
    )
}

export default ApplicationBar;