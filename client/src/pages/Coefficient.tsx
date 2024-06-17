import {useEffect, useState} from "react";
import {Button} from "@/components/ui/button.tsx";
import {Input} from "@/components/ui/input.tsx";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {FormProvider, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {z} from "zod";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";
import {useCurrencyStore} from "@/entities/Currency/currencyStore.ts";
import {CurrencyCoefficient} from "@/shared/coefficient/model.ts";
import {createCoefficient, getCoefficients} from "@/shared/coefficient";

export const Coefficient = () => {

    const navigate = useNavigate();
    const currencies = useCurrencyStore(state => state.currencies);
    const setCurrencies = useCurrencyStore(state => state.setCurrencies);

    const [coefficients, setCoefficients] = useState<CurrencyCoefficient[] | []>([]);
    const [addCoef, setAddCoef] = useState(false);

    useEffect(() => {
        if (currencies === null) {
            setCurrencies();
        }

        const fetchCoefficients = async () => {
            const data = await getCoefficients();
            setCoefficients(data);
        }

        fetchCoefficients();

    }, [])

    const formScheme = z.object({
        firstCurrency: z.coerce.number(),
        secondCurrency: z.coerce.number(),
        coefficient: z.coerce.number().min(0, {message: 'Коэффициент не может быть меньше 0'})
    });

    const form = useForm<z.infer<typeof formScheme>>({
        resolver: zodResolver(formScheme),
        defaultValues: {
            coefficient: 0,
            firstCurrency: 1,
            secondCurrency: 2
        }
    });

    const onSubmit = (values: z.infer<typeof formScheme>) => {

        const promise = createCoefficient({
            coefficient: values.coefficient,
            fromId: values.firstCurrency,
            toId: values.secondCurrency
        });

        toast.promise(promise, {
            loading: "Загрузка ...",
            success: resp => {
                if (resp.status >= 400) {
                    throw new Error(`Statues code ${resp.status}`);
                }
                setCoefficients(prev =>
                    [...prev,
                        {
                            id: resp.data.id,
                            coefficient: values.coefficient,
                            fromCurrencyName: resp.data.fromCurrencyName,
                            toCurrencyName: resp.data.toCurrencyName
                        }
                    ])
                setAddCoef(false);
                return "Коэффициент перевода валюты успешно создан";
            },
            error: e => {
                return `${e.response.data.detail}`;
            },
        });
    };

    return (
        <>
            <Button className='mr-1' onClick={() => navigate('/')}>Главная</Button>
            <Button onClick={() => setAddCoef(true)}>Добавить коэффициент</Button>
            {
                addCoef &&
                <div className='flex justify-center'>
                    <FormProvider {...form}>
                        <form className='mt-1' onSubmit={form.handleSubmit(onSubmit)}>

                            <FormField
                                control={form.control}
                                name="firstCurrency"
                                render={({field}) => (
                                    <FormItem className='w-96'>
                                        <FormLabel>Из</FormLabel>
                                        <Select onValueChange={field.onChange} defaultValue={field.value.toString()}>
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Выберите из какой валюты"/>
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                {
                                                    currencies!.map(x => (
                                                        <SelectItem value={x.id.toString()}>{x.name}</SelectItem>
                                                    ))
                                                }
                                            </SelectContent>
                                        </Select>
                                        <FormMessage/>
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="secondCurrency"
                                render={({field}) => (
                                    <FormItem>
                                        <FormLabel>В</FormLabel>
                                        <Select onValueChange={field.onChange} defaultValue={field.value.toString()}>
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Выберите в какую валюту"/>
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                {
                                                    currencies!.map(x => (
                                                        <SelectItem value={x.id.toString()}>{x.name}</SelectItem>
                                                    ))
                                                }
                                            </SelectContent>
                                        </Select>
                                        <FormMessage/>
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="coefficient"
                                render={({field}) => (
                                    <FormItem>
                                        <FormLabel>Коэффициент</FormLabel>
                                        <FormControl>
                                            <Input type={'number'} placeholder="Укажите коэффициент" {...field} />
                                        </FormControl>
                                        <FormMessage/>
                                    </FormItem>
                                )}
                            />

                            <Button className='mt-2' type="submit">Создать</Button>
                        </form>
                    </FormProvider>
                </div>
            }

            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead>Из</TableHead>
                        <TableHead>В</TableHead>
                        <TableHead className='w-0.5'>Коэффициент</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {
                        coefficients.map(x => (
                            <TableRow style={{textAlign: 'start'}}>
                                <TableCell>{x.fromCurrencyName}</TableCell>
                                <TableCell>{x.toCurrencyName}</TableCell>
                                <TableCell>{x.coefficient}</TableCell>
                            </TableRow>
                        ))
                    }
                </TableBody>
            </Table>
        </>
    )
}