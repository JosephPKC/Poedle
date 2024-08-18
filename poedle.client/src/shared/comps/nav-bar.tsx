import { NavLink } from "react-router-dom";

import "../styles/nav-bar.css";

export function NavBar() {
    return (
        <nav>
            <div>
                <NavLink className="txt-fontin" to="/skill-gems"><span>Skill Gems</span></NavLink>
                <NavLink className="txt-fontin" to="/unique-items"><span>Unique Items</span></NavLink>
            </div>
        </nav>
    );
}