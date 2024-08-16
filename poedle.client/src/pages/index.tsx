import { BrowserRouter, Routes, Route } from "react-router-dom";
import { UniqueItemsGame } from "./unique-items.tsx";
import { NavBar } from "../shared/comps/nav-bar.tsx";

import "../normalize.css";
import "./styles/index.css";

export function Index() {
    return (
        <div id="div-root">
            <BrowserRouter>
                <NavBar  />
                <Routes>
                    <Route path="/unique-items" element={<UniqueItemsGame />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}