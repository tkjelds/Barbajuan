using System.Collections.Concurrent;

public class MCTS_Player : Iplayer
{

    List<Card> hand = new List<Card>();

    int determinations = 0;

    int iterations = 0;

    string name = new Random().Next(int.MaxValue).ToString();
    
    ImovePicker picker = new StackingMovePicker();


    public MCTS_Player(string name, int determinations, int iterations, ImovePicker picker){
        this.name = name;
        this.determinations = determinations;
        this.iterations = iterations; 
        this.picker = picker;
    }



    public MCTS_Player(List<Card> hand, int determinations, int iterations, string name, ImovePicker picker)
    {
        this.hand = hand;
        this.determinations = determinations;
        this.iterations = iterations;
        this.name = name;
        this.picker = picker;
    }

    public MCTS_Player(string name, int determinations, int iterations)
    {
        this.name = name;
        this.determinations = determinations;
        this.iterations = iterations; 
    }
    public List<Card> Action(GameState gameState)
    {
        
        var legalMoves = new StackingMovePicker().GetStackingActions(gameState.GetDeck().discardPile.Peek(),hand);
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];
     
        var moveRobustness = new ConcurrentBag<(List<Card>,int,int)>();
        var stackingMoves = new StackingMovePicker();
        
        Parallel.For(0,determinations, _ =>{
            var clonedGameState = gameState.Clone();
            var determinationRoot = CreateRootDetermination(clonedGameState);
            var result = MCTS(determinationRoot);
            foreach (var moveRobust in result)
            { 
                moveRobustness.Add(moveRobust);
            }              
        });
        var moveRobustList = moveRobustness.ToList();
        var resultList = new List<(List<Card>,int,int)>();
        CardsComparer cardsTheSame = new CardsComparer();
        foreach (var moveRobust in moveRobustList)
        {
            // Check if entry exists
            var exists = resultList.Exists(mr =>  cardsTheSame.Equals(mr.Item1,moveRobust.Item1));
            if(!exists) resultList.Add(moveRobust);
            else {
                var moveIndex = resultList.FindIndex(mr => cardsTheSame.Equals(mr.Item1,moveRobust.Item1));
                var moveValue = resultList[moveIndex].Item2;
                var movewins = resultList[moveIndex].Item3;
                var newWinsValue = movewins + moveRobust.Item3;
                var newMoveValue = moveValue + moveRobust.Item2;
                resultList[moveIndex] = (moveRobust.Item1,newMoveValue,newWinsValue);
            }
        }

        resultList.Sort((x,y) => x.Item2.CompareTo(y.Item2));
        var bestMove = resultList.Last().Item1;
    
        return bestMove;
    }
    
    public List<(List<Card>,int,int)> MCTS(Node node){
        //Console.WriteLine("start MCTS");
        for (int i = 0; i < iterations; i++)
        {
            // Select
            var currentNode = node;
            while(!currentNode.IsLeaf()){
                currentNode = Select(currentNode);
            }
            // Expand
            // Check to see if terminal
            if (currentNode.IsTerminal()) currentNode.BackPropagate(1,currentNode.GetPlayerIndex()); 
            else {
                currentNode.Expand();
                // Select a child from selected.
                var selectedChild = Select(currentNode);
                // Simulate
                var childRolloutWinner = Rollout(selectedChild);
                // Backpropogate 
                selectedChild.BackPropagate(1.0,childRolloutWinner);
                
            } 
        }
        var children = node.GetChildren();
        var result = new List<(List<Card>,int,int)>();
        var amountOfValues = node.Getvalue().Count;
        
        foreach (var child in children)
        {
            
            result.Add((child.GetAction(),(int)child.GetVisits(),(int)child.GetPlayerValue(0)));
        }
        return result;
    }
    public void AddCardsToHand(List<Card> cards)
    {
        hand.AddRange(cards);
    }

    public List<Card> GetHand()
    {
        return hand;
    }

    public string GetName()
    {
        return name;
    }

    public void RemoveCardFromHand(Card cards)
    {
        hand.Remove(cards);
    }
     
    public Node Select(Node node){    
        Node? selected = null;
        double bestValue = double.MinValue;
        foreach (var child in node.GetChildren())
        {   
            double uctValue = child.GetUCT();
            if(uctValue > bestValue){
                selected = child;
                bestValue = uctValue;                           
            }
        }
        return selected!;
    }

    // RollOut // SIMULATION returns playerindex in gamestate on which player won

    public int Rollout(Node node) 
    {
        
        if(node.IsTerminal()) {
            return node.GetPlayerIndex();       
        } 

        var gs = node.GetGameState().Clone();

        var remainingPlayers = gs.GetPlayers().Count;

        while(gs.GetPlayers().Count == remainingPlayers){
            var toMovePlayerIndex = gs.GetCurrentPlayerIndex();
            var action = picker.Pick(gs);
            gs.ApplyNoClone(action);
            if(gs.GetPlayers().Count < remainingPlayers) return toMovePlayerIndex;
        }
        return -1;
    }


    public Node CreateRootDetermination(GameState gameState)
    {
        var copyGameState = gameState.Clone();
        List<(int,Iplayer)> cardsInHandPerPlayer = new List<(int, Iplayer)>();

        // Remove cards from players hand.
        // And add them to the draw pile.
        foreach (var p in copyGameState.GetPlayers()) {
            if(p.GetName().Trim().Equals(this.name.Trim()) == false) { // This might be wrong // TODO
                cardsInHandPerPlayer.Add(((p.GetHand().Count(),p)));
                foreach (var card in new List<Card>(p.GetHand()))
                {
                    copyGameState.GetDeck().drawPile.Push(card);
                    p.RemoveCardFromHand(card);
                }
            }
        }
        // Shuffle Deck (Not discard)
        copyGameState.GetDeck().ShuffleDrawPile();
        // Give Cards to players again
        foreach (var k in cardsInHandPerPlayer) {
            k.Item2.AddCardsToHand(copyGameState.GetDeck().Draw((k.Item1)));
        }
        return new Node(null, new List<Node>(), copyGameState, new List<Card>(), 0, CreateEmptyValueList(copyGameState), -1);  // change to not use index (Root should not have index)  
    }

    public Iplayer Clone()
    {
        var clonedHand = new List<Card>();
        foreach(var card in this.hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new MCTS_Player(clonedHand,this.determinations,this.iterations,this.name,this.picker);
        return clonedPlayer;
    }

    public List<double> CreateEmptyValueList(GameState gameState){
        List<Double> valueList = new List<double>();
        for (int i = 0; i < gameState.GetPlayers().Count(); i++)
        {
            valueList.Add(0);
        }
        return valueList;
    }
    
    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        return new StackingMovePicker().GetStackingActions(topCard,hand);;
    }

}
