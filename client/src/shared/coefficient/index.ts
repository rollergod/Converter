import {CurrencyCoefficient} from "@/shared/coefficient/model.ts";
import axiosInstance from "@/shared/api/axiosClient.ts";

export const getCoefficients = async (): Promise<CurrencyCoefficient[]> => {
    const promise = await axiosInstance.get('coefficients');

    if (promise.status === 200) {
        return promise.data
    } else {
        return [];
    }
}

export const createCoefficient = (props: { coefficient: number, fromId: number, toId: number }) => {
    return axiosInstance.post('coefficients',
        {
            coefficient: props.coefficient,
            fromId: props.fromId,
            toId: props.toId
        });
}