import { NavLink } from "react-router-dom";

export function NavBar() {
    return (
        <nav>
            <div>
                <NavLink to="/unique-items">Unique Items</NavLink>
            </div>
        </nav>
    );
}