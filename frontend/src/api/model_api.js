import axios from 'axios';

const BASE_URL = process.env.REACT_APP_MODEL_API_URL;

export const getMakeModelYearFromImage = async (image) => {
    const formData = new FormData();
    formData.append('image', image);

    try {
        const response = await axios.post(`${BASE_URL}/classify`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error uploading image:', error);
        throw error;
    }
};

