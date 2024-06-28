import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/traction';

export const getAllTractions = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching tractions:', error);
        throw error;
    }
}

export const createTraction = async (traction) => {
    try {
        const response = await axios.post(BASE_URL, traction);
        return response.data;
    } catch (error) {
        console.error('Error creating traction:', error);
        throw error;
    }
}

export const updateTraction = async (traction) => {
    try {
        const response = await axios.put(BASE_URL, traction);
        return response.data;
    } catch (error) {
        console.error('Error updating traction:', error);
        throw error;
    }
}