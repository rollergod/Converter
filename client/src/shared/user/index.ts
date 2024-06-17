import axios from "axios";
import axiosInstance from "@/shared/api/axiosClient.ts";

export const LoginUserApi = (values: { password: string, username: string }) => {
    // return axios.post('https://localhost:7093/Login', {
    //     userName: values.username,
    //     password: values.password
    // }, {
    //     withCredentials: true
    // });

    return axiosInstance.post('Login', {
        userName: values.username,
        password: values.password
    });
};

export const RegisterUserApi = async (values: { password: string, username: string }) => {
    return await axios.post('https://localhost:7093/Register', {
        userName: values.username,
        password: values.password
    });
};

export const LogoutUserApi = () => {
    axios.post('https://localhost:7093/Logout',
        {},
        {
            withCredentials: true
        })
}
