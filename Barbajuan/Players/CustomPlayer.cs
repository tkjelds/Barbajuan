
class CustomPlayer : Iplayer
{

    List<Card> hand = new List<Card>();
    string name = "";
    readonly ImovePicker movePicker;

    public CustomPlayer(List<Card> hand, string name, ImovePicker movePicker)
    {
        this.hand = hand;
        this.name = name;
        this.movePicker = movePicker;
    }

    public void AddCardsToHand(List<Card> cards)
    {
        hand.AddRange(cards);
    }

    public List<Card> GetHand()
    {
        return this.hand;
    }

    public string GetName()
    {
        return this.name;
    }

    public void RemoveCardFromHand(Card cards)
    {
        this.hand.Remove(cards);
    }

    public Iplayer Clone()
    {
        var clonedHand = new List<Card>();

        foreach (var card in hand)
        {
            clonedHand.Add(card.Clone());
        }

        return new CustomPlayer(clonedHand, this.name, this.movePicker);
    }

    List<List<Card>> Iplayer.GetLegalMoves(Card topCard)
    {
        return movePicker.GetLegalMoves(topCard, hand);
    }

    public List<Card> Action(GameState gameState)
    {
        return movePicker.Pick(gameState);
    }
}