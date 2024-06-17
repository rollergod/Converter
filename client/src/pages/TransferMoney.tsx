import {useEffect, useState} from "react";
import {FormProvider, useForm} from "react-hook-form";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {Button} from "@/components/ui/button.tsx";
import {z} from "zod";
import {zodResolver} from "@hookform/resolvers/zod";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";
import {usePersonStore} from "@/entities/User/userStore.ts";
import {Account} from "@/shared/account/model.ts";
import {createTransfer, getTransferAccounts} from "@/shared/transferMoney";
import {TransferMoneyAccount} from "@/shared/transferMoney/model.ts";

export const TransferMoney = () => {

    const navigate = useNavigate();
    const user = usePersonStore(state => state.user)!;

    const [currencyUserAccounts, setCurrencyUserAccounts] = useState<Account[]>([]);
    const [allAccounts, setAllAccounts] = useState<TransferMoneyAccount[]>([]);

    useEffect(() => {
        if (user === null) {
            navigate('/')
        }

        const fetchTransferAccounts = async () => {
            const {currentUserAccounts, transferAccounts} = await getTransferAccounts(user.id);
            setCurrencyUserAccounts(currentUserAccounts);
            setAllAccounts(transferAccounts);
        }

        fetchTransferAccounts();

        // axios.get('https://localhost:7093/TransferAccounts',
        //     {
        //         withCredentials: true,
        //         params: {
        //             userId: user.id
        //         }
        //     })
        //     .then(resp => {
        //         if (resp.status === 200) {
        //             setCurrencyUserAccounts(resp.data.currentUserAccounts);
        //             setAllAccounts(resp.data.transferAccounts);
        //         }
        //     })
    }, []);

    const formScheme = z.object({
        amount: z.coerce.number().min(1, {
            message: 'Значение должно быть больше 0'
        }),
        fromAccount: z.string({required_error: "Укажите аккаунт отправителя"}),
        toAccount: z.string({required_error: "Укажите аккаунт получателя",})
    }).refine((data) => data.fromAccount !== data.toAccount, {
        message: 'Невозможно осуществить перевод на один и тот же аккаунт',
        path: ['toAccount']
    });

    const form = useForm<z.infer<typeof formScheme>>({
        resolver: zodResolver(formScheme),
        defaultValues: {
            amount: 0,
        }
    });

    const onSubmit = (values: z.infer<typeof formScheme>) => {
        // const promise = axios.post('https://localhost:7093/TransferMoney',
        //     {
        //         fromAccountId: values.fromAccount,
        //         toAccountId: values.toAccount,
        //         money: values.amount
        //     },
        //     {
        //         withCredentials: true
        //     });

        const promise = createTransfer({
            fromAccount: values.fromAccount,
            toAccount: values.toAccount,
            amount: values.amount
        });

        toast.promise(promise, {
            loading: "Загрузка ...",
            success: data => {
                if (data.status >= 400) {
                    throw new Error(`Statues code ${data.status}`);
                }

                navigate('/');
                return "Перевод денежных средств успешно произведён";
            },
            error: e => {
                return `${e.response.data.detail}`;
            },
        });
    };

    return (
        <>
            {
                currencyUserAccounts && allAccounts ?
                    (
                        <FormProvider {...form}>
                            <form onSubmit={form.handleSubmit(onSubmit)}>

                                <FormField
                                    control={form.control}
                                    name="amount"
                                    render={({field}) => (
                                        <FormItem>
                                            <FormLabel>Сумма перевода</FormLabel>
                                            <FormControl>
                                                <Input type={'number'}
                                                       placeholder="Введите сумму перевода" {...field} />
                                            </FormControl>
                                            <FormMessage/>
                                        </FormItem>
                                    )}
                                />

                                <FormField
                                    control={form.control}
                                    name="fromAccount"
                                    render={({field}) => (
                                        <FormItem>
                                            <FormLabel>Из</FormLabel>
                                            <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                <FormControl>
                                                    <SelectTrigger>
                                                        <SelectValue placeholder="Выберите откуда переводить средства"/>
                                                    </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                    {
                                                        currencyUserAccounts?.map(x => (
                                                            <SelectItem value={x.id.toString()}>{x.name} / баланс
                                                                - {x.balance}</SelectItem>
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
                                    name="toAccount"
                                    render={({field}) => (
                                        <FormItem>
                                            <FormLabel>В</FormLabel>
                                            <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                <FormControl>
                                                    <SelectTrigger>
                                                        <SelectValue placeholder="Выберите куда переводить средства"/>
                                                    </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                    {
                                                        allAccounts?.map(x => (
                                                            <SelectItem
                                                                value={x.id.toString()}>{x.id}-{x.name}-{x.userName}</SelectItem>
                                                        ))
                                                    }
                                                </SelectContent>
                                            </Select>
                                            <FormMessage/>
                                        </FormItem>
                                    )}
                                />
                                <Button className='mt-2 mr-2' type="submit">Перевести</Button>
                                <Button onClick={() => navigate('/')}>На главную</Button>
                            </form>
                        </FormProvider>
                    ) : (<>Ошибка</>)
            }
        </>
    )

}