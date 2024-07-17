import { useEffect, useState, useRef, useCallback } from "react";
import { searchListings } from "../../api/listing-api";
import {useNavigate, useSearchParams} from "react-router-dom";
import {Box, Button, Grid, Typography} from "@mui/material";
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import ListingThumbnail from "../../common/listing/listing-thumbnail";
import SearchField from "./search-field";
import { debounce } from "lodash";

const Search = () => {
    const [searchParams] = useSearchParams();
    const [searchResults, setSearchResults] = useState([]);
    const [listingSearchDto, setListingSearchDto] = useState({});
    const [loadingResults, setLoadingResults] = useState(true);
    const prevListingSearchDto = useRef(listingSearchDto);
    const navigate = useNavigate();
    const [totalPages, setTotalPages] = useState(1);
    const prevPage = useRef(null);

    const page = parseInt(searchParams.get('page')) || 1;

    const fetchListings = useCallback(
        debounce(async (dto, page) => {
            setLoadingResults(true);
            try {
                const result = await searchListings(dto, page, 20);
                setTotalPages(result.totalPages);
                setSearchResults(result.listings);
            } catch (error) {
                console.error(error);
            } finally {
                setLoadingResults(false);
            }
        }, 500),
        []
    );

    useEffect(() => {
        if (JSON.stringify(prevListingSearchDto.current) !== JSON.stringify(listingSearchDto)) {
            prevListingSearchDto.current = listingSearchDto;
            fetchListings(listingSearchDto, page);
            if (page <= 1) {
                navigate(`?page=1`);
            }
        }
        else if (prevPage.current !== page) {
            prevPage.current = page;
            fetchListings(listingSearchDto, page);
        }
    }, [listingSearchDto, page, fetchListings, navigate]);

    console.log(listingSearchDto);

    return (
        <Box sx={{ pb: 4 }}>
            <SearchField setListingSearchDto={setListingSearchDto} />
            <Grid
                container
                direction={"row"}
                justifyContent={"center"}
                alignItems={"stretch"}
                spacing={2}
                mt={4}
                mb={4}
            >
                {loadingResults ? (
                    <div>Loading...</div>
                ) : (
                    searchResults.map((listing, index) => (
                        <Grid key={index} item xs={12}>
                            <ListingThumbnail listing={listing} />
                        </Grid>
                    ))
                )}
            </Grid>
            <Grid container justifyContent="center" alignItems="center" spacing={2}>
                <Grid item>
                    <Button
                        variant="contained"
                        onClick={() => {
                            if (page > 1) {
                                navigate(`?page=${page - 1}`);
                            }
                        }}
                        disabled={page <= 1}
                    >
                        <NavigateBeforeIcon />
                    </Button>
                </Grid>
                <Grid item>
                    <Typography>
                        Page {totalPages === 0 ? 0 : page} of {totalPages}
                    </Typography>
                </Grid>
                <Grid item>
                    <Button
                        variant="contained"
                        onClick={() => {
                            if (page < totalPages) {
                                navigate(`?page=${page + 1}`);
                            }
                        }}
                        disabled={page >= totalPages}
                    >
                        <NavigateNextIcon />
                    </Button>
                </Grid>
            </Grid>
        </Box>
    );
};

export default Search;