import './App.css'
import {Login} from "@/pages/Login.tsx";
import {Register} from "@/pages/Register.tsx";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import {Home} from "@/pages/Home.tsx";
import {Coefficient} from "@/pages/Coefficient.tsx";
import {TransferMoney} from "@/pages/TransferMoney.tsx";
import {Toaster} from "react-hot-toast";
import {TransferHistory} from "@/pages/TransferHistory.tsx";

function App() {
    return (
        <BrowserRouter>
            <Toaster position={'top-right'}/>
            <Routes>
                <Route path={'/login'} element={<Login/>}/>
                <Route path={'/register'} element={<Register/>}/>
                <Route path={'/coefficients'} element={<Coefficient/>}/>
                <Route path={'/transferHistory'} element={<TransferHistory/>}/>
                <Route path={'/transfer'} element={<TransferMoney/>}/>
                <Route path={'/'} element={<Home/>}/>
            </Routes>
        </BrowserRouter>
    )
}

export default App
