import CommonPaper from "../common/common-paper";
import {Box, List, ListItem, ListItemText, Table, TableBody, TableCell, TableRow, Typography} from "@mui/material";
import CheckIcon from '@mui/icons-material/Check';

const ListingFeatures = ({ listing }) => {
    const features = [...listing.featuresExterior, ...listing.featuresInterior].sort((a, b) => a.name.localeCompare(b.name));

    return(
        <CommonPaper title={"Features"}>
            <Table size={"small"}>
                <TableBody>
                    {features.map((feature, index) => (
                        <TableRow key={index}>
                            <TableCell>
                                <Box display={"flex"}>
                                    <CheckIcon sx={{ mr: 2 }}/>
                                    <Typography>{feature.name}</Typography>
                                </Box>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </CommonPaper>
    )
}

export default ListingFeatures;