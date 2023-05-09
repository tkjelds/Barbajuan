internal partial class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            RunTest();
            return;
        }
        if (args.Length == 3)
        {
            try
            {
                RunTest(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
                return;
            }
            catch (System.Exception)
            {

                throw;

            }
        }
        throw new Exception("incompatible amount of argurments");
    }

    private static void RunTest(int determinizations = 25, int iterations = 50, int games = 100)
    {
        List<List<Iplayer>> scoreBoards = new List<List<Iplayer>>();
        List<(String, List<int>)> playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("MCTS player", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("Random Player 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("Random Player 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("Random Player 4", new List<int>() { 0, 0, 0, 0 }));
        var i = 0;

        var records = new List<Timeline>();

        while (i < games)
        {
            var players = new List<Iplayer>(){
                new MCTS_Player("MCTS player",determinizations,iterations),
                new RandomStackingPlayer("Random Player 2"),
                new RandomStackingPlayer("Random Player 3"),
                new RandomStackingPlayer("Random Player 4")
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.RunReturnScoreBoard();


            scoreBoards.Add(scoreBoard);
            i++;
            Console.WriteLine("Done with game number: " + i + " Out of " + games);
        }

        PrintScoreBoard(scoreBoards, playerPlacements);
    }

    private static void PrintScoreBoard(List<List<Iplayer>> scoreBoards, List<(string, List<int>)> playerPlacements)
    {
        foreach (var scoreboard in scoreBoards)
        {
            for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
            {

                var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].GetName());

                entry.Item2[placementIndex]++;
            }
        }

        foreach (var player in playerPlacements)
        {
            var playername = player.Item1;
            Console.WriteLine(player.Item1);

            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                switch (placement)
                {
                    case(1):
                    Console.WriteLine("1st place : " +  player.Item2[placementIndex]);
                    break;
                    case(2):
                    Console.WriteLine("2nd place : " +  player.Item2[placementIndex]);
                    break;
                    case(3):
                    Console.WriteLine("3rd place : " +  player.Item2[placementIndex]);
                    break;
                    case(4):
                    Console.WriteLine("4th place : " +  player.Item2[placementIndex]);
                    break;
                    default:
                    Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                    break;
                }
            }
        }
    }
}
