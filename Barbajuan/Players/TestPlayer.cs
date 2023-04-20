using static CardColor;
using static CardType;

public class TestPlayer : Iplayer
{

    String? name;

    List<Card> hand = new List<Card>();

    public TestPlayer(List<Card> hand)
    {
        this.hand = hand;
    }

    public TestPlayer() { }

    public TestPlayer(string name, List<Card> hand)
    {
        this.name = name;
        this.hand = hand;
    }

    public List<Card> Action(GameState gameState)
    {
        var moves = this.GetActions(gameState.GetDeck().discardPile.Peek());
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(WILD, DRAW1) };
        }
        moves = new List<List<Card>>(moves.Distinct());
        return moves[0];
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
    public void AddCardsToHand(List<Card> cards)
    {
        throw new NotImplementedException();
    }

    public List<Card> GetHand()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        return "TestBot";
    }

    public void RemoveCardFromHand(Card cards)
    {
        throw new NotImplementedException();
    }
    public Iplayer Clone()
    {
        //cursed 
        var clonedHand = new List<Card>();
        foreach(var card in this.hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new TestPlayer(this.name,clonedHand);
        return clonedPlayer;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        var legalMoves = GetActions(topCard);
        if(legalMoves.Count == 0 ) return new List<List<Card>>() { new List<Card>(){new Card(WILD, DRAW1)} };
        legalMoves.Distinct();
        return legalMoves;
    }
}
