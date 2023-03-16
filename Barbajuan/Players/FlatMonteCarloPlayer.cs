/*

public class monteCarloNode

Value NodeValue
Iterationer NumberOfIterations
List<Card> Action
GameState gs
List<MonteCarloNode> children
MonteCarloNode   Parent

*/


using System.Collections.Concurrent;

public class FlatMonteCarloPlayer : Iplayer
{

    List<Card> Hand = new List<Card>();

    int Determinations = 0;

    int Iterations = 0;

    string Name = "Not_Assigned";

    IgameEvaluator Evaluator = new FactorialEvaluator();
    
    ImovePicker Picker = new RandomMovePicker();
    



    public FlatMonteCarloPlayer(){}


    public FlatMonteCarloPlayer( string name, int determinations, int iterations ){
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
    }

    public FlatMonteCarloPlayer( string name, int determinations, int iterations, ImovePicker movepicker, IgameEvaluator evaluator ){
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        this.Picker = movepicker;
        this.Evaluator = evaluator;
    }


    public FlatMonteCarloPlayer(List<Card> hand, int determinations, int iterations, string name)
    {
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
    }

    public FlatMonteCarloPlayer(List<Card> hand, int determinations, int iterations, string name, IgameEvaluator evaluator, ImovePicker picker)
    {
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        Evaluator = evaluator;
        Picker = picker;
    }

    public List<Card> action(IgameState gameState)
    {
        // Add all our own legal moves to the moveAndValue bag
        // -------------
        ConcurrentDictionary<int, int> moveAndValue = new ConcurrentDictionary<int, int>();
        CardsComparer cardsTheSame = new CardsComparer();
        var legalMoves = getStackingActions(gameState.getDeck().discardPile.Peek());
        if (legalMoves.Count == 0) return new List<Card>() { new Card(WILD, DRAW1) };
        if (legalMoves.Count == 1) return legalMoves[0];
        legalMoves.Distinct();

        moveAndValue = new ConcurrentDictionary<int, int>(Determinations, legalMoves.Count());
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
        for (var i = 0; i < Determinations; i++)
        {
            var d = createDetermination((GameState)gameState);
            Parallel.For(0, Iterations, x =>
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
        var pickedAction = Picker.pick(determination);

        var result = (pickedAction, 0);

        determination.applyNoClone(pickedAction);

        if (determination.IsGameOver()  || (determination.GetPlayers().Find(p => p.getName() == Name) == null) )
        {
            result.Item2 = Evaluator.evaluate(determination,Name);
            return result;
        }

        var notGameOver = true;
        while (notGameOver) // TODO change to when we are knocked out
        {
            var action = Picker.pick(determination);
            determination.applyNoClone(action);
            if(determination.IsGameOver() || (determination.GetPlayers().Find(p => p.getName() == Name) == null)) {
                if (determination.IsGameOver()) determination.getScoreBoard().Add(determination.GetPlayers()[0]);
                notGameOver = false;
                var actionValue = Evaluator.evaluate(determination, Name);
                result.Item2 = actionValue;
            }
        }
        return result;       
    }

    public void addCardsToHand(List<Card> cards)
    {
        this.Hand.AddRange(cards);
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

    public GameState createDetermination(GameState gs) {
        
        var copyGameState = gs.Clone();
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
        return copyGameState;
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


    /*
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        Evaluator = evaluator;
        Picker = picker;
        Bloody mary, bloody mary, bloody mary*/
    public Iplayer clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new FlatMonteCarloPlayer(clonedHand,this.Determinations,this.Iterations,this.Name,this.Evaluator,this.Picker);
        return clonedPlayer;
    }
}