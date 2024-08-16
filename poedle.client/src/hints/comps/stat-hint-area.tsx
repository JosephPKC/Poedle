import { StatHint } from "../types/hints-type-def";

interface StatHintAreaProps {
    readonly statModHint: StatHint
}

export function StatHintArea({ statModHint }: StatHintAreaProps) {
    let i = 0;
    return (
        <div>
            <p><span className="cl-span-heading">Stats</span>:</p>
            <div>
                {statModHint.hint.slice(0, statModHint.nbrImplicits).map((x: string) =>
                    <p key={i++} className="cl-p-implicit-stat">{x}</p>
                )}
            </div>
            <p>-------------------------------------------------------------</p>
            <div>
                {statModHint.hint.slice(statModHint.nbrImplicits).map((x: string) =>
                    <p key={i++} className="cl-p-explicit-stat">{x}</p>
                )}
            </div>
            <p>-------------------------------------------------------------</p>
        </div>
    );
}