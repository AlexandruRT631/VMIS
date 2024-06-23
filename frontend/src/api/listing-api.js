import axios from 'axios';
import imageCompression from 'browser-image-compression';

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
        for (const image of images) {
            const compressedImage = await imageCompression(image, {
                maxSizeMB: 1,
                maxWidthOrHeight: 1920,
                useWebWorker: true,
            });
            formData.append('images', compressedImage, compressedImage.name);
        }

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
        if (images) {
            for (const image of images) {
                const compressedImage = await imageCompression(image, {
                    maxSizeMB: 1,
                    maxWidthOrHeight: 1920,
                    useWebWorker: true,
                });
                formData.append('images', compressedImage, compressedImage.name);
            }
        }

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

export const searchListings = async (listingSearchDto, pageIndex = 1, pageSize = 10) => {
    try {
        const formData = new FormData();
        if (listingSearchDto) {
            Object.keys(listingSearchDto).forEach(key => {
                const value = listingSearchDto[key];
                if (Array.isArray(value)) {
                    value.forEach(item => formData.append(key, item));
                } else if (value !== null) {
                    formData.append(key, value);
                }
            });
        }
        formData.append('pageIndex', pageIndex);
        formData.append('pageSize', pageSize);

        const response = await axios.post(`${BASE_URL}/search`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error searching listings:', error);
        throw error;
    }
}

export const getActiveListingsByUserId = async (id, pageIndex = 1, pageSize = 10) => {
    try {
        const response = await axios.get(`${BASE_URL}/active/${id}`, {
            params: {
                pageIndex,
                pageSize
            }
        });
        return response.data;
    } catch (error) {
        console.error(`Error fetching listing with ID ${id}:`, error);
        throw error;
    }
};

export const getInactiveListingsByUserId = async (id, pageIndex = 1, pageSize = 10) => {
    try {
        const response = await axios.get(`${BASE_URL}/inactive/${id}`, {
            params: {
                pageIndex,
                pageSize
            }
        });
        return response.data;
    } catch (error) {
        console.error(`Error fetching listing with ID ${id}:`, error);
        throw error;
    }
};