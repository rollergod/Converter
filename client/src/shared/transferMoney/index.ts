import axiosInstance from "@/shared/api/axiosClient.ts";

export const getTransferAccounts = async (userId: number) => {
    const promise = await axiosInstance.get('TransferAccounts',
        {
            params: {
                userId: userId
            }
        })
    // .then(resp => {
    //     if (resp.status === 200) {
    //         setCurrencyUserAccounts(resp.data.currentUserAccounts);
    //         setAllAccounts(resp.data.transferAccounts);
    //     }
    // })

    if (promise.status === 200) {
        return promise.data;
    } else {
        return {}
    }
}

export const createTransfer = (props: { fromAccount: string, toAccount: string, amount: number }) => {
    return axiosInstance.post('https://localhost:7093/TransferMoney',
        {
            fromAccountId: props.fromAccount,
            toAccountId: props.toAccount,
            money: props.amount
        });
}