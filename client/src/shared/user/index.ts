import axiosInstance from "@/shared/api/axiosClient.ts";

export const LoginUserApi = (values: { password: string, username: string }) => {
    return axiosInstance.post('auth/login', {
        userName: values.username,
        password: values.password
    });
};

export const RegisterUserApi = async (values: { password: string, username: string }) => {
    return await axiosInstance.post('auth/register',
        {
            userName: values.username,
            password: values.password
        });
};

export const LogoutUserApi = () => {
    axiosInstance.post('auth/logout')
}
