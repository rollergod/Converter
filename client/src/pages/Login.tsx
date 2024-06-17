import {FormProvider, useForm} from "react-hook-form";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {z} from "zod";
import {zodResolver} from "@hookform/resolvers/zod";
import {useNavigate} from "react-router-dom";
import {usePersonStore} from "@/entities/User/userStore.ts";

export const Login = () => {

    const navigate = useNavigate();
    const setUser = usePersonStore(state => state.setUser);

    const formSchema = z.object({
        username: z.string({required_error: 'Обязательное поле'}).min(2, {
            message: "Логин должен содержать минимум 2 символа.",
        }),
        password: z.string({required_error: 'Обязательное поле'}).min(4, {
            message: "Пароль должен содержать минимум 4 символа.",
        })
    });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
    });

    const onSubmit = (values: z.infer<typeof formSchema>) => {
        setUser(values, navigate);
    };

    return <FormProvider {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
                control={form.control}
                name="username"
                render={({field}) => (
                    <FormItem>
                        <FormLabel>Логин</FormLabel>
                        <FormControl>
                            <Input placeholder="Введите логин" {...field} />
                        </FormControl>
                        <FormMessage/>
                    </FormItem>
                )}
            />

            <FormField
                control={form.control}
                name="password"
                render={({field}) => (
                    <FormItem>
                        <FormLabel>Пароль</FormLabel>
                        <FormControl>
                            <Input placeholder="Введите пароль" {...field} />
                        </FormControl>
                        <FormMessage/>
                    </FormItem>
                )}
            />

            <Button type="submit">Войти</Button>
            <Button className='ml-1' onClick={() => navigate('/register')}>Регистрация</Button>
        </form>
    </FormProvider>
};
