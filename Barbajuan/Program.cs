using System.Globalization;
using CsvHelper;

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
        if (args[3] == "t"){
            try
            {
                RunTimelineTest(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
                return;
            }
            catch (System.Exception)
            {

                throw;

            }
        }
        throw new Exception("incompatible amount of argurments");
    }

    private static void RunTimelineTest(int determinizations = 25, int iterations = 50, int games = 100){
        var i = 0;
        var records = new List<Timeline>();
        while(i<games){
            var temprecords = new List<Timeline>();
            var players = new List<Iplayer>(){
                new MCTS_Player("bot 1",determinizations,iterations),
                new RandomStackingPlayer("bot 2"),
                new RandomStackingPlayer("bot 3"), 
                new RandomStackingPlayer("bot 4")
            };
            var gameState = new GameState(players);
            var timeLine = gameState.RunReturnTimeline();
            var turnCount = 0; 
            
            foreach(var turn in timeLine) {
                if(turn.Item2 == "bot 1") {
                    turnCount++;
                    var cardColor = turn.Item3.First().cardColor.ToString();

                    //If number, put "Number", else the type.
                    string cardType;
                    if((int) turn.Item3.First().cardType < 10) {
                        cardType = "Number";
                    } else {
                        cardType = turn.Item3.First().cardType.ToString();
                    }

                    //If stacking move, concat stacking in front.
                    if(turn.Item3.Count() > 1) {
                        cardType = "STACKING" + cardType;
                    }
                     
                    temprecords.Add(new Timeline(turnCount, cardColor, cardType, 0));
                }
            }

            var turnCountDouble = Convert.ToDouble(turnCount);
            var quart = turnCountDouble / 4.0;

            foreach(var t in temprecords) {
                if(t.turn <= quart) {
                    t.quartile = 1;
                    
                } else if(t.turn <= (quart*2.0)) {
                    t.quartile = 2;
                    
                } else if(t.turn <= (quart*3.0)) {
                    t.quartile = 3;
                    
                } else if(t.turn <= (quart*4.0)) {
                    t.quartile = 4;
                   
                }
            }
            
            foreach(var t in temprecords) {
                records.Add(t);
            }


            i++;
            Console.WriteLine("Done with game number: " + i + " Out of " + games);
        }

        
        using (var writer = new StreamWriter(@".\Documentation\BachelorDefenseShowcase.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }

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
                    case (1):
                        Console.WriteLine("1st place : " + player.Item2[placementIndex]);
                        break;
                    case (2):
                        Console.WriteLine("2nd place : " + player.Item2[placementIndex]);
                        break;
                    case (3):
                        Console.WriteLine("3rd place : " + player.Item2[placementIndex]);
                        break;
                    case (4):
                        Console.WriteLine("4th place : " + player.Item2[placementIndex]);
                        break;
                    default:
                        Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                        break;
                }
            }
        }
    }
}
