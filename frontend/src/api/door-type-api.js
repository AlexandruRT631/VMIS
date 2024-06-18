import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/doortype';

export const getAllDoorTypes = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching door types:', error);
        throw error;
    }
}