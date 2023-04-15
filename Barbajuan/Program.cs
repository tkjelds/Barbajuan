using System.Globalization;
using CsvHelper;

internal class Program
{
    private static void Main(string[] args)
    {

        // var i = 0;
        
        // var records = new List<Timeline>();

        // while(i<10){
        //     var players = new List<Iplayer>(){
        //         new MCTS_Player("bot 1", 50, 250),
        //         new RandomStackingPlayer("bot 2"),
        //         new RandomStackingPlayer("bot 3"),
        //         new RandomStackingPlayer("bot 4")          
        //     };
        //     var gameState = new GameState(players);
        //     var timeLine = gameState.runReturnTimeline();
        //     //int turnCount = 0;
        //     // foreach(var turn in timeLine) {
        //     //     if(turn.Item2 == "bot 1") {
        //     //         turnCount++;
        //     //         foreach(var card in turn.Item3) {
        //     //             records.Add(new Timeline(turnCount, card));
        //     //         }
        //     //     }
        //     // }
        //     // scoreBoards.Add(scoreBoard);
        //     i++;
        //     if(i % 1 == 100 ) {Console.WriteLine("Done with game number: " + i);};  
        //     //Console.WriteLine("Done with game number: " + i);
        // }



        // using (var writer = new StreamWriter(@".\Documentation\TimeLine.csv"))
        // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        // {
        //     csv.WriteRecords(records);
        //}

        List<List<Iplayer>> scoreBoards = new List<List<Iplayer>>();
        // var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",210,10000000)
        //     };
        // var gameState = new GameState(players);
        // gameState.runReturnNumberOfTurns();
        List<(String, List<int>)> playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
        Console.WriteLine("Current Test: 10,1000");
        //Console.WriteLine("Seat 1");
        for (int i = 0; i < 1000; i++){ 
            var players = new List<Iplayer>(){
                
                new Cheating_UCT_Player("bot 1",10,1000),
                new RandomStackingPlayer("bot 2"),
                new RandomStackingPlayer("bot 3"),
                new RandomStackingPlayer("bot 4")          
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.runReturnScoreBoard();
            scoreBoards.Add(scoreBoard);
            //if(i % 10 == 0 ) Console.WriteLine("Done with game number: " + i);            
        }

        foreach (var scoreboard in scoreBoards)
        {
            for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
            {
                
                var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].getName());
                
                entry.Item2[placementIndex]++;
            }
        }

        foreach (var player in playerPlacements)
        {
            var playername = player.Item1;
            //Console.WriteLine(player.Item1);
            using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                }
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                Console.Write(player.Item2[placementIndex]+",");
                //Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                

                using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField(numberOfPlacements);
                    csv.NextRecord();
                }
            } 
            Console.WriteLine();
        }
        scoreBoards = new List<List<Iplayer>>();
        // var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",210,10000000)
        //     };
        // var gameState = new GameState(players);
        // gameState.runReturnNumberOfTurns();
        playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
        Console.WriteLine();
        Console.WriteLine();
        //Console.WriteLine("Seat 2");
        for (int i = 0; i < 1000; i++){ 
            var players = new List<Iplayer>(){
                
                new RandomStackingPlayer("bot 1"),
                new Cheating_UCT_Player("bot 2",10,1000),
                new RandomStackingPlayer("bot 3"),
                new RandomStackingPlayer("bot 4")          
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.runReturnScoreBoard();
            scoreBoards.Add(scoreBoard);
            //if(i % 10 == 0 ) Console.WriteLine("Done with game number: " + i);            
        }

        foreach (var scoreboard in scoreBoards)
        {
            for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
            {
                
                var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].getName());
                
                entry.Item2[placementIndex]++;
            }
        }

        foreach (var player in playerPlacements)
        {
            var playername = player.Item1;
            ////Console.WriteLine(player.Item1);
            using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                }
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                Console.Write(player.Item2[placementIndex]+",");
                //Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                

                using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField(numberOfPlacements);
                    csv.NextRecord();
                }
            } 
            Console.WriteLine();
        }
        scoreBoards = new List<List<Iplayer>>();
        // var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",210,10000000)
        //     };
        // var gameState = new GameState(players);
        // gameState.runReturnNumberOfTurns();
        playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
Console.WriteLine();
Console.WriteLine();
        //Console.WriteLine("Seat 3");
        for (int i = 0; i < 1000; i++){ 
            var players = new List<Iplayer>(){
                
                new RandomStackingPlayer("bot 1"),
                new RandomStackingPlayer("bot 2"),
                new Cheating_UCT_Player("bot 3",10,1000),
                new RandomStackingPlayer("bot 4")          
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.runReturnScoreBoard();
            scoreBoards.Add(scoreBoard);
            //if(i % 10 == 0 ) Console.WriteLine("Done with game number: " + i);            
        }

        foreach (var scoreboard in scoreBoards)
        {
            for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
            {
                
                var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].getName());
                
                entry.Item2[placementIndex]++;
            }
        }

        foreach (var player in playerPlacements)
        {
            var playername = player.Item1;
            //Console.WriteLine(player.Item1);
            using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                }
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                Console.Write(player.Item2[placementIndex]+",");
                //Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                

                using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField(numberOfPlacements);
                    csv.NextRecord();
                }
                
            } 
        Console.WriteLine();
        }


        scoreBoards = new List<List<Iplayer>>();
        // var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",210,10000000)
        //     };
        // var gameState = new GameState(players);
        // gameState.runReturnNumberOfTurns();
        playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
Console.WriteLine();
Console.WriteLine();
        //Console.WriteLine("Seat 4");
        for (int i = 0; i < 1000; i++){ 
            var players = new List<Iplayer>(){
                
                new RandomStackingPlayer("bot 1"),
                new RandomStackingPlayer("bot 2"),
                new RandomStackingPlayer("bot 3"),
                new Cheating_UCT_Player("bot 4",10,1000)          
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.runReturnScoreBoard();
            scoreBoards.Add(scoreBoard);
            //if(i % 10 == 0 ) Console.WriteLine("Done with game number: " + i);            
        }

        foreach (var scoreboard in scoreBoards)
        {
            for (int placementIndex = 0; placementIndex < scoreboard.Count(); placementIndex++)
            {
                
                var entry = playerPlacements.Find(p => p.Item1 == scoreboard[placementIndex].getName());
                
                entry.Item2[placementIndex]++;
            }
        }

        foreach (var player in playerPlacements)
        {
            var playername = player.Item1;
            //Console.WriteLine(player.Item1);
            using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                }
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];
                Console.Write(player.Item2[placementIndex]+",");
                //Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);
                

                using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField(numberOfPlacements);
                    csv.NextRecord();
                }
                
            } 
            Console.WriteLine();
        }

        // Console.Write(scoreBoards.Count());
        //var scoreBoards = gameState.runReturnScoreBoard(); */
    }
    public class Timeline
    {
        public Timeline(int item1, Card card)
        {
            Turn = item1;
            CardColor = card.cardColor.ToString();
            CardType = card.cardType.ToString();
        }

        public int Turn {get; set;}
        public string CardColor {get;set;} 
        public string CardType {get;set;}
    }
}