import axios from "axios";

const host: string = "https://localhost:7093/";

const axiosInstance = axios.create({
    baseURL : `${host}`,
    withCredentials: true
});

// Добавьте перехватчик ответа
axiosInstance.interceptors.response.use(
    response => {
        // Любой статус кода, который находится в диапазоне 2xx, вызывает эту функцию для обработки ответа
        return response;
    },
    error => {
        // Любой статус кода, который выходит за пределы диапазона 2xx, вызывает эту функцию для обработки ошибок
        if (error.response && error.response.status === 401) {
            // Перенаправляем на страницу логина
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;
export const enum API_URLS {
    REGISTER = `account/register`,
    LOGIN = `account/login`,
    PRIVATE_METHOD = 'private/getOkMessage',
    REFRESH = 'account/refreshToken',
    GET_ME = 'private/getMe',
    GET_POSTS = 'posts',
    CHANGE_PROFILE = 'account/change'
};