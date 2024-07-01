import axios from "axios";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/message';

export const getMessages = async (conversationId, pageIndex = 1, pageSize = 20) => {
    try {
        const response = await axios.get(`${BASE_URL}/${conversationId}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
        return response.data;
    }
    catch (error) {
        console.error('Error fetching messages:', error);
        throw error;
    }
}

export const sendMessage = async (messageDto, listingId = null) => {
    try {
        if (listingId) {
            const response = await axios.post(BASE_URL + "?listingId=" + listingId, messageDto);
            return response.data;
        }
        else {
            const response = await axios.post(BASE_URL, messageDto);
            return response.data;
        }
    }
    catch (error) {
        console.error('Error sending message:', error);
        throw error;
    }
}