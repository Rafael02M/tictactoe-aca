namespace TicTacToeACA;

public class GameEngine
{
    private IBoard _board;
    private IRenderer _renderer;
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private GameState _currentState;

    private bool _running;

    public GameEngine()
    {
        // _renderer =new
        _board = new Board();
        _currentState = new MenuState();
    }

    public void Run()
    {
        _running = true;
        while (_running)
        {
            _currentState.Handle(this);
        }
    }

    public void ShowMenu()
    {
        string[] menuOptions =
        {
            "Player vs Player",
            "Player vs AI",
            "View Statistics",
            "Reset Statistics",
            "How To Play",
            "Exit"
        };
        int selectedOption = 0;

        while (true)
        {
            _renderer?.Clear();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║       TIC TAC TOE GAME             ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n Use ↑↓ Arrow Keys to navigate, Enter to select\n");

            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($" ► {i + 1}. {menuOptions[i]} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {i + 1}. {menuOptions[i]}");
                }
            }

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = (selectedOption + 1) % menuOptions.Length;
                    break;
                case ConsoleKey.Enter:
                    return;
                    break;
            }
        }
    }
}