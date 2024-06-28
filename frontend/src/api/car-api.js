import axios from 'axios';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/car';

export const getCarByModelYear = async (modelId, year) => {
    try {
        const response = await axios.get(`${BASE_URL}/getCarByModelYear?modelId=${modelId}&year=${year}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching car with modelId ${modelId} and year ${year}:`, error);
        throw error;
    }
}

export const getCarsByModel = async (modelId) => {
    try {
        const response = await axios.get(`${BASE_URL}/getCarsByModel?modelId=${modelId}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching cars with modelId ${modelId}:`, error);
        throw error;
    }
}

export const createCar = async (car) => {
    try {
        const response = await axios.post(BASE_URL, car);
        return response.data;
    } catch (error) {
        console.error('Error creating car:', error);
        throw error;
    }
}

export const updateCar = async (car) => {
    try {
        const response = await axios.put(BASE_URL, car);
        return response.data;
    } catch (error) {
        console.error('Error updating car:', error);
        throw error;
    }
}