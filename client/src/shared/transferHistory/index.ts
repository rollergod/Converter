import axiosInstance from "@/shared/api/axiosClient.ts";

export const getTransferHistory = async (props: {
    userId: number,
    startDate: Date,
    endDate: Date,
    accountIds: string[],
    currencyIds: string[]
}) => {
    const promise = await axiosInstance.get(`transfers/${props.userId}/history`,
        {
            params: {
                userId: props.userId,
                startDate: props.startDate,
                endDate: props.endDate,
                accountIds: props.accountIds.join(','),
                currencyIds: props.currencyIds.join(','),
            },
        });

    if (promise.status === 200) {
        return promise.data;
    } else {
        return [];
    }
}

export const getTransferHistoryFilters = async (userId: number) => {

    const promise = await axiosInstance.get(`transfers/${userId}/filters`,
        {
            params: {
                userId: userId
            }
        })

    if (promise.status === 200) {
        return promise.data;
    } else {
        return {}
    }
}