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

export const createColor = async (color) => {
    try {
        const response = await axios.post(BASE_URL, color);
        return response.data;
    } catch (error) {
        console.error('Error creating color:', error);
        throw error;
    }
}

export const updateColor = async (color) => {
    try {
        const response = await axios.put(BASE_URL, color);
        return response.data;
    } catch (error) {
        console.error('Error updating color:', error);
        throw error;
    }
}