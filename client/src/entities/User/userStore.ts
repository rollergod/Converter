import {create} from 'zustand';
import {createJSONStorage, persist} from "zustand/middleware";
import {User} from "@/shared/user/model.ts";
import {LoginUserApi, LogoutUserApi} from "@/shared/user";
import toast from "react-hot-toast";
import {NavigateFunction} from "react-router-dom";

interface userStore {
    user: User | null,
    setUser: (values: { password: string, username: string }, navigate: NavigateFunction) => void,
    logout: () => void
}

export const usePersonStore = create<userStore>()(
    persist(
        (set) => ({
            user: null,
            setUser: (values: { password: string, username: string, }, navigate: NavigateFunction) => {
                const promise = LoginUserApi(values);

                toast.promise(promise, {
                    loading: "Загрузка ...",
                    success: (resp) => {

                        if (resp.status >= 400) {
                            throw new Error(`Ошибка -  ${resp.status}`);
                        }

                        const user: User = resp.data;
                        set({user});

                        navigate('/');

                        return "Успешный вход в аккаунт";
                    },
                    error: e => {
                        return `${e.response.data.detail}`;
                    },
                });
            },
            logout: () => {
                LogoutUserApi();
                set({});
            }
        }),
        {
            name: 'user-storage', // уникальное имя для хранилища
            storage: createJSONStorage(() => sessionStorage), // использование sessionStorage
        }
    )
);
