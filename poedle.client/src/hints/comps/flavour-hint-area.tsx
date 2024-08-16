import { ListHint } from "../types/hints-type-def";
import "../styles/flavour-hint-area.css";

interface FlavourHintAreaProps {
    readonly flavourHint: ListHint
}

export function FlavourHintArea({ flavourHint }: FlavourHintAreaProps) {
    let i = 0;
    return (
        <div id="id-div-flavour-hint">
            {flavourHint.hint.map((x: string) =>
                <p key={i++}><span className="cl-span-flavour">{x}</span></p>
            )}
        </div>
    );
}