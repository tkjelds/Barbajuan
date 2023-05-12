
public class RandomPlayer : Iplayer
{
    public List<Card> hand = new List<Card>();

    public string name = "Unassigned";

    public RandomPlayer(List<Card> hand, string name)
    {
        this.hand = hand;
        this.name = name;
    }

    public RandomPlayer(List<Card> hand)
    {
        this.hand = hand;
    }

    public RandomPlayer(string name)
    {
        this.name = name;
    }

    public List<Card> Action(GameState gameState)
    {
        var moves = this.GetActions(gameState.GetDeck().discardPile.Peek());
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(CardColor.WILD, CardType.DRAW1) };
        }

        var random = new Random();

        var randomMove = random.Next(moves.Count);

        return moves[randomMove];
    }
    public List<List<Card>> GetActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(this.hand))
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
                else
                {
                    moves.Add(new List<Card>() { card });
                }


            }
        }
        return moves;
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
    public List<Card> GetHand()
    {
        return this.hand;
    }

    public void RemoveCardFromHand(Card cards)
    {
        hand.Remove(cards);
    }

    public string GetName()
    {
        return name;
    }

    public void AddCardsToHand(List<Card> cards)
    {
        this.hand.AddRange(cards);
    }

    public Iplayer Clone()
    {
        var clonedHand = new List<Card>();
        foreach (var card in hand)
        {
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new RandomPlayer(clonedHand, this.name);
        return clonedPlayer;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        var legalMoves = GetActions(topCard);
        if (legalMoves.Count == 0) return new List<List<Card>>() { new List<Card>() { new Card(WILD, DRAW1) } };
        legalMoves.Distinct();
        return legalMoves;
    }
}
