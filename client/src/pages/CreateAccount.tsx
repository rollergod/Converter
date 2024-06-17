import {FormProvider, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useEffect} from "react";
import {z} from "zod";
import {usePersonStore} from "@/entities/User/userStore.ts";
import {useAccountStore} from "@/entities/Account/AccountStore.ts";
import {useCurrencyStore} from "@/entities/Currency/currencyStore.ts";

export const CreateAccount = (props: { addedAccount: () => void }) => {

    const user = usePersonStore(state => state.user!);
    const currencies = useCurrencyStore(state => state.currencies);
    const setCurrencies = useCurrencyStore(state => state.setCurrencies);

    useEffect(() => {
        if (currencies === null) {
            setCurrencies();
        }
    }, [currencies]);

    const addAccount = useAccountStore(state => state.addAccount);

    const formScheme = z.object({
        name: z.string().min(2, {
            message: 'Название должно быть больше двух символов'
        }),
        balance: z.coerce.number().min(0, {
            message: 'Значение должно быть больше 0'
        }),
        firstCurrency: z.string().min(0, {
            message: 'Укажите валюту для перевода'
        }),
        secondCurrency: z.string().min(0, {
            message: 'Укажите валюту для перевода'
        })
    });

    const form = useForm<z.infer<typeof formScheme>>({
        resolver: zodResolver(formScheme),
        defaultValues: {
            name: '',
            balance: 0,
            firstCurrency: 'RUB',
            secondCurrency: 'USD'
        }
    });

    const onSubmit = (values: z.infer<typeof formScheme>) => {
        addAccount({
            userId: user.id,
            name: values.name,
            balance: values.balance,
            firstCurrency: values.firstCurrency,
            secondCurrency: values.secondCurrency,
            addedAccount: props.addedAccount
        });
    };

    return (
        <FormProvider {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
                <FormField
                    control={form.control}
                    name="name"
                    render={({field}) => (
                        <FormItem>
                            <FormLabel>Название валюты</FormLabel>
                            <FormControl>
                                <Input  {...field} />
                            </FormControl>
                            <FormMessage/>
                        </FormItem>
                    )}
                />

                <FormField
                    control={form.control}
                    name="balance"
                    render={({field}) => (
                        <FormItem>
                            <FormLabel>Количество валюты</FormLabel>
                            <FormControl>
                                <Input type={'number'} placeholder="login" {...field} />
                            </FormControl>
                            <FormMessage/>
                        </FormItem>
                    )}
                />

                <FormField
                    control={form.control}
                    name="firstCurrency"
                    render={({field}) => (
                        <FormItem>
                            <FormLabel>Основная валюта</FormLabel>
                            <Select onValueChange={field.onChange} defaultValue={field.value}>
                                <FormControl>
                                    <SelectTrigger>
                                        <SelectValue placeholder="Выберите основную валюту"/>
                                    </SelectTrigger>
                                </FormControl>
                                <SelectContent>
                                    {
                                        currencies?.map(x => (
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
                            <FormLabel>Вторая валюта</FormLabel>
                            <Select onValueChange={field.onChange} defaultValue={field.value}>
                                <FormControl>
                                    <SelectTrigger>
                                        <SelectValue placeholder="Выберите вторую валюту"/>
                                    </SelectTrigger>
                                </FormControl>
                                <SelectContent>
                                    {
                                        currencies?.map(x => (
                                            <SelectItem value={x.id.toString()}>{x.name}</SelectItem>
                                        ))
                                    }
                                </SelectContent>
                            </Select>
                            <FormMessage/>
                        </FormItem>
                    )}
                />

                <Button type="submit">Создать</Button>
            </form>
        </FormProvider>
    )
};

