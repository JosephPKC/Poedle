interface PageTitleProps {
    readonly gameType: string
}

export function PageTitle({ gameType }: PageTitleProps) {
    return (
        <div>
            <h1 className="txt-center txt-outer txt-wrap">Can you guess the <span className="txt-strong">{gameType}</span>?</h1>
        </div>
    );
}
