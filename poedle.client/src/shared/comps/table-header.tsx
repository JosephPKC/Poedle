import "../styles/table-header.css";

interface TableHeaderProps {
    headers: string[]
}

export function TableHeader({ headers }: TableHeaderProps) {
    return (
        <thead>
            <tr>
                {headers.map((h: string) => <th className="th-table-header" key={h}>{h}</th>)}
            </tr>
        </thead>
    );
}