interface TitleHeaderProps {
    gameGuessType: string
}

export function TitleHeader({ gameGuessType }: TitleHeaderProps) {
    return (
        <div>
            <h3>Can you guess the <span className="text-important">{gameGuessType}</span>?</h3>
        </div>
    );
}