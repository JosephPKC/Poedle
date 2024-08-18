import { BrowserRouter, Routes, Route } from "react-router-dom";

import { NavBar } from "./shared/comps/nav-bar.tsx";
import { SkillGemsGame } from "./skill-gems-game.tsx";
import { UniqueItemsGame } from "./unique-items-game.tsx";

import "./global/normalize.css";
import "./global/base.css";
import "./index.css";

export function Index() {
    return (
        <div>
            <BrowserRouter>
                <NavBar  />
                <Routes>
                    <Route path="/skill-gems" element={<SkillGemsGame />} />
                    <Route path="/unique-items" element={<UniqueItemsGame />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}