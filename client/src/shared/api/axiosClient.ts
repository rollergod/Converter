import axios from "axios";

const refreshExpiredTokenClosure = () => {
    let isRefreshing = false;
    let runningPromise: Promise<any> = undefined;

    return () => {
        if (isRefreshing) {
            return runningPromise;
        } else {
            isRefreshing = true;
            runningPromise = axiosInstance.post('auth/refresh')
                .then(response => {
                    isRefreshing = false;
                    return response;
                })
                .catch(error => {
                    isRefreshing = false;
                    throw error;
                });
            return runningPromise;
        }
    };
};

const host: string = "https://localhost:7093/";

const axiosInstance = axios.create({
    baseURL : `${host}`,
    withCredentials: true
});
const refreshExpiredToken = refreshExpiredTokenClosure();
axiosInstance.interceptors.response.use(
    response => {
        return response;
    },
    async error => {
        const originalRequest = error.config;
        if(error.response && error.response.status === 401 && !originalRequest._retry){
            originalRequest._retry = true;

            try {
                await refreshExpiredToken();
                return axios(originalRequest);
            } catch (error) {
                window.location.href = '/login';
            }
        }
        else if(error.response && error.response.status === 401) {
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