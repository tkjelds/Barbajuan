using System.Collections.Concurrent;

public class Cheating_UCT_Player : Iplayer
{

    List<Card> hand = new List<Card>();

    int determinizations = 0;

    int iterations = 0;

    string name = new Random().Next(int.MaxValue).ToString();

    ImovePicker picker = new StackingMovePicker();




    public Cheating_UCT_Player(List<Card> hand, int determinizations, int iterations, string name, ImovePicker picker)
    {
        this.hand = hand;
        this.determinizations = determinizations;
        this.iterations = iterations;
        this.name = name;
        this.picker = picker;
    }

    public Cheating_UCT_Player(string name, int determinizations, int iterations)
    {
        this.name = name;
        this.determinizations = determinizations;
        this.iterations = iterations;
    }
    public List<Card> Action(GameState gameState)
    {

        var legalMoves = new StackingMovePicker().GetStackingActions(gameState.GetDeck().discardPile.Peek(), hand);
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];

        var moveRobustness = new ConcurrentBag<(List<Card>, int, int)>();
        var stackingMoves = new StackingMovePicker();

        Parallel.For(0, determinizations, _ =>
        {
            var clonedGameState = gameState.Clone();
            var determinationRoot = CreateRoot(clonedGameState);
            var result = MCTS(determinationRoot);
            foreach (var moveRobust in result)
            {
                moveRobustness.Add(moveRobust);
            }
        });
        var moveRobustList = moveRobustness.ToList();
        var resultList = new List<(List<Card>, int, int)>();
        CardsComparer cardsTheSame = new CardsComparer();
        foreach (var moveRobust in moveRobustList)
        {
            // Check if entry exists
            var exists = resultList.Exists(mr => cardsTheSame.Equals(mr.Item1, moveRobust.Item1));
            if (!exists) resultList.Add(moveRobust);
            else
            {
                var moveIndex = resultList.FindIndex(mr => cardsTheSame.Equals(mr.Item1, moveRobust.Item1));
                var moveValue = resultList[moveIndex].Item2;
                var movewins = resultList[moveIndex].Item3;
                var newWinsValue = movewins + moveRobust.Item3;
                var newMoveValue = moveValue + moveRobust.Item2;
                resultList[moveIndex] = (moveRobust.Item1, newMoveValue, newWinsValue);
            }
        }
        resultList.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        var bestMove = resultList.Last().Item1;
        return bestMove;
    }

    public List<(List<Card>, int, int)> MCTS(Node node)
    {
        for (int i = 0; i < iterations; i++)
        {
            var currentNode = node;
            while (!currentNode.IsLeaf())
            {
                currentNode = Select(currentNode);
            }
            // Select
            //var selected = selection(node);
            // Expand
            // Check to see if terminal
            if (currentNode.IsTerminal()) currentNode.BackPropagate(1, currentNode.GetPlayerIndex());
            else
            {
                currentNode.Expand();
                // Select a child from selected.
                var selectedChild = Select(currentNode);
                // Simulate
                var childRolloutWinner = Rollout(selectedChild);
                // Backpropogate 
                selectedChild.BackPropagate(1.0, childRolloutWinner);

            }
        }
        var children = node.GetChildren();
        var result = new List<(List<Card>, int, int)>();
        var amountOfValues = node.Getvalue().Count;
        foreach (var child in children)
        {
            result.Add((child.GetAction(), (int)child.GetVisits(), (int)child.GetPlayerValue(0)));
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

    public Node Select(Node node)
    {
        Node? selected = null;
        double bestValue = double.MinValue;
        foreach (var child in node.GetChildren())
        {
            double uctValue = child.GetUCT();
            if (uctValue > bestValue)
            {
                selected = child;
                bestValue = uctValue;
            }
        }
        return selected!;
    }


    public int Rollout(Node node)
    {

        if (node.IsTerminal())
        {
            return node.GetPlayerIndex();
        }

        var gs = node.GetGameState().Clone();

        var remainingPlayers = gs.GetPlayers().Count;

        while (gs.GetPlayers().Count == remainingPlayers)
        {
            var toMovePlayerIndex = gs.GetCurrentPlayerIndex();
            var action = picker.Pick(gs);
            gs.ApplyNoClone(action);
            if (gs.GetPlayers().Count < remainingPlayers) return toMovePlayerIndex;
        }
        return -1;
    }


    public Node CreateRoot(GameState gameState)
    {
        var copyGameState = gameState.Clone();
        return new Node(null, new List<Node>(), copyGameState, new List<Card>(), 0, CreateEmptyValueList(copyGameState), -1);  // change to not use index (Root should not have index)  
    }

    public Iplayer Clone()
    {
        var clonedHand = new List<Card>();
        foreach (var card in this.hand)
        {
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new Cheating_UCT_Player(clonedHand, this.determinizations, this.iterations, this.name, this.picker);
        return clonedPlayer;
    }

    public List<double> CreateEmptyValueList(GameState gameState)
    {
        List<Double> valueList = new List<double>();
        for (int i = 0; i < gameState.GetPlayers().Count(); i++)
        {
            valueList.Add(0);
        }
        return valueList;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        return new StackingMovePicker().GetStackingActions(topCard, hand);
    }
}