using static CardColor;
using static CardType;
[Serializable]
public class Player : Iplayer
{
    public List<Card> Hand;

    public string name = "Unassigned";

    public Player(List<Card> Hand) => this.Hand = Hand;

    public Player(string name)
    {
        this.Hand = new List<Card>();
        this.name = name;
    }

    public Player(Player player)
    {
        Hand = player.Hand;
    }

    public Player(List<Card> Hand, string name)
    {
        this.name = name;
        this.Hand = Hand;
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
    public List<Card> getHand()
    {
        return Hand;
    }

    public void removeCardFromHand(Card cards)
    {
        Hand.Remove(cards);
    }

    public string getName()
    {
        return name;
    }

    public void addCardsToHand(List<Card> cards)
    {
        this.Hand.AddRange(cards);
    }

    public Iplayer clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new Player(clonedHand,this.name);
        return clonedPlayer;
    }
}
