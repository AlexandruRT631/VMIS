import axios from "axios";
import imageCompression from "browser-image-compression";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/user';

export const getUserDetails = async (id) => {
    try {
        const response = await axios.get(BASE_URL + `/details/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching categories:', error);
        throw error;
    }
}

export const getUsersDetailByIds = async (ids, pageIndex = 1, pageSize = 10) => {
    try {
        const response = await axios.post(`${BASE_URL}/details/ids?pageIndex=${pageIndex}&pageSize=${pageSize}`, ids);
        return response.data;
    } catch (error) {
        console.error(`Error fetching users with ids ${ids}:`, error);
        throw error;
    }
};

export const updateUser = async (user, profileImage) => {
    try {
        const formData = new FormData();
        formData.append('user', JSON.stringify(user));
        if (profileImage) {
            const compressedImage = await imageCompression(profileImage, {
                maxSizeMB: 1,
                maxWidthOrHeight: 1920,
                useWebWorker: true,
            });
            formData.append('profileImage', compressedImage, compressedImage.name);
        }

        const response = await axios.put(BASE_URL, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error updating user:', error);
        throw error;
    }
}