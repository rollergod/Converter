import axiosInstance from "@/shared/api/axiosClient.ts";

export const getTransferAccounts = async (userId: number) => {
    const promise = await axiosInstance.get(`${userId}/transferAccounts`,
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

export const createTransfer = (props: { fromAccount: string, toAccount: string, amount: number }) => {
    return axiosInstance.post('transfers',
        {
            fromAccountId: props.fromAccount,
            toAccountId: props.toAccount,
            money: props.amount
        });
}