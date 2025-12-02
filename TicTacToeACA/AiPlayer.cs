namespace TicTacToeACA;

public class AiPlayer:Player
{
    private Random _random;
    
    public AiPlayer(string name, char symbol) : base(name, symbol)
    {
        _random = new Random();
    }

    public override int GetMove(IBoard board)
    {
        Console.WriteLine($"{Name} ({Symbol}) is thinking...");
        Thread.Sleep(1000);

        var move = GetRandomMove(board);
        return move;
    }

    private int GetRandomMove(IBoard board)
    {
        var available = board.GetAvailablePositions();
        int randomIndex = _random.Next(0, available.Length);
        return available[randomIndex];
    }
}