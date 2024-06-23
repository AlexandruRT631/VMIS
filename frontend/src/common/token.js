import {jwtDecode} from "jwt-decode";

export const setToken = (data) => {
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
}

export const removeToken = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
}

export const getAccessToken = () => {
    return localStorage.getItem('accessToken');
}

export const getRefreshToken = () => {
    return localStorage.getItem('refreshToken');
}

export const getUserId = () => {
    const token = getAccessToken();
    if (token) {
        try {
            const decodedToken = jwtDecode(token);
            return parseInt(decodedToken.nameid) || null;
        }
        catch (error) {
            console.error('Error decoding token:', error);
            return null;
        }
    }
}

export const getUserName = () => {
    const token = getAccessToken();
    if (token) {
        try {
            const decodedToken = jwtDecode(token);
            return decodedToken.unique_name || null;
        }
        catch (error) {
            console.error('Error decoding token:', error);
            return null;
        }
    }
}

export const getUserRole = () => {
    const token = getAccessToken();
    if (token) {
        try {
            const decodedToken = jwtDecode(token);
            return decodedToken.role || null;
        }
        catch (error) {
            console.error('Error decoding token:', error);
            return null;
        }
    }
}

export const getUserProfilePictureUrl = () => {
    const token = getAccessToken();
    if (token) {
        try {
            const decodedToken = jwtDecode(token);
            return decodedToken.ProfilePictureUrl || null;
        }
        catch (error) {
            console.error('Error decoding token:', error);
            return null;
        }
    }
}