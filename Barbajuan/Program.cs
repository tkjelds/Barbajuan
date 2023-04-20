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
        var i = 0;
        
        var records = new List<Timeline>();

        while(i<100){
            var temprecords = new List<Timeline>();
            var players = new List<Iplayer>(){
                new MCTS_Player("bot 1",25,250),
                new RandomStackingPlayer("bot 2"),
                new RandomStackingPlayer("bot 3"), 
                new RandomStackingPlayer("bot 4")
            };
            var gameState = new GameState(players);
            var scoreBoard = gameState.RunReturnScoreBoard();

            // ----- Following code runs timeline data modeling experiment.
            /*var timeLine = gameState.runReturnTimeline();
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

            */
            
            scoreBoards.Add(scoreBoard);
            i++;
            if(i % 1 == 0 ) {Console.WriteLine("Done with game number: " + i);};  
        }

        // ----------- Used for writing timeline data to csv file.
        /*
        using (var writer = new StreamWriter(@".\Documentation\UpdatedTimeLine.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
        */
        
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

            // ------------- used for writing placement data to csv file.
            /*using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // csv.WriteRecords(playername);
                    csv.WriteField(playername);
                    csv.NextRecord();
                } 
                */
            
            for (int placementIndex = 0; placementIndex < player.Item2.Count(); placementIndex++)
            {
                var placement = placementIndex + 1;

                var placeNumber = placement;
                var numberOfPlacements = player.Item2[placementIndex];

                Console.WriteLine("Placement : " + placement + "    Amount of times : " + player.Item2[placementIndex]);

                // --------------------- used for writing placement data to csv file.
                // using (var stream = File.Open(@".\Documentation\FlatMonteCarloExperiments.csv", FileMode.Append))
                // using (var writer = new StreamWriter(stream))
                // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                // {
                //     csv.WriteField(numberOfPlacements);
                //     csv.NextRecord();
                // }
            } 
        }
    }
    // Helper class for timeline experiment.
    public class Timeline
    {

        public int turn {get; set;}
        public string cardColor {get;set;} 
        public string cardType {get;set;}
        public int quartile {get;set;}

        public Timeline(int turn, Card card)
        {
            this.turn = turn;
            this.cardColor = card.cardColor.ToString();
            this.cardType = card.cardType.ToString();
        }

        public Timeline(int turn, string cardColor, string cardType, int quartile) 
        {
            this.turn = turn;
            this.cardColor = cardColor;
            this.cardType = cardType;
            this.quartile = quartile;
        }


    }
}
