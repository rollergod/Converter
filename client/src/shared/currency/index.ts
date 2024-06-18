import {Account} from "@/shared/account/model.ts";
import axiosInstance from "@/shared/api/axiosClient.ts";

export const getCurrencies = async (): Promise<Account[]> => {
    const promise = await axiosInstance.get('currencies');

    if (promise.status === 200) {
        return promise.data;
    } else {
        return [];
    }
}

export const createCurrency = async (name: string) => {
    return await axiosInstance.post('currencies', JSON.stringify(name),
        {
            headers: {
                "content-type": "application/json"
            }
        });
}