class Hand
{
    private List<Card> cards;

    public Hand(List<Card> cards) => Cards = cards;

    internal List<Card> Cards { get => cards; set => cards = value; }
}