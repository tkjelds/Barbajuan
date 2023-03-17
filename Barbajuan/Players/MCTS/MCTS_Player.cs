using System.Collections.Concurrent;

public class MCTS_Player : Iplayer
{

    List<Card> Hand = new List<Card>();

    int Determinations = 0;

    int Iterations = 0;

    string Name = new Random().Next(int.MaxValue).ToString();
    
    ImovePicker Picker = new RandomMovePicker();




    public MCTS_Player(List<Card> hand, int determinations, int iterations, string name, ImovePicker picker)
    {
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        Picker = picker;
    }

    public MCTS_Player(string name, int determinations, int iterations)
    {
        Name = name;
        Determinations = determinations;
        Iterations = iterations; 
    }

    public List<Card> action(IgameState gameState)
    {
        var moveRobustness = new ConcurrentBag<(List<Card>,int)>();
        var stackingMoves = new StackingMovePicker();
        Parallel.For(0,Determinations, _ =>{
            var detRoot = createRootDetermination(new Node(null,new List<Node>(),(GameState)gameState,new List<Card>(),0,createEmptyValueList((GameState)gameState),gameState.getCurrentPlayerIndex()));
            var result = MCTS(detRoot);
            foreach (var moveRobust in result)
            { 
                moveRobustness.Add(moveRobust);
            }              
        });
        var moveRobustList = moveRobustness.ToList();
        var resultList = new List<(List<Card>,int)>();
        foreach (var moveRobust in moveRobustList)
        {
            // Check if entry exists
            var exists = resultList.Exists(mr => mr.Item1 == moveRobust.Item1);
            if(!exists) resultList.Add(moveRobust);
            else {
                var moveIndex = resultList.FindIndex(mr => mr.Item1 == moveRobust.Item1);
                var moveValue = resultList[moveIndex].Item2;
                var newMoveValue = moveValue + moveRobust.Item2;
                resultList[moveIndex] = (moveRobust.Item1,newMoveValue);
            }
        }
        moveRobustness.OrderByDescending(mr => mr.Item2);
        return moveRobustness.First().Item1;
    }



    // Returns the first gen children of rootNode, along with the number of simulations gone through them
    public List<(List<Card>,int)> MCTS(Node node){
        for (int i = 0; i < Iterations; i++)
        {
            // Select
            var selected = selection(node);
            // Expand
            // Check to see if terminal
            if (selected.isTerminal()) selected.backPropagate(1,selected.getPlayerIndex());
            else {
                selected.expand();
                // Select a child from selected.
                var selectedChild = selected.getChildren()[0];
                // Simulate
                var childRolloutWinner = rollout(selectedChild);
                // Backpropogate 
                selectedChild.backPropagate(1.0,childRolloutWinner);
            } 
        }
        var children = node.getChildren();
        var result = new List<(List<Card>,int)>();
        foreach (var child in children)
        {
            result.Add((child.getAction(),(int)child.getVisits()));
        }
        return result;
    }
    
    public void addCardsToHand(List<Card> cards)
    {
        Hand.AddRange(cards);
    }

    public List<Card> getHand()
    {
        return Hand;
    }

    public string getName()
    {
        return Name;
    }

    public void removeCardFromHand(Card cards)
    {
        Hand.Remove(cards);
    }
    

    public Node selection(Node node) 
    {
        if(node.isLeaf()) return node;
        Node selected = null;
        var highestUCT = double.MinValue;
        foreach (var n in node.getChildren()) {
            if(highestUCT < n.getUCT()) {
                highestUCT = n.getUCT();
                selected = n;
                }
        }
        return selection(selected);
    }
    
    // RollOut // SIMULATION returns playerindex in gamestate on which player won

    public int rollout(Node node) 
    {
        
        if(node.isTerminal()) return node.getGameState().getCurrentPlayerIndex();

        var gs = node.getGameState().Clone();

        var remainingPlayers = gs.GetPlayers().Count;

        while(gs.GetPlayers().Count == remainingPlayers){
            var toMovePlayerIndex = gs.getCurrentPlayerIndex();
            var action = Picker.pick(gs);
            gs.applyNoClone(action);
            if(gs.GetPlayers().Count < remainingPlayers) return toMovePlayerIndex;
        }
        return -1;
    }


    public Node createRootDetermination(Node node)
    {

        var copyGameState = node.getGameState().Clone();
        List<(int,Iplayer)> cardsInHandPerPlayer = new List<(int, Iplayer)>();

        // Remove cards from players hand.
        // And add them to the draw pile.
        foreach (var p in copyGameState.GetPlayers()) {
            if(p.getName().Trim().Equals(this.Name.Trim()) == false) { // This might be wrong // TODO
                cardsInHandPerPlayer.Add(((p.getHand().Count(),p)));
                foreach (var card in new List<Card>(p.getHand()))
                {
                    copyGameState.getDeck().drawPile.Push(card);
                    p.removeCardFromHand(card);
                }
            }
        }
        // Shuffle Deck (Not discard)
        copyGameState.getDeck().ShuffleDrawPile();
        // Give Cards to players again
        foreach (var k in cardsInHandPerPlayer) {
            k.Item2.addCardsToHand(copyGameState.getDeck().draw((k.Item1)));
        }
        return new Node(null, new List<Node>(), copyGameState, new List<Card>(), 0, node.createEmptyValueList(), copyGameState.getCurrentPlayerIndex());    
    }


    public void rollout(IgameEvaluator evaluator) 
    {
        throw new NotImplementedException();
    }

    public Iplayer clone()
    {
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new MCTS_Player(clonedHand,this.Determinations,this.Iterations,this.Name,this.Picker);
        return clonedPlayer;
    }

    public List<double> createEmptyValueList(GameState gameState){
        List<Double> valueList = new List<double>();
        for (int i = 0; i < gameState.GetPlayers().Count(); i++)
        {
            valueList.Add(0);
        }
        return valueList;
    }
    
    public List<List<Card>> getLegalMoves(Card topCard)
    {
        throw new NotImplementedException();
    }


}
