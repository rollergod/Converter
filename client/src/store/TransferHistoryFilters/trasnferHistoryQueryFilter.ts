import {create} from "zustand";

interface Query {
    startDate: Date,
    endDate: Date,
    accountIds: string[],
    currencyIds: string[],
}

interface QueryState {
    query: Query,
    setData: (query: Query) => void
}

// Функция для получения даты минус один месяц от текущей даты
const getOneMonthAgoDate = () => {
    const currentDate = new Date();
    currentDate.setMonth(currentDate.getMonth() - 1);
    return currentDate;
}

export const useQueryStore = create<QueryState>(
    (set) => ({
        query: {
            startDate: getOneMonthAgoDate(),
            endDate: new Date(),
            accountIds: [],
            currencyIds: []
        },
        setData: (query: Query) => set(() => ({query}))
    })
)