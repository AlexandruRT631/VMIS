import axios from "axios";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/conversation';

export const getConversations = async (userId, pageIndex = 1, pageSize = 20) => {
    try {
        const response = await axios.get(`${BASE_URL}/${userId}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching conversations:', error);
        throw error;
    }
}