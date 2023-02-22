using static CardColor;
using static CardType;
[Serializable]
public class TestPlayer : Iplayer
{

    List<Card> hand = new List<Card>();

    public TestPlayer(List<Card> hand)
    {
        this.hand = hand;
    }

    public TestPlayer() { }

    public List<Card> action(IgameState gameState)
    {
        var moves = this.getActions(gameState.getDeck().discardPile.Peek());
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moves = new List<List<Card>>(moves.Distinct());
        return moves[0];
    }
    public List<List<Card>> getActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(this.hand))
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
                else
                {
                    moves.Add(new List<Card>() { card });
                }


            }
        }
        return moves;
    }
    public void addCardsToHand(List<Card> cards)
    {
        throw new NotImplementedException();
    }

    public List<Card> getHand()
    {
        throw new NotImplementedException();
    }

    public string getName()
    {
        return "TestBot";
    }

    public void removeCardFromHand(Card cards)
    {
        throw new NotImplementedException();
    }
}
