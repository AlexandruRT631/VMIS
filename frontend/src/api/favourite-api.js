import axios from "axios";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/favourite';

export const addFavouriteListing = async (userId, favouriteListingId) => {
    try {
        const response = await axios.post(BASE_URL + '/addFavouriteListing?userId=' +
            userId + '&favouriteListingId=' + favouriteListingId
        );
        return response.data;
    } catch (error) {
        console.error('Error adding favourite listing:', error);
        throw error;
    }
}

export const removeFavouriteListing = async (userId, favouriteListingId) => {
    try {
        const response = await axios.delete(BASE_URL + '/removeFavouriteListing?userId=' +
            userId + '&favouriteListingId=' + favouriteListingId
        );
        return response.data;
    } catch (error) {
        console.error('Error removing favourite listing:', error);
        throw error;
    }
}

export const addFavouriteUser = async (userId, favouriteUserId) => {
    try {
        const response = await axios.post(BASE_URL + '/addFavouriteUser?userId=' +
            userId + '&favouriteUserId=' + favouriteUserId
        );
        return response.data;
    } catch (error) {
        console.error('Error adding favourite user:', error);
        throw error;
    }
}

export const removeFavouriteUser = async (userId, favouriteUserId) => {
    try {
        const response = await axios.delete(BASE_URL + '/removeFavouriteUser?userId=' +
            userId + '&favouriteUserId=' + favouriteUserId
        );
        return response.data;
    } catch (error) {
        console.error('Error removing favourite user:', error);
        throw error;
    }
}