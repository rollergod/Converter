import axios from "axios";

export const convertApi = (accountId: number) => {
    return axios.post(
        'https://localhost:7093/Convert',
        {accountId: accountId},
        {withCredentials: true}
    );
}