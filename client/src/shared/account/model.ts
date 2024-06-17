export interface Account {
    id: number,
    name: string,
    firstCurrencyName: string,
    secondCurrencyName: string,
    balance: number,
    isFirstCurrencyMain: boolean
}