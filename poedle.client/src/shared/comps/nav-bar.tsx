import { NavLink } from "react-router-dom";

export function NavBar() {
    return (
        <nav>
            <div id="nav-bar">
                <NavLink to="/unique-items">Unique Items</NavLink>
            </div>
        </nav>
    );
}