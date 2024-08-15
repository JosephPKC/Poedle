import { BrowserRouter, Routes, Route } from "react-router-dom";
import { UniqueItemsGame } from "./unique-items.tsx";
import { NavBar } from "../shared/comps/nav-bar.tsx";

export function Index() {
    return (
        <>
            <BrowserRouter>
                <NavBar />
                <Routes>
                    <Route path="/unique-items" element={<UniqueItemsGame />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}