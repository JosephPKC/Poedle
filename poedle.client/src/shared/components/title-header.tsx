interface TitleHeaderProps {
    gameGuessType: string
}

export function TitleHeader({ gameGuessType }: TitleHeaderProps) {
    return (
        <div>
            <h3>Can you guess the <span className="text-important">Unique Item</span> by...</h3>
            <h2 className="text-header-important">{gameGuessType}?</h2>
        </div>
    );
}