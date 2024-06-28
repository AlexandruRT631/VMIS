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

export const createDoorType = async (doorType) => {
    try {
        const response = await axios.post(BASE_URL, doorType);
        return response.data;
    } catch (error) {
        console.error('Error creating door type:', error);
        throw error;
    }
}

export const updateDoorType = async (doorType) => {
    try {
        const response = await axios.put(BASE_URL, doorType);
        return response.data;
    } catch (error) {
        console.error('Error updating door type:', error);
        throw error;
    }
}