using System.Globalization;
using CsvHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        List<List<Iplayer>> scoreBoards = new List<List<Iplayer>>();
        // var players = new List<Iplayer>(){
        //         new Player("bot 1"),
        //         new Player("bot 2"),
        //         new Player("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",25,10000)
        //     };
        // var gameState = new GameState(players);
        // gameState.runReturnNumberOfTurns();
        List<(String, List<int>)> playerPlacements = new List<(string, List<int>)>();
        playerPlacements.Add(("bot 1", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 2", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 3", new List<int>() { 0, 0, 0, 0 }));
        playerPlacements.Add(("bot 4", new List<int>() { 0, 0, 0, 0 }));
        var i = 0;
        
        var records = new List<Timeline>();

        while(i<10000){
            var temprecords = new List<Timeline>();
            var players = new List<Iplayer>(){
                new MCTS_Player("bot 1",50,250),
                new RandomStackingPlayer("bot 2"),
                new RandomStackingPlayer("bot 3"), 
                new RandomStackingPlayer("bot 4")
            };
            var gameState = new GameState(players);
            //var scoreBoard = gameState.runReturnScoreBoard();
            var timeLine = gameState.runReturnTimeline();
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

            //Console.WriteLine("Turn count = " + turnCount);
            //Console.WriteLine("Turn count as a double = " + turnCountDouble);
            //Console.WriteLine("Quartile size = " + quart);
            foreach(var t in temprecords) {
                if(t.Turn <= quart) {
                    t.Quartile = 1;
                    
                } else if(t.Turn <= (quart*2.0)) {
                    t.Quartile = 2;
                    
                } else if(t.Turn <= (quart*3.0)) {
                    t.Quartile = 3;
                    
                } else if(t.Turn <= (quart*4.0)) {
                    t.Quartile = 4;
                   
                }
            }
            
            foreach(var t in temprecords) {
                records.Add(t);
            }

            
            // records.Add(new Timeline(turnCount, "ENDOFGAME", (i+1).ToString()));
            //scoreBoards.Add(scoreBoard);
            i++;
            if(i % 1 == 0 ) {Console.WriteLine("Done with game number: " + i);};  
            //Console.WriteLine("Done with game number: " + i);
        }

        
        using (var writer = new StreamWriter(@".\Documentation\UpdatedTimeLine.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
        
        

        
        // for (int i = 0; i < 100; i++)
        // { 
        //     var players = new List<Iplayer>(){
                
        //         new RandomStackingPlayer("bot 1"),
        //         new RandomStackingPlayer("bot 2"),
        //         new FlatMonteCarloPlayer("bot 3", 250, 10),
        //         new RandomStackingPlayer("bot 4")          
        //     };
        //     var gameState = new GameState(players);
        //     var scoreBoard = gameState.runReturnScoreBoard();
        //     scoreBoards.Add(scoreBoard);
        //     Console.WriteLine("Done with game number: " + i);            
        // }
        // Parallel.ForEach(Partitioner.Create(0, 100), range => {
        // for (var index = range.Item1; index < range.Item2; index++) {
        //     var players = new List<Iplayer>(){
        //         new FlatMonteCarloPlayer("bot 1", 10, 1000),
        //         new StackingMovePlayer("bot 2"),
        //         new StackingMovePlayer("bot 3"),
        //         new StackingMovePlayer("bot 4")         
        //     };
        //     var gameState = new GameState(players);
        //     var scoreBoard = gameState.runReturnScoreBoard();
        //     scoreBoards.Add(scoreBoard);
        //     Console.WriteLine("Done with game number: " + index);   
        // }
        // });
        // Parallel.For(1, 100, number =>
        // {
        //     var players = new List<Iplayer>(){
        //         new StackingMovePlayer("bot 1"),
        //         new StackingMovePlayer("bot 2"),
        //         new StackingMovePlayer("bot 3"),
        //         new FlatMonteCarloPlayer("bot 4",10,1000)
        //     };
        //     var gameState = new GameState(players);
        //     var scoreBoard = gameState.runReturnScoreBoard();
        //     scoreBoards.Add(scoreBoard);
        //     Console.WriteLine("Done with game number: " + number);
        // });
        /*
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
            Console.WriteLine(player.Item1);
            /*using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                } 
                */
            /*
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);

                
                using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField(numberOfPlacements);
                    csv.NextRecord();
                }*/
            } 
        }
    


        // Console.Write(scoreBoards.Count());
        //var scoreBoards = gameState.runReturnScoreBoard(); 
    public class Timeline
    {
        public Timeline(int item1, Card card)
        {
            Turn = item1;
            CardColor = card.cardColor.ToString();
            CardType = card.cardType.ToString();
        }

        public Timeline(int item1, string cardColor, string cardType, int quartile) 
        {
            Turn = item1;
            CardColor = cardColor;
            CardType = cardType;
            Quartile = quartile;
        }

        public int Turn {get; set;}
        public string CardColor {get;set;} 
        public string CardType {get;set;}

        public int Quartile {get;set;}
    }

