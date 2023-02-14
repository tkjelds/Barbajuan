
class Deck
{
    Stack<Card> cards;

    public Deck(Stack<Card> cards)
    {
        this.cards = cards;
    }

    public List<Card> dealSeven()
    {
        List<Card> hand = new List<Card> { };
        for (int i = 0; i < 6; i++)
        {
            hand.Add(this.cards.Pop());
        }
        return hand;
    }
    public List<Card> DrawN(int n)
    {
        List<Card> hand = new List<Card> { };
        for (int i = 0; i < n - 1; i++)
        {
            hand.Add(this.cards.Pop());
        }
        return hand;
    }
}