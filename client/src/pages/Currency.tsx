import {Button} from "@/components/ui/button.tsx";
import {Input} from "@/components/ui/input.tsx";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {FormProvider, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {z} from "zod";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";
import {createCurrency} from "@/shared/currency";

export const Currency = () => {

    const navigate = useNavigate();

    const formScheme = z.object({
        name: z.string({required_error: 'Обязательное поле'})
            .min(2, {message: 'Название должно содержать минимум два симолва'}),
    });

    const form = useForm<z.infer<typeof formScheme>>({
        resolver: zodResolver(formScheme),
    });

    const onSubmit = (values: z.infer<typeof formScheme>) => {

        const promise = createCurrency(values.name);

        toast.promise(promise, {
            loading: "Загрузка ...",
            success: resp => {
                if (resp.status >= 400) {
                    throw new Error(`Ошибка - ${resp.status}`);
                }

                navigate('/');
                return "Валюта создана";
            },
            error: e => {
                return `${e.response.data.detail}`;
            },
        });
    };

    return (
        <>
            <Button className='mr-1' onClick={() => navigate('/')}>Главная</Button>
                <div className='flex justify-center'>
                    <FormProvider {...form}>
                        <form className='mt-1' onSubmit={form.handleSubmit(onSubmit)}>

                            <FormField
                                control={form.control}
                                name="name"
                                render={({field}) => (
                                    <FormItem>
                                        <FormLabel>Название</FormLabel>
                                        <FormControl>
                                            <Input type={'text'} placeholder="Название валюты" {...field} />
                                        </FormControl>
                                        <FormMessage/>
                                    </FormItem>
                                )}
                            />

                            <Button className='mt-2' type="submit">Создать</Button>
                        </form>
                    </FormProvider>
                </div>
        </>
    )
}