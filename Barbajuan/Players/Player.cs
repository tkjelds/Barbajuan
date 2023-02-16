using static CardColor;
using static CardType;
public abstract class Player : Iplayer
{
    public List<Card> hand;

    protected Player(List<Card> hand) => this.hand = hand;

    public List<Card> action(IgameState gameState)
    {
        var moves = this.getActions(gameState.getDeck().discardPile.Peek());
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moves = (List<List<Card>>)moves.Distinct();
        return moves[0];
    }

    public List<List<Card>> getActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in this.hand)
        {
            if (card.canBePlayedOn(topCard))
            {
                if (card.cardType == DRAW4){
                moves.Add(new List<Card>(){new Card(GREEN,DRAW4)});
                moves.Add(new List<Card>(){new Card(BLUE,DRAW4)});
                moves.Add(new List<Card>(){new Card(YELLOW,DRAW4)});
                moves.Add(new List<Card>(){new Card(RED,DRAW4)});
                }
                if(card.cardType == SELECTCOLOR){
                moves.Add(new List<Card>(){new Card(GREEN,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(BLUE,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(YELLOW,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(RED,SELECTCOLOR)});
                } else
                {
                    moves.Add(new List<Card>() { card }); 
                }
                

            }
        }
        return moves;
    }

    public List<List<Card>> getStackingActions(Card topCard)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
        {
            if (card.canBePlayedOn(topCard))
            {
                if (card.cardType == DRAW4){
                moves.Add(new List<Card>(){new Card(GREEN,DRAW4)});
                moves.Add(new List<Card>(){new Card(BLUE,DRAW4)});
                moves.Add(new List<Card>(){new Card(YELLOW,DRAW4)});
                moves.Add(new List<Card>(){new Card(RED,DRAW4)});
                }
                if(card.cardType == SELECTCOLOR){
                moves.Add(new List<Card>(){new Card(GREEN,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(BLUE,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(YELLOW,SELECTCOLOR)});
                moves.Add(new List<Card>(){new Card(RED,SELECTCOLOR)});
                } if(card.cardType != SELECTCOLOR && card.cardType != DRAW4) {
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

    List<Card> Iplayer.getActions(Card topCard) => throw new NotImplementedException();
    Card Iplayer.action(IgameState gameState) => throw new NotImplementedException();
}
