import {create} from "zustand";
import {getTransferHistoryFilters} from "@/shared/transferHistory";

interface Option {
    label: string,
    value: string
}

interface filterStore {
    accountOptions: Option[],
    currencyOptions: Option[],
    setData: (userId: number) => void
}

export const useTransferHistoryFilterStore = create<filterStore>(
    (set) => ({
        accountOptions: [],
        currencyOptions: [],
        setData: async (userId: number) => {
            const {accountOptions, currencyOptions} = await getTransferHistoryFilters(userId);
            set({accountOptions,currencyOptions});
        },
    })
)