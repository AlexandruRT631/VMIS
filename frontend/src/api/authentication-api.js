import axios from "axios";
import imageCompression from "browser-image-compression";

const BASE_URL = process.env.REACT_APP_USER_API_URL + '/api/authentication';

export const login = async (email, password) => {
    try {
        const response = await axios.post(BASE_URL + '/login', {
            email,
            password
        });
        return response.data;
    } catch (error) {
        console.error('Error login user:', error);
        throw error;
    }
}

export const register = async (user, image) => {
    try {
        const formData = new FormData();
        formData.append('user', JSON.stringify(user));

        if (image) {
            const compressedImage = await imageCompression(image, {
                maxSizeMB: 1,
                maxWidthOrHeight: 1920,
                useWebWorker: true,
            });
            formData.append('profileImage', compressedImage, compressedImage.name);
        }

        const response = await axios.post(BASE_URL + '/register', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error register user:', error);
        throw error;
    }
};