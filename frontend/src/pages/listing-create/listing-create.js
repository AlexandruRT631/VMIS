import {Box, Grid} from "@mui/material";
import ListingCreateCar from "./listing-create-car";
import {useState} from "react";

const ListingCreate = () => {
    const [car, setCar] = useState(null);

    console.log(car);

    return (
        <Box>
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={4}
                mb={4}
            >
                <Grid item xs={12}>
                    <ListingCreateCar setCar={setCar}/>
                </Grid>
            </Grid>
        </Box>
    );
}

export default ListingCreate;