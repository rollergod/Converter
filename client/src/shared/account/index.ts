import axios from "axios";
import {Account} from "@/shared/account/model.ts";

// export const GetAccountsApi = (userId: number) => {
//     let accounts: Account[] = [];
//     axios.get('https://localhost:7093/Account',
//         {
//             params: {
//                 userId: userId
//             },
//             withCredentials: true
//         }).then(resp => {
//         if (resp.status === 200) {
//             accounts = resp.data;
//         }
//     });
//     console.log(accounts);
//
//     return accounts;
// }

export const GetAccountsApi = async (userId: number): Promise<Account[]> => {
    const response = await axios.get('https://localhost:7093/Account', {
        params: {
            userId: userId
        },
        withCredentials: true
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
    return axios.post('https://localhost:7093/Account',
        {
            userId: props.userId,
            name: props.name,
            balance: props.balance,
            firstCurrencyId: props.firstCurrency,
            secondCurrencyId: props.secondCurrency
        },
        {
            withCredentials: true
        });
}
