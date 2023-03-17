using System.Collections.Concurrent;

public class MCTS_Player : Iplayer
{

    List<Card> Hand = new List<Card>();

    int Determinations = 0;

    int Iterations = 0;

    string Name = new Random().Next(int.MaxValue).ToString();
    
    ImovePicker Picker = new StackingMovePicker();




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
        
        var legalMoves = getStackingActions(gameState.getDeck().discardPile.Peek());
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];
        // Console.WriteLine("New turn: ");
        
        //     foreach (var move in legalMoves)
        //     {
        //         Console.Write("Move : ");
        //         foreach (var card in move)
        //         {
        //             Console.Write(card.ToString() + " ");
        //         }
        //         Console.WriteLine();
        //     }
        var moveRobustness = new ConcurrentBag<(List<Card>,int,int)>();
        var stackingMoves = new StackingMovePicker();
        Parallel.For(0,Determinations, _ =>{
            var clonedGameState = gameState.Clone();
            var detRoot = createRootDetermination(clonedGameState);
            var result = MCTSReturnsMoveAndVisitsAndWins(detRoot);
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
        // Console.WriteLine("After MCTS");
        // foreach (var mr in resultList)
        // {
        //     Console.Write("Move : ");
        //     foreach (var card in mr.Item1)
        //     {
        //         Console.Write(card.ToString() + " ");
        //     }
        //     Console.Write("Visits : " + mr.Item2);
        //     Console.Write(" Wins : " + mr.Item3);

        //     Console.WriteLine();
        // }
        resultList.Sort((x,y) => x.Item2.CompareTo(y.Item2));
        return resultList.Last().Item1;
    }
    public List<Card> actionx(IgameState gameState)
    {
        
        var legalMoves = getStackingActions(gameState.getDeck().discardPile.Peek());
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];
        // Console.WriteLine("New turn: ");
        
        //     foreach (var move in legalMoves)
        //     {
        //         Console.Write("Move : ");
        //         foreach (var card in move)
        //         {
        //             Console.Write(card.ToString() + " ");
        //         }
        //         Console.WriteLine();
        //     }
        var moveRobustness = new ConcurrentBag<(List<Card>,int)>();
        var stackingMoves = new StackingMovePicker();
        Parallel.For(0,Determinations, _ =>{
            var clonedGameState = gameState.Clone();
            var detRoot = createRootDetermination(clonedGameState);
            var result = MCTS(detRoot);
            foreach (var moveRobust in result)
            { 
                moveRobustness.Add(moveRobust);
            }              
        });
        var moveRobustList = moveRobustness.ToList();
        var resultList = new List<(List<Card>,int)>();
        CardsComparer cardsTheSame = new CardsComparer();
        foreach (var moveRobust in moveRobustList)
        {
            // Check if entry exists
            var exists = resultList.Exists(mr =>  cardsTheSame.Equals(mr.Item1,moveRobust.Item1));
            if(!exists) resultList.Add(moveRobust);
            else {
                var moveIndex = resultList.FindIndex(mr => cardsTheSame.Equals(mr.Item1,moveRobust.Item1));
                var moveValue = resultList[moveIndex].Item2;
                var newMoveValue = moveValue + moveRobust.Item2;
                resultList[moveIndex] = (moveRobust.Item1,newMoveValue);
            }
        }
        // Console.WriteLine("After MCTS");
        // foreach (var mr in resultList)
        // {
        //     Console.Write("Move : ");
        //     foreach (var card in mr.Item1)
        //     {
        //         Console.Write(card.ToString() + " ");
        //     }
        //     Console.Write("Visits : " + mr.Item2);

        //     Console.WriteLine();
        // }
        resultList.Sort((x,y) => x.Item2.CompareTo(y.Item2));
        return resultList.Last().Item1;
    }



    // Returns the first gen children of rootNode, along with the number of simulations gone through them
    public List<(List<Card>,int)> MCTS(Node node){
        //Console.WriteLine("start MCTS");
        for (int i = 0; i < Iterations; i++)
        {
            var currentNode = node;
            while(!currentNode.isLeaf()){
                currentNode = select(currentNode);
            }
            // Select
            //var selected = selection(node);
            // Expand
            // Check to see if terminal
            if (currentNode.isTerminal()) currentNode.backPropagate(1,currentNode.getParent().getPlayerIndex());
            else {
                currentNode.expand();
                // Select a child from selected.
                var selectedChild = select(currentNode);
                // Simulate
                var childRolloutWinner = rollout(selectedChild);
                // Backpropogate 
                selectedChild.backPropagate(1.0,childRolloutWinner);
                
            } 
        }
        var children = node.getChildren();
        var result = new List<(List<Card>,int)>();
        var amountOfValues = node.getvalue().Count;
        //Console.WriteLine("new MCTS");
        foreach (var child in children)
        {
            //Console.WriteLine(child.getVisits());
            result.Add((child.getAction(),(int)child.getVisits()));
        }
        return result;
    }
    
    public List<(List<Card>,int,int)> MCTSReturnsMoveAndVisitsAndWins(Node node){
        //Console.WriteLine("start MCTS");
        for (int i = 0; i < Iterations; i++)
        {
            var currentNode = node;
            while(!currentNode.isLeaf()){
                currentNode = select(currentNode);
            }
            // Select
            //var selected = selection(node);
            // Expand
            // Check to see if terminal
            if (currentNode.isTerminal()) currentNode.backPropagate(1,currentNode.getParent().getPlayerIndex());
            else {
                currentNode.expand();
                // Select a child from selected.
                var selectedChild = select(currentNode);
                // Simulate
                var childRolloutWinner = rollout(selectedChild);
                // Backpropogate 
                selectedChild.backPropagate(1.0,childRolloutWinner);
                
            } 
        }
        var children = node.getChildren();
        var result = new List<(List<Card>,int,int)>();
        var amountOfValues = node.getvalue().Count;
        //Console.WriteLine("new MCTS");
        foreach (var child in children)
        {
            //Console.WriteLine(child.getVisits());
            result.Add((child.getAction(),(int)child.getVisits(),(int)child.getPlayerValue(0)));
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
     
    public Node select(Node node){    
        Node? selected = null;
        double bestValue = double.MinValue;
        foreach (var child in node.getChildren())
        {   
            double uctValue = child.getUCT();
            if(uctValue > bestValue){
                selected = child;
                bestValue = uctValue;                           
            }
        }
        return selected;
    }

    // RollOut // SIMULATION returns playerindex in gamestate on which player won

    public int rollout(Node node) 
    {
        
        if(node.isTerminal()) {
            return node.getParent().getPlayerIndex();        
        } 

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


    public Node createRootDetermination(GameState gameState)
    {
        var copyGameState = gameState.Clone();
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
        return new Node(null, new List<Node>(), copyGameState, new List<Card>(), 0, createEmptyValueList(copyGameState), copyGameState.getCurrentPlayerIndex());    
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
        return getStackingActions(topCard);
    }
    public List<List<Card>> getStackingActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(Hand))
        {
            if (card.canBePlayedOn(topCard))
            {
                if (card.cardType == DRAW4)
                {
                    moves.Add(new List<Card>() { new Card(GREEN, DRAW4) });
                    moves.Add(new List<Card>() { new Card(BLUE, DRAW4) });
                    moves.Add(new List<Card>() { new Card(YELLOW, DRAW4) });
                    moves.Add(new List<Card>() { new Card(RED, DRAW4) });
                }
                if (card.cardType == SELECTCOLOR)
                {
                    moves.Add(new List<Card>() { new Card(GREEN, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(BLUE, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(YELLOW, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(RED, SELECTCOLOR) });
                }
                if (card.cardType != SELECTCOLOR && card.cardType != DRAW4)
                {
                    var nextHand = new List<Card>(Hand);
                    moves.Add(new List<Card>() { card });
                    nextHand.Remove(card);
                    moves.AddRange(getCardOfSameType(card, nextHand, new List<Card>() { card }));
                }

            }
        }
        return moves;
    }
    public List<List<Card>> getCardOfSameType(Card toBePlayedOn, List<Card> hand, List<Card> currentStack)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
        {
            var nextHand = new List<Card>(hand);
            if (card.cardType == toBePlayedOn.cardType)
            {
                var moveStack = new List<Card>(currentStack);
                moveStack.Add(card);
                moves.Add(moveStack);
                nextHand.Remove(card);
                moves.AddRange(getCardOfSameType(card, nextHand, moveStack));

            }
        }
        return moves;
    }

}
