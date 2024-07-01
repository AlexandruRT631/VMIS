import {Avatar, Box, Button, Divider, Grid, Paper, Typography} from "@mui/material";
import React, {useEffect, useRef, useState} from "react";
import {getUserId} from "../../common/token";
import {getConversations} from "../../api/conversation-api";
import {getListingById} from "../../api/listing-api";
import NavigateBeforeIcon from "@mui/icons-material/NavigateBefore";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import MessageBox from "./message-box";

const LISTING_BASE_URL = process.env.REACT_APP_LISTING_API_URL;

const ConversationThumbnail = ({conversation, setConversationId, conversationId}) => {
    const [loading, setLoading] = useState(true);
    const [listing, setListing] = useState(null);

    console.log(listing);

    useEffect(() => {
        getListingById(conversation.listingId)
            .then((fetchedListing) => {
                setListing(fetchedListing);
                setLoading(false);
            })
            .catch(console.error);
    }, []);

    if (loading) {
        return;
    }

    return (
        <Box
            onClick={() => setConversationId(conversation.id)}
            sx={{
                cursor: "pointer",
                padding: 1,
                backgroundColor: conversation.id === conversationId ? "#1E1E1E" : "inherit",
                borderRadius: 1,
                marginBottom: 1,
                margin: '4px 0'
            }}
        >
            <Grid
                container
                spacing={2}
                sx={{
                    justifyContent: "center",
                    alignItems: "center",
                }}
            >
                <Grid item xs={3} sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <Avatar src={LISTING_BASE_URL + listing.listingImages[0].url}/>
                </Grid>
                <Grid item xs={9}>
                    <Typography
                        variant={"h6"}
                    >
                        {listing.title}
                    </Typography>
                </Grid>
            </Grid>
        </Box>

    )
}

const Messages = () => {
    const [loading, setLoading] = useState(true);
    const [userId, setUserId] = useState(null);
    const [conversations, setConversations] = useState([]);
    const [conversationsPage, setConversationsPage] = useState(1);
    const [conversationId, setConversationId] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            const fetchedUserId = getUserId();
            setUserId(fetchedUserId);
            if (fetchedUserId) {
                const fetchedConversations = await getConversations(fetchedUserId, conversationsPage);
                setConversations(fetchedConversations);
            }
        }

        fetchData()
            .then(() => setLoading(false))
            .catch(console.error);
    }, []);

    useEffect(() => {
        if (userId) {
            getConversations(userId, conversationsPage)
                .then(setConversations)
                .catch(console.error);
        }
    }, [conversationsPage]);

    console.log(conversations);

    if (loading) {
        return;
    }

    if (!userId) {
        return (
            <Typography variant={"h4"}>You must be logged in to view messages</Typography>
        );
    }

    return (
        <Box sx={{
            display: "flex",
            flexDirection: "row",
            height: "85vh"
        }}>
            <Paper sx={{
                height: "100%",
                width: "30%"
            }}>
                <Box sx={{
                    display: "flex",
                    flexDirection: "column",
                    height: "90%",
                    overflow: "auto",
                }}
                >
                    <Grid container>
                        {conversations.conversations.map((conversation, index) => (
                            <Grid item key={conversation.id} xs={12}>
                                <ConversationThumbnail
                                    conversation={conversation}
                                    setConversationId={setConversationId}
                                    conversationId={conversationId}
                                />
                                {index < conversations.conversations.length - 1 && <Divider />}
                            </Grid>
                        ))}
                    </Grid>
                </Box>
                <Box sx={{
                    display: "flex",
                    flexDirection: "row",
                    height: "10%"
                }}
                >
                    <Grid
                        container
                        direction={"row"}
                        justifyContent={"center"}
                        alignItems={"center"}
                        spacing={2}
                        mt={2}
                        mb={2}
                    >
                        <Grid item>
                            <Button
                                variant="contained"
                                onClick={() => {
                                    if (conversationsPage > 1) {
                                        setConversationsPage(conversationsPage - 1);
                                    }
                                }}
                                disabled={conversationsPage <= 1}
                            >
                                <NavigateBeforeIcon />
                            </Button>
                        </Grid>
                        <Grid item>
                            <Typography>
                                Page {conversationsPage} of {conversations.totalPages}
                            </Typography>
                        </Grid>
                        <Grid item>
                            <Button
                                variant="contained"
                                onClick={() => {
                                    if (conversationsPage < conversations.totalPages) {
                                        setConversationsPage(conversationsPage + 1);
                                    }
                                }}
                                disabled={conversationsPage >= conversations.totalPages}
                            >
                                <NavigateNextIcon />
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>
            <Paper sx={{
                height: "100%",
                width: "70%",
                overflow: "auto"
            }}>
                <MessageBox
                    conversationId={conversationId}
                    userId={parseInt(userId)}
                    conversation={conversations.conversations.find((conversation) => conversation.id === conversationId)}
                />
            </Paper>
        </Box>
    )
}

export default Messages;