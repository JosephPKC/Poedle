import { NavLink } from "react-router-dom";

export function NavBar() {
    return (
        <nav>
            <div>
                <NavLink to="/by-attr">By Attribute</NavLink>
            </div>
        </nav>
    );
}