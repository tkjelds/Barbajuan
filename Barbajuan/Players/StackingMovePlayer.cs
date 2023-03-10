using static CardColor;
using static CardType;
[Serializable]
public class StackingMovePlayer : Iplayer
{
    String Name;
    List<Card> Hand = new List<Card>();

    public StackingMovePlayer(String Name) => this.Name = Name;
    public StackingMovePlayer(String Name, List<Card> Hand)
    {
        this.Name = Name;
        this.Hand = Hand;
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
    public List<List<Card>> getCardOfSameType(Card toBePlayedOn, List<Card> Hand, List<Card> currentStack)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(Hand))
        {
            var nextHand = new List<Card>(Hand);
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
        public Iplayer clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new StackingMovePlayer(this.Name,clonedHand);
        return clonedPlayer;
    }
}