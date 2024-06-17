import {create} from "zustand";
import {Currency} from "@/shared/currency/model.ts";
import {getCurrencies} from "@/shared/currency";

interface currencyStore {
    currencies: Currency[] | null,
    addCurrency: (id: number, name: string) => void,
    setCurrencies: () => void
}

export const useCurrencyStore = create<currencyStore>(
    (set) => ({
        currencies: null,
        setCurrencies: async () => {
            const currencies = await getCurrencies();
            set({currencies});
        },
        addCurrency: (id: number, name: string) =>
            set((state) => (
                {
                    currencies: [
                        ...state.currencies!,
                        {id: id, name: name}
                    ]
                }
            ))
    })
)