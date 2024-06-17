import {FormProvider, useForm} from "react-hook-form";
import {FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {zodResolver} from "@hookform/resolvers/zod";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";
import {z} from "zod";
import {RegisterUserApi} from "@/shared/user";

export const Register = () => {

    const navigate = useNavigate();

    const formSchema = z.object({
        username: z.string({required_error: 'Обязательное поле'}).min(2, 'Логин должен содержать мнимум два символа'),
        password: z.string({required_error: 'Обязательное поле'}).min(4, "Пароль должен содержать минимум 4 символа"),
        confirmPassword: z.string({required_error: 'Обязательное поле'}).min(4, "Confirm password is required")
    }).refine((data) => data.password === data.confirmPassword, {
        message: "Пароли не совпадают",
        path: ["confirmPassword"],
    });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
    })

    const onSubmit = (values: z.infer<typeof formSchema>) => {
        const promise = RegisterUserApi(values);

        toast.promise(promise, {
            loading: "Загрузка ...",
            success: data => {
                if (data.status >= 400) {
                    throw new Error(`Statues code ${data.status}`);
                }

                navigate('/login')
                return "Регистрация прошла успешно";
            },
            error: e => {
                return `${e.response.data.detail}`;
            },
        })
    }

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

            <FormField
                control={form.control}
                name="confirmPassword"
                render={({field}) => (
                    <FormItem>
                        <FormLabel>Пароль</FormLabel>
                        <FormControl>
                            <Input placeholder="Подтвердите пароль" {...field} />
                        </FormControl>
                        <FormMessage/>
                    </FormItem>
                )}
            />

            <Button type="submit">Создать аккаунт</Button>
            <Button className='ml-1' onClick={() => navigate('/login')}>Логин</Button>
        </form>
    </FormProvider>
};
