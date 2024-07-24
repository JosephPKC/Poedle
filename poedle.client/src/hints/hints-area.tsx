// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";

interface HintsAreaProps {
    hints: string
}

export function HintsArea({ hints }: HintsAreaProps) {
    const hintsArea = (hints == null) ? (<DefaultLoadingText />) : (
        <>
            <div className="cl-div-hint">
                <p><span className="cl-span-heading">Hint</span>: {hints}</p>
            </div>
        </>
    );

    return (
        <div>{hintsArea}</div>
    );
}