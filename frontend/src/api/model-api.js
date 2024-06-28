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

export const createModel = async (model) => {
    try {
        const response = await axios.post(BASE_URL, model);
        return response.data;
    } catch (error) {
        console.error('Error creating model:', error);
        throw error;
    }
}

export const updateModel = async (model) => {
    try {
        const response = await axios.put(BASE_URL, model);
        return response.data;
    } catch (error) {
        console.error('Error updating model:', error);
        throw error;
    }
}