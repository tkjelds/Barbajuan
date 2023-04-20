public class RandomStackingPlayer : Iplayer
{

    string name;

    List<Card> hand = new List<Card>();



    public RandomStackingPlayer(string name)
    {
        this.name = name;
    }

    public RandomStackingPlayer(string name, List<Card> hand) : this(name)
    {
        this.hand = hand;
    }

    public List<Card> Action(GameState gameState)
    {
        var moves = GetStackingActions(gameState.GetDeck().discardPile.Peek());
        var rng = new Random();

        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moves.Distinct();
        return moves[rng.Next(moves.Count())];
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
    public List<List<Card>> GetStackingActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
        {
            if (card.CanBePlayedOn(topCard))
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
                    var nextHand = new List<Card>(hand);
                    moves.Add(new List<Card>() { card });
                    nextHand.Remove(card);
                    moves.AddRange(GetCardOfSameType(card, nextHand, new List<Card>() { card }));
                }

            }
        }
        return moves;
    }
    public List<List<Card>> GetCardOfSameType(Card toBePlayedOn, List<Card> hand, List<Card> currentStack)
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
                moves.AddRange(GetCardOfSameType(card, nextHand, moveStack));

            }
        }
        return moves;
    }
    public Iplayer Clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new RandomStackingPlayer(this.name,clonedHand);
        return clonedPlayer;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        var legalMoves = GetStackingActions(topCard);
        if(legalMoves.Count == 0 ) return new List<List<Card>>() { new List<Card>(){new Card(WILD, DRAW1)} };
        legalMoves.Distinct();
        return legalMoves;
    }
}
