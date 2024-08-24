import { NullableSingleHint, SingleHint } from "./hint.ts";

interface NameHintProps {
    readonly hint: SingleHint
    readonly secondaryHint: NullableSingleHint
    readonly hintStyleId: string
    readonly secondaryHintStyleId: string
    readonly styleClassName: string
    readonly fadeStyleClassName: string
}

export function NameHint({ hint, secondaryHint, hintStyleId, secondaryHintStyleId, styleClassName, fadeStyleClassName }: NameHintProps) {
    return (
        <div>
            <div className={fadeStyleClassName} />
            <p id={hintStyleId} className={styleClassName}>{hint.hint}</p>
            {secondaryHint == null ? <></> :
                <p id={secondaryHintStyleId}  className={styleClassName}>{secondaryHint.hint}</p>
            }
            <div className={fadeStyleClassName} />
        </div>
    );
}