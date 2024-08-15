import { Hint } from "../types/hints-type-def";
import "../styles/flavour-hint-area.css";

interface FlavourHintAreaProps {
    readonly flavourHint: Hint
}

export function FlavourHintArea({ flavourHint }: FlavourHintAreaProps) {
    return (
        <div>
            <p><span className="cl-span-flavour">{flavourHint.hint}</span></p>
        </div>
    );
}