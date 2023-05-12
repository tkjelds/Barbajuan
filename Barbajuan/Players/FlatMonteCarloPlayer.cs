using System.Collections.Concurrent;

public class FlatMonteCarloPlayer : Iplayer
{

    List<Card> hand = new List<Card>();

    int determinizations = 0;

    int iterations = 0;

    string name = "Not_Assigned";

    IgameEvaluator evaluator = new FactorialEvaluator();

    ImovePicker picker = new RandomMovePicker();




    public FlatMonteCarloPlayer() { }


    public FlatMonteCarloPlayer(string name, int determinizations, int iterations)
    {
        this.determinizations = determinizations;
        this.iterations = iterations;
        this.name = name;
    }

    public FlatMonteCarloPlayer(string name, int determinizations, int iterations, ImovePicker movepicker, IgameEvaluator evaluator)
    {
        this.determinizations = determinizations;
        this.iterations = iterations;
        this.name = name;
        this.picker = movepicker;
        this.evaluator = evaluator;
    }


    public FlatMonteCarloPlayer(List<Card> hand, int determinizations, int iterations, string name)
    {
        this.hand = hand;
        this.determinizations = determinizations;
        this.iterations = iterations;
        this.name = name;
    }

    public FlatMonteCarloPlayer(List<Card> hand, int determinizations, int iterations, string name, IgameEvaluator evaluator, ImovePicker picker)
    {
        this.hand = hand;
        this.determinizations = determinizations;
        this.iterations = iterations;
        this.name = name;
        this.evaluator = evaluator;
        this.picker = picker;
    }

    public List<Card> Action(GameState gameState)
    {
        // Add all our own legal moves to the moveAndValue bag
        // -------------
        ConcurrentDictionary<int, int> moveAndValue = new ConcurrentDictionary<int, int>();
        CardsComparer cardsTheSame = new CardsComparer();
        var legalMoves = new StackingMovePicker().GetStackingActions(gameState.GetDeck().discardPile.Peek(), hand);
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];
        legalMoves.Distinct();

        moveAndValue = new ConcurrentDictionary<int, int>(determinizations, legalMoves.Count());
        var numberToMove = new List<(int, List<Card>)>();
        for (int i = 0; i < legalMoves.Count(); i++)
        {
            numberToMove.Add((i, legalMoves[i]));
            moveAndValue.TryAdd(i, 0);
        }
        // -------------

        // Create a number of determinations
        // var determinations = new List<GameState>();

        // Simulate each determination a number of times, run in parallel
        for (var i = 0; i < determinizations; i++)
        {
            var d = CreateDetermination((GameState)gameState);
            Parallel.For(0, iterations, x =>
            {
                //var copyOfd = d.DeepClone(d);
                var copyOfd = d.Clone();
                var value = Simulate(copyOfd);
                // Big if true
                while (true)
                {
                    var returnedMove = value.Item1;
                    var numberOfMove = numberToMove.Find(m => cardsTheSame.Equals(m.Item2, returnedMove)).Item1;
                    var existing = moveAndValue[numberOfMove];
                    var updated = existing + value.Item2;
                    if (moveAndValue.TryUpdate(numberOfMove, updated, existing)) break;
                }

            });

        }

        var moveAndValueList = moveAndValue.ToList();

        moveAndValueList.Sort((x, y) => x.Value.CompareTo(y.Value));

        //Console.WriteLine("Total number of tested games (I think): " + totalnumberofgames);
        var bestMove = moveAndValueList.Last().Key;
        var chosenMove = numberToMove.Find(x => x.Item1 == bestMove).Item2;
        return chosenMove;
    }

    private static void PrintMoveAndValue(List<(int, List<Card>)> numberToMove, List<KeyValuePair<int, int>> moveAndValueList)
    {
        Console.WriteLine("New Move");
        foreach (var MAV in moveAndValueList)
        {
            var move = numberToMove.Find(x => x.Item1 == MAV.Key).Item2;
            Console.Write("Move : ");

            foreach (var card in move)
            {
                Console.Write("  " + card.ToString());
            }
            Console.Write(" | Value : " + MAV.Value);
            Console.WriteLine();
        }
        Console.WriteLine("Turn over");
    }

    private (List<Card>, int) Simulate(GameState determination)
    {
        var pickedAction = picker.Pick(determination);

        var result = (pickedAction, 0);

        determination.ApplyNoClone(pickedAction);

        if (determination.IsGameOver() || (determination.GetPlayers().Find(p => p.GetName() == name) == null))
        {
            result.Item2 = evaluator.Evaluate(determination, name);
            return result;
        }

        var notGameOver = true;
        while (notGameOver) // TODO change to when we are knocked out
        {
            var action = picker.Pick(determination);
            determination.ApplyNoClone(action);
            if (determination.IsGameOver() || (determination.GetPlayers().Find(p => p.GetName() == name) == null))
            {
                if (determination.IsGameOver()) determination.GetScoreBoard().Add(determination.GetPlayers()[0]);
                notGameOver = false;
                var actionValue = evaluator.Evaluate(determination, name);
                result.Item2 = actionValue;
            }
        }
        return result;
    }

    public void AddCardsToHand(List<Card> cards)
    {
        this.hand.AddRange(cards);
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

    public GameState CreateDetermination(GameState gs)
    {

        var copyGameState = gs.Clone();
        List<(int, Iplayer)> cardsInHandPerPlayer = new List<(int, Iplayer)>();

        // Remove cards from players hand.
        // And add them to the draw pile.
        foreach (var p in copyGameState.GetPlayers())
        {
            if (p.GetName().Trim().Equals(this.name.Trim()) == false)
            { // This might be wrong // TODO
                cardsInHandPerPlayer.Add(((p.GetHand().Count(), p)));
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
        foreach (var k in cardsInHandPerPlayer)
        {
            k.Item2.AddCardsToHand(copyGameState.GetDeck().Draw((k.Item1)));
        }
        return copyGameState;
    }
    /*
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        Evaluator = evaluator;
        Picker = picker;
        Bloody mary, bloody mary, bloody mary*/
    public Iplayer Clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach (var card in this.hand)
        {
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new FlatMonteCarloPlayer(clonedHand, this.determinizations, this.iterations, this.name, this.evaluator, this.picker);
        return clonedPlayer;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        var legalMoves = new StackingMovePicker().GetStackingActions(topCard, hand);
        if (legalMoves.Count == 0) return new List<List<Card>>() { new List<Card>() { new Card(WILD, DRAW1) } };
        legalMoves.Distinct();
        return legalMoves;
    }
}