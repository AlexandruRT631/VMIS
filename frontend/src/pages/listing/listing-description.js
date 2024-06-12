import CommonPaper from "../../common/common-paper";
import {Typography} from "@mui/material";

const ListingDescription = ({ listing }) => {
    return (
        <CommonPaper title={"Description"}>
            <Typography component={"pre"} style={{ whiteSpace: 'pre-wrap', color: 'white' }}>
                {listing.description}
            </Typography>
        </CommonPaper>
    )
}

export default ListingDescription;