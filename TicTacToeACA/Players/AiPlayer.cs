using TicTacToeACA.Models;
using TicTacToeACA.Models.Board;

namespace TicTacToeACA.Players;

public class AiPlayer : Player
{
    private Random _random;
    private Difficulty _difficulty;

    public AiPlayer(string name, char symbol, Difficulty difficulty) : base(name, symbol)
    {
        _random = new Random();
        _difficulty = difficulty;
    }

    public override int GetMove(IBoard board)
    {
        Console.WriteLine($"{Name} ({Symbol}) is thinking...");
        Thread.Sleep(1000);

        switch (_difficulty)
        {
            case Difficulty.Easy:
                return GetRandomMove(board);
            case Difficulty.Medium:
                return GetMediumMove(board);
            case Difficulty.Hard:
                return GetSmartMove(board);
        }

        return -1;
    }

    private int GetRandomMove(IBoard board)
    {
        var available = board.GetAvailablePositions();
        int randomIndex = _random.Next(0, available.Length);
        return available[randomIndex];
    }

    private int GetMediumMove(IBoard board)
    {
        int winMove = FindWinningMove(board, Symbol);
        if (winMove != -1) return winMove;

        char opponentSymbol = Symbol == 'X' ? 'O' : 'X';
        int blockMove = FindWinningMove(board, opponentSymbol);
        if(blockMove != -1) return blockMove;

        var available = board.GetAvailablePositions();
        return available[_random.Next(0, available.Length)];

    }

    private int GetSmartMove(IBoard board)
    {
        int winMove = FindWinningMove(board, Symbol);
        if (winMove != -1) return winMove;

        char opponentSymbol = Symbol == 'X' ? 'O' : 'X';
        int blockMove = FindWinningMove(board, opponentSymbol);
        if (blockMove != -1) return blockMove;

        if (board.IsPositionEmpty(4))
        {
            return 4;
        }

        int[] corners = { 0, 2, 6, 8 };
        int[] tempCorners = new int[4];
        int cornerCount = 0;
        for (int i = 0; i < corners.Length; i++)
        {
            if (board.IsPositionEmpty(corners[i]))
            {
                tempCorners[cornerCount++] = corners[i];
            }
        }

        if (cornerCount > 0)
        {
            return tempCorners[_random.Next(0, cornerCount)];
        }

        var available = board.GetAvailablePositions();
        return available[_random.Next(0, available.Length)];
    }

    private int FindWinningMove(IBoard board, char symbol)
    {
        for (int i = 0; i < Board.Width * Board.Height; i++)
        {
            if (board.IsPositionEmpty(i))
            {
                board.PlaceSymbol(i,symbol);
                bool isWin = board.CheckWin(symbol);
                board.ClearPosition(i);
                if (isWin)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
