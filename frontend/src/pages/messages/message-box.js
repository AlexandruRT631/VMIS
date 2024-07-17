import React, {useEffect, useRef, useState} from "react";
import {getMessages, sendMessage} from "../../api/messages-api";
import {Box, Button, Grid, TextField, Typography} from "@mui/material";
import SendIcon from '@mui/icons-material/Send';
import {Link} from "react-router-dom";
import {getListingById} from "../../api/listing-api";
import { format } from "date-fns";

const USER_BASE_WEBSOCKET = process.env.REACT_APP_USER_API_WS;

const MessageBox = ({ userId, conversation }) => {
    const [loading, setLoading] = useState(true);
    const [messages, setMessages] = useState([]);
    const [messagesPage, setMessagesPage] = useState(1);
    const [hasMoreMessages, setHasMoreMessages] = useState(true);
    const [newMessage, setNewMessage] = useState('');
    const [sendingMessage, setSendingMessage] = useState(false);
    const [listing, setListing] = useState(null);
    const messageBoxRef = useRef(null);
    const pageSize = 75;
    const wsRef = useRef(null);

    const fetchData = async (prevMessages, page) => {
        if (conversation) {
            const fetchedMessages = await getMessages(conversation.id, page, pageSize);
            if (fetchedMessages.length < pageSize) {
                setHasMoreMessages(false);
            }
            setMessages([...prevMessages, ...fetchedMessages]);
        }
    }

    useEffect(() => {
        setLoading(true);
        if (conversation) {
            getListingById(conversation.listingId)
                .then(setListing)
                .catch(console.error);
        }
        setMessagesPage(1);
        setHasMoreMessages(true);
        setNewMessage('');
        setSendingMessage(false);
        fetchData([], 1)
            .then(() => setLoading(false))
            .catch(console.error);
    }, [conversation]);

    useEffect(() => {
        if (conversation) {
            const ws = new WebSocket(`${USER_BASE_WEBSOCKET}/conversation/${conversation.id}`);
            ws.onmessage = (event) => {
                const message = JSON.parse(event.data);
                setMessages((prevMessages) => [...prevMessages, {
                    id: message.Id,
                    senderId: message.SenderId,
                    content: message.Content,
                    sentAt: message.SentAt,
                    conversation: conversation
                }]);
            };
            wsRef.current = ws;
            return () => {
                ws.close();
            };
        }
    }, [conversation]);

    useEffect(() => {
        if (messagesPage !== 1) {
            fetchData(messages, messagesPage)
                .then(() => setLoading(false))
                .catch(console.error);
        }
    }, [messagesPage]);

    useEffect(() => {
        if (messageBoxRef.current) {
            messageBoxRef.current.scrollTop = messageBoxRef.current.scrollHeight;
        }
    }, [loading]);

    useEffect(() => {
        if (!loading && messageBoxRef.current) {
            messageBoxRef.current.scrollTop = messageBoxRef.current.scrollHeight;
        }
    }, [messages]);

    console.log(conversation);
    console.log(listing);
    console.log(messages);

    if (loading) {
        return null;
    }

    if (!conversation) {
        return null;
    }

    return (
        <Box sx={{
            height: "100%",
        }}>
            <Box sx={{
                height: "10%",
                display: "flex",
                backgroundColor: "#1E1E1E",
                alignItems: "center",
                justifyContent: "center",
            }}>
                {listing && (
                    <Link to={`/listing/${listing.id}`} style={{ textDecoration: 'none' }}>
                        {listing.title}
                    </Link>
                )}
            </Box>
            <Box ref={messageBoxRef} sx={{ height: '80%', overflow: 'auto' }}>
                {hasMoreMessages && messages.length % pageSize === 0 && messages.length > 0 && (
                    <Grid container justifyContent="center" sx={{ marginTop: 2 }}>
                        <Button variant={"contained"} onClick={() => setMessagesPage((prevPage) => prevPage + 1)}>
                            Load more
                        </Button>
                    </Grid>
                )}
                {messages.sort((a, b) => new Date(a.sentAt) - new Date(b.sentAt)).map((message) => (
                    <Grid container key={message.id} justifyContent={userId === message.senderId ? "flex-end" : "flex-start"} sx={{ padding: 1 }}>
                        <Box
                            sx={{
                                maxWidth: '80%',
                                padding: 1,
                                backgroundColor: userId === message.senderId ? "primary.main" : "secondary.main",
                                borderRadius: 2,
                                boxShadow: 1,
                                wordWrap: 'break-word',
                                overflow: 'hidden',
                                textOverflow: 'ellipsis',
                            }}
                        >
                            <Typography
                                variant="body2"
                                sx={{ color: "gray", marginBottom: 1 }}
                            >
                                {format(new Date(message.sentAt), 'PPpp')}
                            </Typography>
                            <Typography
                                variant={"body1"}
                                sx={{
                                    color: userId === message.senderId ? "black" : "inherit"
                                }}
                            >
                                {message.content}
                            </Typography>
                        </Box>
                    </Grid>
                ))}
            </Box>
            <Box sx={{
                height: "10%",
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                justifyContent: "center",
                padding: 2
            }}>
                <Box
                    sx={{
                        width: '90%',
                    }}
                >
                    <TextField
                        value={newMessage}
                        onChange={(e) => setNewMessage(e.target.value)}
                        fullWidth
                    />
                </Box>
                <Box
                    sx={{
                        width: '10%',
                        marginLeft: 2
                    }}
                >
                    <Button
                        variant="contained"
                        onClick={() => {
                            setSendingMessage(true);
                            sendMessage({
                                conversation: {
                                    id: conversation.id
                                },
                                senderId: userId,
                                content: newMessage,
                            })
                                .then((message) => {
                                    setMessages([...messages, message]);
                                    setNewMessage('');
                                    setSendingMessage(false);
                                })
                                .catch(console.error);
                        }}
                        disabled={newMessage.length === 0 || sendingMessage}
                    >
                        <SendIcon />
                    </Button>
                </Box>
            </Box>
        </Box>
    );
};

export default MessageBox;
