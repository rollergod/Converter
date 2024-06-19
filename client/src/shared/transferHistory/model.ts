export interface DailyTransferHistory {
    transferedDate: Date,
    spentAmount: number,
    receivedAmount: number
}


export interface Query {
    startDate: Date,
    endDate: Date,
    accountIds: string[],
    currencyIds: string[],
}
