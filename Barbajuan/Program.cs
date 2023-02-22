internal class Program
{
    private static void Main(string[] args)
    {
        var players = new List<Iplayer>(){
            new StackingMovePlayer("bot 1"),
            new StackingMovePlayer("bot 2"),
            new StackingMovePlayer("bot 3"),
            new StackingMovePlayer("bot 4")
        };
        var gameState = new GameState(players);
        gameState.run();
    }
}