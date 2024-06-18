import {Account} from "@/shared/account/model.ts";
import axiosInstance from "@/shared/api/axiosClient.ts";

export const GetAccountsApi = async (userId: number): Promise<Account[]> => {
    const response = await axiosInstance.get(`${userId}/accounts`, {
        params: {
            userId: userId
        },
    });

    if (response.status === 200) {
        return response.data;
    } else {
        return []
    }
}

export const AddAccountApi = (props: {
    userId: number,
    name: string,
    balance: number,
    firstCurrency: string,
    secondCurrency: string
}) => {
    return axiosInstance.post('accounts',
        {
            userId: props.userId,
            name: props.name,
            balance: props.balance,
            firstCurrencyId: props.firstCurrency,
            secondCurrencyId: props.secondCurrency
        });
}
