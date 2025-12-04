using TicTacToeACA.Models;
using TicTacToeACA.Models.Board;
using TicTacToeACA.Players;
using TicTacToeACA.Rendering;
using TicTacToeACA.States;

namespace TicTacToeACA.Core;

public class GameEngine
{
    private IBoard _board;
    private IRenderer _renderer;
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private GameState _currentState;

    private bool _running;
    private string _lastResult;
    private int _totalGames = 0;
    private int _winsPlayer1 = 0;
    private int _winsPlayer2 = 0;
    private int _ties = 0;

    public GameEngine()
    {
        _renderer = new ConsoleRenderer();
        _board = new Board(_renderer);
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
                    HandleMenuSelection(selectedOption);
                    return;
            }
        }
    }

    private void HandleMenuSelection(int selectedOption)
    {
        switch (selectedOption)
        {
            case 0:
                SetupPvP();
                _currentState = new PlayingState();
                break;
            case 1:
                SetupPvAi();
                _currentState = new PlayingState();
                break;
            case 2:
                ViewStatitics();
                break;
            case 3:
                ResetStatitics();
                break;
            case 4:
                ShowInstructions();
                break;
            case 5:
                Exit();
                break;
        }
    }

    private void SetupPvP()
    {
        _renderer?.Clear();
        Console.WriteLine("Enter name of Player 1: ");
        string name = Console.ReadLine();
        Console.WriteLine("Enter name of Player 2: ");
        string name2 = Console.ReadLine();

        _player1 = new HumanPlayer(string.IsNullOrWhiteSpace(name) ? "Player 1" : name, 'X');
        _player2 = new HumanPlayer(string.IsNullOrWhiteSpace(name2) ? "Player 2" : name2, 'O');
        _currentPlayer = _player1;
    }

    private void SetupPvAi()
    {
        _renderer?.Clear();
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Difficulty[] values = Enum.GetValues<Difficulty>();
        int selectedOption = -1;
        while (true)
        {
            _renderer?.Clear();
            Console.WriteLine("\nSelected AI Difficulty:\n");
            for (int i = 0; i < values.Length; i++)
            {
                string optionName = values[i].ToString();
                if (i==selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($" ► {i + 1}. {optionName} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {i + 1}. {optionName}");
                }
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = (selectedOption - 1 + values.Length) % values.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = (selectedOption + 1) % values.Length;
                    break;
                case ConsoleKey.Enter:
                    Difficulty chosenDifficulty = values[selectedOption];
                    _player1 = new HumanPlayer(string.IsNullOrWhiteSpace(name) ? "Player" : name, 'X');
                    _player2 = new AiPlayer("AI", 'O', chosenDifficulty);
                    _currentPlayer = _player2;
                    return;
            }
        }
    }

    
    public void PlayGame()
    {
        _board.Reset();

        while (true)
        {
            _renderer?.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{_player1.Name} ({_player1.Symbol}) vs {_player2.Name} ({_player2.Symbol})");
            Console.ResetColor();
            
            int highlightPos = -1;
            
            if (_currentPlayer is HumanPlayer humanPlayer)
            {
                highlightPos = humanPlayer.GetSelectedPosition();
            }

            _board.Display(highlightPos);

            if (_currentPlayer is HumanPlayer human)
            {
                Console.WriteLine($"\nUse Arrow Keys ↑↓←→ to navigate, Enter to place {_currentPlayer.Symbol}");
                Console.WriteLine($"Selected Position: {(human.GetSelectedPosition() + 1)}");
            }

            int move = _currentPlayer.GetMove(_board);
            if (move == -1)
            {
                continue;
            }

            _board.PlaceSymbol(move, _currentPlayer.Symbol);

            if (_board.CheckWin(_currentPlayer.Symbol))
            {
                _lastResult = $"{_currentPlayer.Name} wins!";
                _currentState = new GameOverState();
                _totalGames++;
                if(_currentPlayer == _player1)
                {
                    _winsPlayer1++;
                } else
                {
                    _winsPlayer2++;
                }
                    return;
            }

            if (_board.IsFull())
            {
                _lastResult = ":) Tie Game!";
                _currentState = new GameOverState();
                _totalGames++;
                _ties++;
                return;
            }

            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        }
    }

    private void ViewStatitics()
    {
        _renderer?.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║           GAME STATITICS             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine($"Total games: {_totalGames}");
        Console.WriteLine($"Player 1 wins: {_winsPlayer1}");
        Console.WriteLine($"Player 2 wins: {_winsPlayer2}");
        Console.WriteLine($"Ties: {_ties}");

        Console.ResetColor();
        Console.ReadKey();

    }

    private void ResetStatitics()
    {
        _renderer?.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════ ═══════════════════════════╗");
        Console.WriteLine("║          RESET STATITICS             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine("Statistic is reset!");

        _totalGames = 0;
        _winsPlayer1 = 0;
        _winsPlayer2 = 0;
        _ties = 0;

        Console.ResetColor();
        Console.ReadKey();
    }
    
    private void ShowInstructions()
    {
        _renderer?.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔════════════════════════════════════╗");
        Console.WriteLine("║         HOW TO PLAY                ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine("\n📋 Game Rules:");
        Console.WriteLine("   • Players take turns placing their symbol (X or O)");
        Console.WriteLine("   • Get three in a row to win!");
        Console.WriteLine("   • Row can be horizontal, vertical, or diagonal\n");

        Console.WriteLine("⌨️  Keyboard Controls:");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("   • Arrow Keys (↑↓←→) - Navigate board/menu");
        Console.WriteLine("   • Enter - Confirm selection");
        Console.ResetColor();

        Console.WriteLine("\n🎯 Board Positions:");
        Console.WriteLine("   1 | 2 | 3");
        Console.WriteLine("   ---------");
        Console.WriteLine("   4 | 5 | 6");
        Console.WriteLine("   ---------");
        Console.WriteLine("   7 | 8 | 9");

        Console.WriteLine("\n💡 Tips:");
        Console.WriteLine("   • Center position (5) is strategically strong");
        Console.WriteLine("   • Corner positions are good for offense/defense");
        Console.WriteLine("   • Watch for opponent's two-in-a-row!");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nPress any key to return to menu...");
        Console.ResetColor();
        Console.ReadKey();
    }

    private void Exit()
    {
        _running = false;
        _renderer?.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nThanks for playing! Goodbye! \n");
        Console.ResetColor();
    }

    public void ShowGameOver()
    {
        _renderer?.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n{_player1.Name} (X) vs {_player2.Name} (O)");
        Console.ResetColor();

        _board.Display(-1);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"╔════════════════════════════════════╗");
        Console.WriteLine($"║  🎮  {_lastResult,-28}  ║");
        Console.WriteLine($"╚════════════════════════════════════╝\n");
        Console.ResetColor();

        Console.ReadKey(true);
        _currentState = new MenuState();
    }
}
