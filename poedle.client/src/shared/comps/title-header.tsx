interface TitleHeaderProps {
    gameGuessType: string
}

export function TitleHeader({ gameGuessType }: TitleHeaderProps) {
    return (
        <div className="div-txt-box">
            <h1 className="txt-outer">Can you guess the <span className="txt-important">{gameGuessType}</span>?</h1>
        </div>
    );
}