import { BrowserRouter, Routes, Route } from "react-router-dom";
import { NavBar } from "../../common/nav-bar/nav-bar.tsx";
import { NavItem } from "../../common/nav-bar/nav-item.ts";
import { SkillGemsGame } from "../skill-gems/skill-gems-game.tsx";
import { UniqueItemsGame } from "../unique-items/unique-items-game.tsx";

import "../normalize.css";
import "../base.css";
import "./index.css";

export function Index() {
    const SkillGemNav: NavItem = {
        name: "Skill Gems",
        to: "/skill-gems",
        className: "",
        compTo: <SkillGemsGame />
    };

    const UniqueItemNav: NavItem = {
        name: "Unique Items",
        to: "/unique-items",
        className: "",
        compTo: <UniqueItemsGame />
    };

    const navItems: NavItem[] = [SkillGemNav, UniqueItemNav];
    let i: number = 0;
    return (
        <div id="div-root">
            <BrowserRouter>
                <NavBar navItems={navItems} />
                <Routes>
                    {navItems.map((n: NavItem) =>
                        <Route key={i++} path={n.to} element={n.compTo} />
                    )}
                </Routes>
            </BrowserRouter>
        </div>
    );
}