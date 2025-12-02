namespace TicTacToeACA;

public class MenuState : GameState
{
    public override void Handle(GameEngine engine)
    {
        engine.ShowMenu();
    }
}