namespace TicTacToeACA;

public interface IBoard
{
    bool IsPositionEmpty(int position);
    int[] GetAvailablePositions();
}

public class Board:IBoard
{
    private char[] _cells;

    private int[,] _winPatterns = new int[,]
    {
        { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, //Rows
        { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, //Columns
        { 0, 4, 8 }, { 2, 4, 6 } //Diagonal
    };

    public const int Width = 3;
    public const int Height = 3;

    public static readonly Coord Bounds = new Coord(Width, Height);

    public int BoardSize => Width * Height;

    public Board()
    {
        _cells = new char[BoardSize];
        Reset();
    }

    private void Reset()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            _cells[i] = ' ';
        }
    }

    public bool PlaceSymbol(int position, char symbol)
    {
        if (InBounds(position))
        {
            _cells[position] = symbol;
            return true;
        }

        return false;
    }

    public void ClearPosition(int position)
    {
        if (InBounds(position))
        {
            _cells[position] = ' ';
        }
    }

    public bool IsPositionEmpty(int position)
    {
        return InBounds(position) && _cells[position] == ' ';
    }

    private bool InBounds(int position) => position >= 0 && position < BoardSize;

    public bool IsFull()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i] == ' ')
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckWin(char symbol)
    {
        for (int i = 0; i < _winPatterns.GetLength(0); i++)
        {
            if (_cells[_winPatterns[i, 0]] == symbol &&
                _cells[_winPatterns[i, 1]] == symbol &&
                _cells[_winPatterns[i, 2]] == symbol)
            {
                return true;
            }
        }

        return false;
    }

    public int[] GetAvailablePositions()
    {
        int[] available = new int[BoardSize];// {0, 1, 2, 3}
        
        int pointer = 0;
        for (int i = 0; i < BoardSize; i++)
        {
            if (_cells[i]== ' ')
            {
                available[pointer++] = i;
            }
        }

        Array.Resize(ref available, pointer);
        return available;
    }
}