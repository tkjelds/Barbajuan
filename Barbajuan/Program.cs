internal class Program
{
    private static void Main(string[] args)
    {
        var players = new List<Player>(){
            new Player(new List<Card>()),
            new Player(new List<Card>()),
            new Player(new List<Card>()),
            new Player(new List<Card>())
        };
        var gameState = new GameState(players);
        gameState.run();
    }
}