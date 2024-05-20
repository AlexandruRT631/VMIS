import React from "react";
import {Table, TableBody, TableCell, TableRow, Typography} from "@mui/material";
import CommonPaper from "../common/common-paper";

const ListingTechnicalData = ({listing}) => {
    return (
        <CommonPaper title={"TechnicalData"}>
            <Table>
                <TableBody>
                    <TableRow>
                        <TableCell>Make</TableCell>
                        <TableCell>{listing.car.model.make.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Model</TableCell>
                        <TableCell>{listing.car.model.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Year</TableCell>
                        <TableCell>{listing.year}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Category</TableCell>
                        <TableCell>{listing.category.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Mileage</TableCell>
                        <TableCell>{listing.mileage}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Engine Code</TableCell>
                        <TableCell>{listing.engine.engineCode}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Cubic Capacity</TableCell>
                        <TableCell>{listing.engine.displacement}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Power</TableCell>
                        <TableCell>{listing.engine.power}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Torque</TableCell>
                        <TableCell>{listing.engine.torque}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Fuel</TableCell>
                        <TableCell>{listing.engine.fuel.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Door Type</TableCell>
                        <TableCell>{listing.doorType.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Transmission</TableCell>
                        <TableCell>{listing.transmission.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Traction</TableCell>
                        <TableCell>{listing.traction.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Color</TableCell>
                        <TableCell>{listing.exteriorColor.name}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Interior Color</TableCell>
                        <TableCell>{listing.interiorColor.name}</TableCell>
                    </TableRow>
                </TableBody>
            </Table>
        </CommonPaper>
    )
}

export default ListingTechnicalData;