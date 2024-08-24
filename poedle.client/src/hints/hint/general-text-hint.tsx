import { ListHint } from "./hint.ts";

interface GeneralTextHintProps {
    readonly hint: ListHint
    readonly hintStyleId: string
    readonly styleClassName: string
}

export function GeneralTextHint({ hint, hintStyleId, styleClassName }: GeneralTextHintProps) {
    let i = 0;
    return (
        <div>
            {hint.hint.map((x: string) =>
                <p id={hintStyleId} className={styleClassName} key={i++}>{x}</p>
            )}
        </div>
    );
}
