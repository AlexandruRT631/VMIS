import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/transmission';

export const getAllTransmissions = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching transmissions:', error);
        throw error;
    }
}

export const createTransmission = async (transmission) => {
    try {
        const response = await axios.post(BASE_URL, transmission);
        return response.data;
    } catch (error) {
        console.error('Error creating transmission:', error);
        throw error;
    }
}

export const updateTransmission = async (transmission) => {
    try {
        const response = await axios.put(BASE_URL, transmission);
        return response.data;
    } catch (error) {
        console.error('Error updating transmission:', error);
        throw error;
    }
}