import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/engine';

export const getAllEngines = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching engines:', error);
        throw error;
    }
}

export const createEngine = async (engine) => {
    try {
        const response = await axios.post(BASE_URL, engine);
        return response.data;
    } catch (error) {
        console.error('Error creating engine:', error);
        throw error;
    }
}

export const updateEngine = async (engine) => {
    try {
        const response = await axios.put(BASE_URL, engine);
        return response.data;
    } catch (error) {
        console.error('Error updating engine:', error);
        throw error;
    }
}