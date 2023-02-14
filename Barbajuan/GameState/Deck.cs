public class Deck
{
    public static Stack<Card>? drawPile;
    public static Stack<Card>? discardPile;

    private static Random rng = new Random();

    public Stack<Card> getDrawPile()
    {
        return drawPile!;
    }

    public Stack<Card> getDiscardPile()
    {
        return discardPile!;
    }

    public Deck(Stack<Card> DrawPile, Stack<Card> DiscardPile)
    {
        drawPile = DrawPile;
        discardPile = DiscardPile;
    }

    public void popTopDrawPushDiscard()
    {
        var card = drawPile.Pop();
        discardPile.Push(card);
    }

    public Card getTopCard()
    {
        return discardPile.Peek();
    }
    public bool needsShuffle(int n)
    {
        return drawPile.Count < n ? true : false;
    }
    public void Shuffle()
    {
        var list = new List<Card>();
        list.AddRange(discardPile.ToList());
        list.AddRange(drawPile.ToList());
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        drawPile = new Stack<Card>(list);
        discardPile = new Stack<Card>();
    }

    public List<Card> draw(int n)
    {
        var cards = new List<Card>();
        for (int i = 0; i < n; i++)
        {
            if (needsShuffle(1)) Shuffle();
            cards.Add(drawPile.Pop());
        }
        return cards;
    }
    public List<Card> draw()
    {
        var cards = new List<Card>();
        if (needsShuffle(1)) Shuffle();
        cards.Add(drawPile.Pop());
        return cards;
    }

    // public List<Card> dealSeven()
    // {
    //     List<Card> hand = new List<Card> { };
    //     for (int i = 0; i < 6; i++)
    //     {
    //         hand.Add(this.cards.Pop());
    //     }
    //     return hand;
    // }
    // public List<Card> DrawN(int n)
    // {
    //     List<Card> hand = new List<Card> { };
    //     for (int i = 0; i < n - 1; i++)
    //     {
    //         hand.Add(this.cards.Pop());
    //     }
    //     return hand;
    // }
}