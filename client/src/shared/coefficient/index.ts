import axios from "axios";
import {CurrencyCoefficient} from "@/shared/coefficient/model.ts";

export const getCoefficients = async (): Promise<CurrencyCoefficient[]> => {
    const promise = await axios.get('https://localhost:7093/Coefficients', {
        withCredentials: true
    });

    if (promise.status === 200) {
        return promise.data
    } else {
        return [];
    }
}

export const createCoefficient = (props: { coefficient: number, fromId: number, toId: number }) => {
    return axios.post('https://localhost:7093/Coefficients',
        {
            coefficient: props.coefficient,
            fromId: props.fromId,
            toId: props.toId
        }, {
            withCredentials: true
        });
}