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

[Serializable]
public class FlatMonteCarloPlayer : Iplayer
{

    List<Card> Hand = new List<Card>();

    int Determinations = 0;

    int Iterations = 0;

    string Name = "Not_Assigned";

    ImoveEvaluator Evaluator = new NaiveMoveEvaluator();
    
    ImovePicker Picker = new RandomMovePicker();
    



    public FlatMonteCarloPlayer(){}


    public FlatMonteCarloPlayer( string name, int determinations, int iterations ){
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
    }


    public FlatMonteCarloPlayer(List<Card> hand, int determinations, int iterations, string name)
    {
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
    }

    public FlatMonteCarloPlayer(List<Card> hand, int determinations, int iterations, string name, ImoveEvaluator evaluator, ImovePicker picker)
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
        ConcurrentDictionary<int,int> moveAndValue = new ConcurrentDictionary<int, int>();
        var legalMoves = getStackingActions(gameState.getDeck().discardPile.Peek());
        if (legalMoves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moveAndValue = new ConcurrentDictionary<int, int>(Determinations,legalMoves.Count());
        var  numberToMove = new List<(int,List<Card>)>();    
        for (int i = 0; i < legalMoves.Count(); i++)
        {
            numberToMove.Add((i,legalMoves[i]));
            moveAndValue.TryAdd(i,0);
        }
        // -------------
        
        // Create a number of determinations
        var determinations = new List<GameState>();
        for (int i = 0; i < Determinations; i++)
        {
            determinations.Add(createDetermination((GameState) gameState));
        }

        // Simulate each determination a number of times, run in parallel
        Parallel.ForEach(determinations, d => {
            // int utilSum = 0;
            for (int i = 0; i < Iterations; i++)
            {
                var copyOfd = d.DeepClone(d);
                var value = Simulate(copyOfd);
                // Big if true
                while (true)
                {
                    var returnedMove = value.Item1;
                    //Console.WriteLine("Jeg skal til at blive fundet");
                    var numberOfMove = numberToMove.Find(m => m.Equals(returnedMove)).Item1;
                    //Console.WriteLine("Jeg er blevet fundet");
                    var existing = moveAndValue[numberOfMove];
                    var updated = existing + value.Item2;
                    //Console.WriteLine("Jeg er blevet opdateret lokalt og skal til at blive opdateret i ditionarien");
                    if (moveAndValue.TryUpdate(numberOfMove, updated, existing)) break;
                }   
                // var updatedValue = moveAndValue[value.item1] + value.item2; 
                // moveAndValue.TryUpdate(value);
                // utilsum += Simulate(copyOfd); 
                // Update util value for move in dictionary   
            }
        });
        Console.WriteLine("Number of keys in dict : " + moveAndValue.Count() + "   Number of legal moves : " + legalMoves.Count());
        
        var moveAndValueList = moveAndValue.ToList();
        foreach (var mv in moveAndValueList)
        {
            var move = numberToMove[mv.Key];
            Console.WriteLine("Move : ");
            foreach (var card in move.Item2)
            {
                Console.Write("  " + card.ToString() +  "  ");
            }
            Console.WriteLine("Number of wins : " + mv.Value);
        }
        moveAndValueList.Sort((x,y) => x.Value.CompareTo(y.Value));
        return numberToMove.Find(x => x.Item1 == moveAndValueList[0].Key).Item2;
    }

    private (List<Card>, int) Simulate(GameState determination)
    {
        var pickedAction = Picker.pick(determination);
        var result = (pickedAction, 0);
        determination.apply(pickedAction);
        var notGameOver = true;
        while (notGameOver)
        {
            var action = Picker.pick(determination);
            determination.applyNoClone(action);
            if(determination.IsGameOver()){
                determination.getScoreBoard().Add(determination.GetPlayers()[0]);
                notGameOver = false;
                var actionValue = Evaluator.evaluate(determination, Name);
                result.Item2 = actionValue;
                return result;
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
        var copyGameState = gs.DeepClone(gs);
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
        // SHuffle Deck (Not discard)
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
}