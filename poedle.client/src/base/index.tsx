// Ext Modules
import { BrowserRouter, Routes, Route } from "react-router-dom";
// Components
import { ByAttr } from "./by-attr.tsx";
import { NavBar } from "../shared/components/nav-bar.tsx";

export function Index() {
    return (
        <>
            <BrowserRouter>
                <NavBar />
                <Routes>
                    <Route path="/by-attr" element={<ByAttr />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}