import axios from 'axios';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/car';

export const getCarByMakeModelYear = async (makeId, modelId, year) => {
    try {
        const response = await axios.get(`${BASE_URL}/getCarByMakeModelYear?makeId=${makeId}&modelId=${modelId}&year=${year}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching car with makeId ${makeId}, modelId ${modelId}, and year ${year}:`, error);
        throw error;
    }
}