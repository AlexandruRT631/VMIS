import axios from "axios";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/user';

export const getUserDetails = async (id) => {
    try {
        const response = await axios.get(BASE_URL + `/details/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching categories:', error);
        throw error;
    }
}