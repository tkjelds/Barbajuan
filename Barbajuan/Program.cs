internal class Program
{
    private static void Main(string[] args)
    {
        var players = new List<Iplayer>(){
            new Player(new List<Card>(), "bot 1"),
            new Player(new List<Card>(), "bot 2"),
            new Player(new List<Card>(), "bot 3"),
            new Player(new List<Card>(), "bot 4")
        };
        var gameState = new GameState(players);
        gameState.run();
    }
}