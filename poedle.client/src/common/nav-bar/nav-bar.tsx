import { NavLink } from "react-router-dom";
import { NavItem } from "./nav-item";

import "./nav-bar.css";

interface NavBarProps {
    readonly navItems: NavItem[]
}

export function NavBar({ navItems }: NavBarProps) {
    let i: number = 0;
    return (
        <nav className="nav-bar">
            <div>
                {navItems.map((n: NavItem) =>
                    <NavLink key={i++} className={"nav-bar-item " + n.className} to={n.to}>{n.name}</NavLink>
                )}
            </div>
        </nav>
    );
}
