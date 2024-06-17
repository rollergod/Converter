import axios from "axios";
import {Account} from "@/shared/account/model.ts";

export const getCurrencies = async (): Promise<Account[]> => {
    const promise = await axios.get('https://localhost:7093/Currencies',
        {
            withCredentials: true
        });

    if (promise.status === 200) {
        return promise.data;
    } else {
        return [];
    }
}
