import axios from "axios";

export const getTransferHistory = async (props: {
    userId: number,
    startDate: Date,
    endDate: Date,
    accountIds: string[],
    currencyIds: string[]
}) => {
    const promise = await axios.get('https://localhost:7093/TransferMoney',
        {
            withCredentials: true,
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

    const promise = await axios.get('https://localhost:7093/TransferMoneyFilters',
        {
            withCredentials: true,
            params: {
                userId: userId
            }
        })

    if (promise.status === 200) {
        return promise.data;
    } else {
        return {}
    }

    // .then((resp) => {
    //     setFilterStore(resp.data);
    // });
}