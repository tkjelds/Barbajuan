class Discard
{
    public List<Card> pile;
    public Card? topCard;

    public Discard(List<Card> pile)
    {
        this.pile = pile;
        this.topCard = pile[0];
    }

    public void push(Card card)
    {
        this.pile.Add(card);
        topCard = card;
    }
}