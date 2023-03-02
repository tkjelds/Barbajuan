using static CardColor;
using static CardType;
[Serializable]
public class StackingMovePlayer : Iplayer
{
    String name;
    List<Card> hand = new List<Card>();

    public StackingMovePlayer(String name) => this.name = name;
    public StackingMovePlayer(String name, List<Card> hand)
    {
        this.name = name;
        this.hand = hand;
    }

    public List<Card> action(IgameState gameState)
    {

        var moves = getStackingActions(gameState.getDeck().discardPile.Peek());

        // using (var stream = File.Open(@".\Documentation\AverageNumberOfActionsStackingPlayer.csv", FileMode.Append))
        // using (var writer = new StreamWriter(stream))
        // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        // {
        //     if (moves.Count == 0) csv.WriteRecord(0);
        //     else csv.WriteRecord(moves.Count());
        //     csv.NextRecord();
        // }
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moves.Distinct();
        moves.Sort((x, y) => x.Count().CompareTo(y.Count()));
        return moves.Last();
    }

    public void addCardsToHand(List<Card> cards)
    {
        this.hand.AddRange(cards);
    }

    public List<Card> getHand()
    {
        return hand;
    }

    public string getName()
    {
        return name;
    }

    public void removeCardFromHand(Card cards)
    {
        hand.Remove(cards);
    }
    public List<List<Card>> getStackingActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
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
                    var nextHand = new List<Card>(hand);
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