import { HintList } from "../types/hints-type-def";

interface StatHintAreaProps {
    readonly statModHint: HintList
}

export function StatHintArea({ statModHint }: StatHintAreaProps) {
    let i = 0;
    return (
        <div>
            <p><span className="cl-span-heading">Stats</span>:</p>
            {statModHint.hint.map((x: string) =>
                <p key={i++}>{x}</p>
            )}
        </div>
    );
}