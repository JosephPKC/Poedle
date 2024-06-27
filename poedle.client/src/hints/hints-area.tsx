// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";
// Utils
import { HintList } from "../shared/types/type-defs.ts";

interface HintsAreaProps {
    hints: HintList
}

export function HintsArea({ hints }: HintsAreaProps) {
    const hintsArea = (hints == null) ? (<DefaultLoadingText />) : (
        <>
            {hints.map((h: string) => <div key={h}><p>{h}</p></div>)}
        </>
    );

    return (
        <div>{hintsArea}</div>
    );
}