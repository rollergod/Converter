import {create} from "zustand";
import {Account} from "@/shared/account/model.ts";
import {AddAccountApi, GetAccountsApi} from "@/shared/account";
import toast from "react-hot-toast";

interface accountStore {
    accounts: Account[],
    setAccounts: (userId: number) => void,
    addAccount: (props: {
        userId: number,
        name: string,
        balance: number,
        firstCurrency: string,
        secondCurrency: string,
        addedAccount: () => void
    }) => void,
    updateAccount: (accountId: number, isFirstMain: boolean, balance: number) => void
}

export const useAccountStore = create<accountStore>(
    (set) => ({
        accounts: [],
        setAccounts: async (userId: number) => {
            const accounts = await GetAccountsApi(userId);
            set({accounts});
        },
        addAccount: (props: {
            userId: number,
            name: string,
            balance: number,
            firstCurrency: string,
            secondCurrency: string,
            addedAccount: () => void
        }) => {
            const promise = AddAccountApi({...props});

            toast.promise(promise, {
                loading: "Загрузка ...",
                success: resp => {
                    if (resp.status >= 400) {
                        throw new Error(`Ошибка -  ${resp.status}`);
                    }

                    props.addedAccount();

                    set((state) => (
                        {
                            accounts: [
                                ...state.accounts,
                                {
                                    balance: resp.data.balance,
                                    firstCurrencyName: resp.data.firstCurrencyName,
                                    id: resp.data.id,
                                    isFirstCurrencyMain: resp.data.isFirstCurrencyMain,
                                    name: resp.data.name,
                                    secondCurrencyName: resp.data.secondCurrencyName
                                }
                            ]
                        }
                    ))

                    return "Аккаунт успешно создан";
                },
                error: e => {
                    return `${e.response.data.detail}`;
                },
            });
        },
        updateAccount: (accountId: number, isFirstMain: boolean, balance: number) => set((state) => {
            const obj = state.accounts.find(x => x.id == accountId);

            if (obj) {
                obj.isFirstCurrencyMain = isFirstMain;
                obj.balance = balance;
            }
            return {
                accounts: [
                    ...state.accounts
                ]
            }
        })
    })
);
