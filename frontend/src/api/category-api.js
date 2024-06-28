import axios from "axios";

const BASE_URL = process.env.REACT_APP_LISTING_API_URL + '/api/category';

export const getAllCategories = async () => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching categories:', error);
        throw error;
    }
}

export const createCategory = async (category) => {
    try {
        const response = await axios.post(BASE_URL, category);
        return response.data;
    } catch (error) {
        console.error('Error creating category:', error);
        throw error;
    }
}

export const updateCategory = async (category) => {
    try {
        const response = await axios.put(BASE_URL, category);
        return response.data;
    } catch (error) {
        console.error('Error updating category:', error);
        throw error;
    }
}

export const deleteCategory = async (id) => {
    try {
        const response = await axios.delete(BASE_URL + `/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error deleting category:', error);
        throw error;
    }
}