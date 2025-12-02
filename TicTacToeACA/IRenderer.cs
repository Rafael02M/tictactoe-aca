namespace TicTacToeACA;

public interface IRenderer
{
    void Clear();
    void SetCursor(Coord position);
    void NewLine();
    void DrawText(string text, TextStyle style = TextStyle.Normal);
    void DrawTextAt(string text, Coord position, TextStyle style = TextStyle.Normal);
    void DrawCell(char symbol, CellStyle cellStyle);
    void DrawCellAt(char symbol, Coord position, CellStyle cellStyle);
}