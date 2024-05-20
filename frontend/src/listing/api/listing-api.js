import axios from 'axios';

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/listing';

export const getAllListings = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching listings:', error);
        throw error;
    }
};

export const getListingById = async (id) => {
    try {
        const response = await axios.get(`${BASE_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching listing with ID ${id}:`, error);
        throw error;
    }
};

export const createListing = async (listingDto, images) => {
    try {
        const formData = new FormData();
        formData.append('listingDto', JSON.stringify(listingDto));
        images.forEach((image, index) => {
            formData.append('images', image);
        });

        const response = await axios.post(BASE_URL, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error creating listing:', error);
        throw error;
    }
};

export const updateListing = async (listingDto, images) => {
    try {
        const formData = new FormData();
        formData.append('listingDto', JSON.stringify(listingDto));
        images.forEach((image, index) => {
            formData.append('images', image);
        });

        const response = await axios.put(BASE_URL, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error updating listing:', error);
        throw error;
    }
};

export const deleteListing = async (id) => {
    try {
        const response = await axios.delete(`${BASE_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error deleting listing with ID ${id}:`, error);
        throw error;
    }
};
