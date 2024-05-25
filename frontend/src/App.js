import {Component} from "react";
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import Home from "./pages/home/home";
import Listing from "./pages/listing/listing";
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import ListingCreate from "./pages/listing-create/listing-create";

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
                <Container>
                    <Router>
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/listing/:id" element={<Listing/>}/>
                            <Route path="/createListing" element={<ListingCreate/>}/>
                        </Routes>
                    </Router>
                </Container>
            </ThemeProvider>
        );
    }
}

export default App;
