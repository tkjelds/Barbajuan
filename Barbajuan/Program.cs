internal class Program
{
    private static void Main(string[] args)
    {
        List<List<Iplayer>> scoreBoards = new List<List<Iplayer>>();
        List<(String, List<int>)> playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
        // using (var writer = new StreamWriter(@".\Documentation\AverageNumberOfTurns.csv"))
        // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        for (int i = 0; i < 1000; i++)
        {
            var players = new List<Iplayer>(){
                new StackingMovePlayer("bot 1"),
                new StackingMovePlayer("bot 2"),
                new StackingMovePlayer("bot 3"),
                new StackingMovePlayer("bot 4")
            };
            var gameState = new GameState(players);
            var nmbrOfTurns = gameState.runReturnNumberOfTurns();
            //csv.WriteRecord(nmbrOfTurns);
            
        }
       // }
        // Parallel.For(1, 100000, number =>
        // {
        //     var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new StackingMovePlayer("bot 4")
        //     };
        //     var gameState = new GameState(players);
        //     var scoreBoard = gameState.runReturnScoreBoard();
        //     scoreBoards.Add(scoreBoard);
        // });
        // foreach (var scoreboard in scoreBoards)
        // {

        //     for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
        //     {
        //         var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].getName());
        //         entry.Item2[placementIndex]++;
        //     }
        // }

        // foreach (var player in playerPlacements)
        // {
        //     Console.WriteLine(player.Item1);
        //     for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
        //     {
        //         var placement = placementIndex + 1;
        //         Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
        //     }
        // }

        // Console.Write(scoreBoards.Count());
        //var scoreBoards = gameState.runReturnScoreBoard();
    }
}