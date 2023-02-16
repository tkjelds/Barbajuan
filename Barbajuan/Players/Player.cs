public abstract class Player : Iplayer
{
    public List<Card> hand;

    protected Player(List<Card> hand)
    {
        this.hand = hand;
    }

    public Card action(IgameState gameState)
    {
        var moves = getActions(gameState);
        if (moves.Count() == 0) return new Card(CardColor.WILD,CardType.DRAW1);
        return moves[0];
    }

    public List<Card> getActions(IgameState gameState)
    {
        List<Card> moves = new List<Card>();
        foreach (Card card in hand)
        {
            if(card.canBePlayedOn(gameState.getDeck().discardPile.Peek())) moves.Add(card);
        }
        return moves;
    }
}