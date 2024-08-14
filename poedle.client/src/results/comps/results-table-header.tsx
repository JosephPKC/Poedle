interface ResultsTableHeaderProps {
    headers: string[]
}

export function ResultsTableHeader({ headers }: ResultsTableHeaderProps) {
    return (
        <thead>
            <tr>
                {headers.map((h: string) => <th key={h}>{h}</th>)}
            </tr>
        </thead>
    );
}