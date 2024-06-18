import {Link, useNavigate} from "react-router-dom";
import {Button} from "@/components/ui/button.tsx";
import {useEffect, useState} from "react";
import {CreateAccount} from "@/pages/CreateAccount.tsx";
import toast from "react-hot-toast";
import {Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {usePersonStore} from "@/entities/User/userStore.ts";
import {useAccountStore} from "@/entities/Account/AccountStore.ts";
import {convertApi} from "@/shared/convert";

export const Home = () => {

    const navigate = useNavigate();
    const user = usePersonStore(state => state.user);
    const logout = usePersonStore(state => state.logout);
    const accounts = useAccountStore(state => state.accounts);
    const setAccounts = useAccountStore(state => state.setAccounts);
    const updateAccount = useAccountStore(state => state.updateAccount);

    const [isAddedAccount, setIsAddedAccount] = useState(false);

    const addedAccount = () => {
        setIsAddedAccount(false);
    }

    const convertCurrency = (accountId: number) => {
        const promise = convertApi(accountId);
        toast.promise(promise, {
            loading: "Загрузка ...",
            success: resp => {
                if (resp.status >= 400) {
                    throw new Error(`Statues code ${resp.status}`);
                }

                updateAccount(accountId, resp.data.isFirstMain, resp.data.balance);
                return "Перевод в другую валюту успешно произведён";
            },
            error: e => {
                return `${e.response.data.detail}`;
            },
        });
    }

    const logoutHandler = () => {
        logout();
        navigate('/login')
        const user = sessionStorage.getItem('user-storage');

        if (user) {
            sessionStorage.removeItem('user-storage');
        }
    };

    useEffect(() => {
        if (user !== null) {
            setAccounts(user.id);
        }
    }, [user]);

    return (
        <>
            <h1 style={{fontSize: '20px'}}>Главная</h1>
            {
                user === null ? (
                    <div className='mt-1'>
                        <Button className='m-1'><Link to={'/login'}>Вход</Link></Button>
                        <Button><Link to={'/register'}>Регистрация</Link></Button>
                    </div>) : (
                    <div>
                        <div style={{marginBottom: '10px'}}>
                            <span>Логин: {user.userName}</span>
                            <br/>
                            {
                                isAddedAccount === false ?
                                    <>
                                        <Button className='m-1' disabled={accounts.length === 5 ? true : false}
                                                onClick={() => setIsAddedAccount(true)}>Добавить счет</Button>
                                        <Button className='m-1' onClick={() => navigate('/currency')}>Добавить валюту</Button>
                                        <Button className='m-1' onClick={() => navigate('/coefficients')}>Коэффициенты
                                            для перевода валюты</Button>
                                        <Button className='m-1' onClick={() => navigate('/transfer')}>
                                            Перевод на другой счет
                                        </Button>
                                        <Button className='m-1' onClick={() => navigate('/transferHistory')}>
                                            История
                                        </Button>
                                        <Button onClick={logoutHandler}>Выход</Button>
                                    </>
                                    : <CreateAccount addedAccount={addedAccount}/>
                            }
                        </div>

                        <Table>
                            <TableCaption>Аккаунты</TableCaption>
                            <TableCaption className='italic'>_ - основная валюта</TableCaption>
                            <TableHeader>
                                <TableRow>
                                    <TableHead className='text-center'>Название</TableHead>
                                    <TableHead className='text-center'>Первая валюта</TableHead>
                                    <TableHead className='text-center'>Вторая валюта</TableHead>
                                    <TableHead className='text-center'>Баланс</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {
                                    accounts.map(x => (
                                        <TableRow key={x.id}>
                                            <TableCell className='m-1'>{x.name}</TableCell>
                                            <TableCell className='m-1'
                                                       style={{textDecoration: x.isFirstCurrencyMain ? 'underline' : ''}}>{x.firstCurrencyName}</TableCell>
                                            <TableCell className='m-1'
                                                       style={{textDecoration: x.isFirstCurrencyMain ? '' : 'underline'}}>{x.secondCurrencyName}</TableCell>
                                            <TableCell>{x.balance}</TableCell>
                                            <TableCell><Button onClick={() => convertCurrency(x.id)}>Перевести в другую
                                                валюту</Button></TableCell>
                                        </TableRow>
                                    ))
                                }
                            </TableBody>
                        </Table>
                    </div>
                )
            }
        </>
    )
}