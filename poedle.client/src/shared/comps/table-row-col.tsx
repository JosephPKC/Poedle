import "../styles/table-row-col.css";

interface TableRowColProps {
    customClassName: string,
    text: string | number
}

export function TableRowCol({ customClassName, text }: TableRowColProps) {
    return (
        <>
            <td className={customClassName + " txt-fontin table-row-col"}>{text}</td>
        </>
    );
}
