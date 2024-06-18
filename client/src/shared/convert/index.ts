import axiosInstance from "@/shared/api/axiosClient.ts";

export const convertApi = (accountId: number) => {
    return axiosInstance.post(
        'convert',
        {accountId: accountId}
    );
}