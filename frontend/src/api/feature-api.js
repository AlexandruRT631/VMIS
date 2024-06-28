import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/feature';

export const getAllFeatures = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching features:', error);
        throw error;
    }
}

export const createFeature = async (feature) => {
    try {
        const response = await axios.post(BASE_URL, feature);
        return response.data;
    } catch (error) {
        console.error('Error creating feature:', error);
        throw error;
    }
}

export const updateFeature = async (feature) => {
    try {
        const response = await axios.put(BASE_URL, feature);
        return response.data;
    } catch (error) {
        console.error('Error updating feature:', error);
        throw error;
    }
}