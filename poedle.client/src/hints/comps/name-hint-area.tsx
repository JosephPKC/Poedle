import { Hint, NullableHint } from "../types/hints-type-def";

interface NameHintAreaProps {
    readonly nameHint: Hint
    readonly secondaryNameHint: NullableHint
}

export function NameHintArea({ nameHint, secondaryNameHint }: NameHintAreaProps) {
    return (
        <div>
            <p><span className="cl-span-heading">Name</span>: {nameHint.hint}{secondaryNameHint == null ? "" : " , " + secondaryNameHint.hint}</p>
        </div>
    );
}