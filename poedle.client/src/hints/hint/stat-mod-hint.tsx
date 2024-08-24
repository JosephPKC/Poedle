import { StatHint } from "./hint.ts";

interface StatModHintProps {
    readonly hint: StatHint
    readonly imgStyleClassName: string
    readonly hintStyleClassName: string
    readonly divStyleClassName: string
}

export function StatModHint({ hint, imgStyleClassName, hintStyleClassName, divStyleClassName }: StatModHintProps) {
    let i = 0, j = 0;
    return (
        <div>
            <div className={divStyleClassName}>
                {hint.hint.slice(0, hint.nbrImplicits).map((x: string) =>
                    <StatModText key={i++} statModText={x} imgStyleClassName={imgStyleClassName} styleClassName={hintStyleClassName} />
                )}
            </div>
            <div className="fade-border" />
            <div className={divStyleClassName}>
                {hint.hint.slice(hint.nbrImplicits).map((x: string) =>
                    <StatModText key={j++} statModText={x} imgStyleClassName={imgStyleClassName} styleClassName={hintStyleClassName} />
                )}
            </div>
            <div className="fade-border" />
        </div>
    );
}

interface StatModTextProps {
    readonly statModText: string
    readonly imgStyleClassName: string
    readonly styleClassName: string
}

function StatModText({ statModText, imgStyleClassName, styleClassName }: StatModTextProps) {
    const statMod = statModText == "_" ?
        <img className={imgStyleClassName} src="images/Veiled_Mod.gif" /> :
        <p className={styleClassName}>{statModText}</p>;

    return (
        <>{statMod}</>
    );
}
