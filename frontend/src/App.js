import {Component} from "react";
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import Home from "./home/home";
import Listing from "./listing/listing";
import {Container, createTheme, ThemeProvider} from "@mui/material";

const theme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#fff800',
        },
        secondary: {
            main: '#99006d',
        },
    },
});

class App extends Component {
    render() {
        return (
            <ThemeProvider theme={theme}>
                <Container>
                    <Router>
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/listing/:id" element={<Listing/>}/>
                        </Routes>
                    </Router>
                </Container>
            </ThemeProvider>
        );
    }
}

export default App;
