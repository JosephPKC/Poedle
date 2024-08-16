import { SingleHint, NullableSingleHint } from "../types/hints-type-def";

interface NameHintAreaProps {
    readonly nameHint: SingleHint
    readonly secondaryNameHint: NullableSingleHint
}

export function NameHintArea({ nameHint, secondaryNameHint }: NameHintAreaProps) {
    return (
        <div>
            <p><span className="cl-span-heading">Name</span>: {nameHint.hint}{secondaryNameHint == null ? "" : " , " + secondaryNameHint.hint}</p>
        </div>
    );
}