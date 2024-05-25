import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/make';

export const getAllMakes = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching makes:', error);
        throw error;
    }
}