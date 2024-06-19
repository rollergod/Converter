import {useEffect, useState} from "react";
import {z} from "zod";
import {FormProvider, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Button} from "@/components/ui/button.tsx";
import {MultiSelect} from "@/components/ui/multiSelect.tsx";
import {CalendarIcon} from "lucide-react";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover.tsx";
import {Calendar} from "@/components/ui/calendar.tsx";
import {cn} from "@/lib/utils.ts";
import {format} from "date-fns";
import {CartesianGrid, Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";
import {usePersonStore} from "@/entities/User/userStore.ts";
import {useNavigate} from "react-router-dom";
import {getTransferHistory} from "@/shared/transferHistory";
import {useTransferHistoryFilterStore} from "@/entities/TransferHistory/transferHistoryFilterStore.ts";
import {DailyTransferHistory, Query} from "@/shared/transferHistory/model.ts";
import {getOneMonthAgoDate} from "@/features/dateService.ts";

export const TransferHistory = () => {

    const navigate = useNavigate();

    const user = usePersonStore(x => x.user)!;

    const query :Query= {
        startDate: getOneMonthAgoDate(),
        endDate: new Date(),
        accountIds: [],
        currencyIds: []
    };

    const [data, setData] = useState<DailyTransferHistory[]>([])

    const setFilterStore = useTransferHistoryFilterStore(x => x.setData);
    const currencyOptions = useTransferHistoryFilterStore(x => x.currencyOptions);
    const accountOptions = useTransferHistoryFilterStore(x => x.accountOptions);

    useEffect(() => {

        if(user === null){
            navigate('/')
        }

        const fetchTransferHistory = async () => {
            const data = await getTransferHistory({
                userId: user.id,
                startDate: query.startDate,
                endDate: query.endDate,
                accountIds: query.accountIds,
                currencyIds: query.currencyIds
            });
            setData(data);
        }

        fetchTransferHistory();

        setFilterStore(user.id);
    }, []);

    const formScheme = z.object({
        startDate: z.date(),
        endDate: z.date(),
        accountIds: z.array(z.string()),
        currencyIds: z.array(z.string())
    });

    const form = useForm<z.infer<typeof formScheme>>({
        resolver: zodResolver(formScheme),
        defaultValues: {
            startDate: query.startDate,
            endDate: query.endDate,
            accountIds: [],
            currencyIds: [],
        }
    })

    const onSubmit = async (values: z.infer<typeof formScheme>) => {
        const data = await getTransferHistory({
            userId: user.id,
            startDate: values.startDate,
            endDate: values.endDate,
            accountIds: values.accountIds,
            currencyIds: values.currencyIds
        });
        setData(data);
    }

    if (currencyOptions.length === 0 && accountOptions.length === 0) {
        return <>Loading</>
    }

    return (
        <>
            <FormProvider {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)}>

                    <div className='flex justify-around'>
                        <FormField
                            control={form.control}
                            name="startDate"
                            render={({field}) => (
                                <FormItem className="flex flex-col">
                                    <FormLabel>Начальная дата</FormLabel>
                                    <Popover>
                                        <PopoverTrigger asChild>
                                            <FormControl>
                                                <Button
                                                    variant={"outline"}
                                                    className={cn(
                                                        "w-[240px] pl-3 text-left font-normal",
                                                        !field.value && "text-muted-foreground"
                                                    )}
                                                >
                                                    {field.value ? (
                                                        format(field.value, "PPP")
                                                    ) : (
                                                        <span>Выберите дату</span>
                                                    )}
                                                    <CalendarIcon className="ml-auto h-4 w-4 opacity-50"/>
                                                </Button>
                                            </FormControl>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto p-0" align="start">
                                            <Calendar
                                                mode="single"
                                                selected={field.value}
                                                onSelect={field.onChange}
                                                disabled={(date) =>
                                                    date > new Date() || date < new Date("1900-01-01")
                                                }
                                                initialFocus
                                            />
                                        </PopoverContent>
                                    </Popover>
                                    <FormMessage/>
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="endDate"
                            render={({field}) => (
                                <FormItem className="flex flex-col">
                                    <FormLabel>Конечная дата</FormLabel>
                                    <Popover>
                                        <PopoverTrigger asChild>
                                            <FormControl>
                                                <Button
                                                    variant={"outline"}
                                                    className={cn(
                                                        "w-[240px] pl-3 text-left font-normal",
                                                        !field.value && "text-muted-foreground"
                                                    )}
                                                >
                                                    {field.value ? (
                                                        format(field.value, "PPP")
                                                    ) : (
                                                        <span>Выберите дату</span>
                                                    )}
                                                    <CalendarIcon className="ml-auto h-4 w-4 opacity-50"/>
                                                </Button>
                                            </FormControl>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto p-0" align="start">
                                            <Calendar
                                                mode="single"
                                                selected={field.value}
                                                onSelect={field.onChange}
                                                disabled={(date) =>
                                                    date > new Date() || date < new Date("1900-01-01")
                                                }
                                                initialFocus
                                            />
                                        </PopoverContent>
                                    </Popover>
                                    <FormMessage/>
                                </FormItem>
                            )}
                        />

                    </div>

                    <FormField
                        control={form.control}
                        name="accountIds"
                        render={({field}) => (
                            <FormItem className="mb-5">
                                <FormLabel>Выберите счет</FormLabel>
                                <FormControl>
                                    <MultiSelect
                                        options={accountOptions}
                                        onValueChange={field.onChange}
                                        defaultValue={field.value}
                                    />
                                </FormControl>
                                <FormMessage/>
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="currencyIds"
                        render={({field}) => (
                            <FormItem className="mb-5">
                                <FormLabel>Выберите валюту</FormLabel>
                                <FormControl>
                                    <MultiSelect
                                        options={currencyOptions}
                                        onValueChange={field.onChange}
                                        defaultValue={field.value}
                                    />
                                </FormControl>
                                <FormMessage/>
                            </FormItem>
                        )}
                    />
                    <Button className=' mb-2' type="submit">Отправить</Button>
                    <Button className='ml-1' onClick={() => navigate('/')}>На главную</Button>
                </form>
            </FormProvider>

            <ResponsiveContainer width="100%" height={400}>
                <LineChart
                    data={data}
                    margin={{
                        top: 5, right: 30, left: 20, bottom: 5,
                    }}
                >
                    <CartesianGrid strokeDasharray="3 3"/>
                    <XAxis dataKey="transferedDate"/>
                    <YAxis/>
                    <Tooltip/>
                    <Legend/>
                    <Line type="monotone" dataKey="spentAmount" stroke="#8884d8" name="Потрачено"/>
                    <Line type="monotone" dataKey="receivedAmount" stroke="#82ca9d" name="Получено"/>
                </LineChart>
            </ResponsiveContainer>
        </>
    )
};