import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/model';

export const getAllModelsByMakeId = async (makeId) => {
    try {
        const response = await axios.get(`${BASE_URL}/make/${makeId}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching models for make with ID ${makeId}:`, error);
        throw error;
    }
}