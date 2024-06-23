import {Component} from "react";
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import Home from "./pages/home/home";
import Listing from "./pages/listing/listing";
import {Box, Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import ListingCreate from "./pages/listing-create/listing-create";
import Search from "./pages/search/search";
import ApplicationBar from "./common/application-bar";
import Login from "./pages/login/login";
import Register from "./pages/register/register";
import ListingModify from "./pages/listing-modify/listing-modify";
import Profile from "./pages/profile/profile";

const theme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#fff800',
        },
        secondary: {
            main: '#99006d',
        },
        background: {
            default: '#2F2F2F',
            paper: '#1E1E1E',
        },
    },
});

class App extends Component {
    render() {
        return (
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <Router>
                    <ApplicationBar />
                    <Box sx={{height: '64px'}}/>
                    <Container sx={{ mt: 2 }}>
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/listing/:id" element={<Listing/>}/>
                            <Route path="/createListing" element={<ListingCreate/>}/>
                            <Route path="/modifyListing/:id" element={<ListingModify/>}/>
                            <Route path="/search" element={<Search/>}/>
                            <Route path="/login" element={<Login/>}/>
                            <Route path="/register" element={<Register/>}/>
                            <Route path="/profile/:id" element={<Profile/>}/>
                        </Routes>
                    </Container>
                </Router>
            </ThemeProvider>
        );
    }
}

export default App;
