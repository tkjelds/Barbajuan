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

    public List<List<Card>> getStackingActions(IgameState gameState){
        List<List<Card>> moves = new List<List<Card>>();
        Card toBePlayedOn = gameState.getDeck().discardPile.Peek();
        foreach (Card card in hand)
        {   
            if(card.canBePlayedOn(toBePlayedOn)){
                var tempHand = hand;
                moves.Add(new List<Card>(){card});
                tempHand.Remove(card);
                moves.AddRange(getStackingMoves(card,tempHand, new List<Card>(){card}));
            }
        }
        return moves;
    }

    public List<List<Card>> getStackingMoves(Card toBePlayedOn, List<Card> hand, List<Card> currentStack){
        List<List<Card>> moves = new List<List<Card>>();
        foreach (Card card in hand)
        {
            var tempHand = hand;
            if(card.canBePlayedOn(toBePlayedOn)){
                var tempStack = currentStack;
                tempStack.Add(card);
                moves.Add(tempStack);
                tempHand.Remove(card);
                moves.AddRange(getStackingMoves(card, tempHand, tempStack));

            }
        }
        return moves;
    }
}