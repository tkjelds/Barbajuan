public class StackingMovePlayer : Iplayer
{
    String name;
    List<Card> hand = new List<Card>();

    ImovePicker movePicker = new StackingMovePicker();

    public StackingMovePlayer(String name) => this.name = name;
    public StackingMovePlayer(String name, List<Card> hand)
    {
        this.name = name;
        this.hand = hand;
    }

    public List<Card> Action(GameState gameState)
    {

        return movePicker.Pick(gameState);
    }

    public void AddCardsToHand(List<Card> cards)
    {
        this.hand.AddRange(cards);
    }

    public List<Card> GetHand()
    {
        return hand;
    }

    public string GetName()
    {
        return name;
    }

    public void RemoveCardFromHand(Card cards)
    {
        hand.Remove(cards);
    }

    public Iplayer Clone()
    {
        var clonedHand = new List<Card>();
        foreach (var card in this.hand)
        {
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new StackingMovePlayer(this.name, clonedHand);
        return clonedPlayer;
    }

    public List<List<Card>> GetLegalMoves(Card topCard)
    {
        var legalMoves = movePicker.GetLegalMoves(topCard, hand);
        if (legalMoves.Count == 0) return new List<List<Card>>() { new List<Card>() { new Card(WILD, DRAW1) } };
        legalMoves.Distinct();
        return legalMoves;
    }
}