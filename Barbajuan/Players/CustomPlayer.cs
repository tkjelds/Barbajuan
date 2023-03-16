
class CustomPlayer : Iplayer
{

    List<Card> Hand = new List<Card>();
    string Name = "";

    readonly ImovePicker MovePicker;

    public CustomPlayer(List<Card> hand, string name, ImovePicker movePicker)
    {
        Hand = hand;
        Name = name;
        MovePicker = movePicker;
    }

    public List<Card> action(IgameState gameState)
    {
        var action = MovePicker.pick((GameState)gameState);
        return action;
    }

    public void addCardsToHand(List<Card> cards)
    {
        Hand.AddRange(cards);
    }

    public List<Card> getHand()
    {
        return this.Hand;
    }

    public string getName()
    {
        return this.Name;
    }

    public void removeCardFromHand(Card cards)
    {
        this.Hand.Remove(cards);
    }

    public Iplayer clone()
    {
        var clonedHand = new List<Card>();

        foreach (var card in Hand)
        {
            clonedHand.Add(card.Clone());
        }

        return new CustomPlayer(clonedHand, this.Name,this.MovePicker);
    }

    List<List<Card>> Iplayer.getLegalMoves(Card topCard)
    {
        return MovePicker.getLegalMoves(topCard,Hand);
    }
}