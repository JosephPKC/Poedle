import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ByAttr } from "./by-attr.tsx";
import { NavBar } from "../shared/comps/nav-bar.tsx";

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