namespace TicTacToeACA;

public abstract class Player
{
    public string Name { get; protected set; }
    public char Symbol { get; protected set; }

    protected Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public abstract int GetMove(IBoard board);
}