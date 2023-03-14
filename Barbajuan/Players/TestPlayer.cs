using static CardColor;
using static CardType;

public class TestPlayer : Iplayer
{

    String Name;

    List<Card> Hand = new List<Card>();

    public TestPlayer(List<Card> Hand)
    {
        this.Hand = Hand;
    }

    public TestPlayer() { }

    public TestPlayer(string name, List<Card> hand)
    {
        Name = name;
        Hand = hand;
    }

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
        foreach (var card in new List<Card>(this.Hand))
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
    public Iplayer clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new TestPlayer(this.Name,clonedHand);
        return clonedPlayer;
    }
}
