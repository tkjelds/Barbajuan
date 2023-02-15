public class Deck
{
    public Stack<Card> drawPile;
    public Stack<Card> discardPile;

    private static Random rng = new Random();

    public Deck(Stack<Card> DrawPile, Stack<Card> DiscardPile)
    {
        this.drawPile = DrawPile;
        this.discardPile = DiscardPile;
    }

    public Stack<Card> getDrawPile()
    {
        return this.drawPile!;
    }

    public Stack<Card> getDiscardPile()
    {
        return this.discardPile!;
    }

    public void popTopDrawPushDiscard()
    {
        var card = this.drawPile.Pop();
        discardPile.Push(card);
    }

    public Card getTopCard()
    {
        return this.discardPile.Peek();
    }
    public bool needsShuffle(int n)
    {
        return this.drawPile.Count < n ? true : false;
    }
    public void Shuffle()
    {
        var list = new List<Card>();
        list.AddRange(this.discardPile.ToList());
        list.AddRange(this.drawPile.ToList());
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
            cards.Add(this.drawPile.Pop());
        }
        return cards;
    }
    public List<Card> draw()
    {
        var cards = new List<Card>();
        if (needsShuffle(1)) Shuffle();
        cards.Add(this.drawPile.Pop());
        return cards;
    }

    public int deckCount(){
        return this.drawPile.Count() + this.discardPile.Count();
    }
}