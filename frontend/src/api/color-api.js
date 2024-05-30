import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/color';

export const getAllColors = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching colors:', error);
        throw error;
    }
}