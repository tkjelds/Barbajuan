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

    public Deck(){
        this.drawPile = new Stack<Card>();
        this.discardPile = new Stack<Card>();
    }

    public void setup(){
        var deck = new List<Card>();
        for (int i = 0; i < 4; i++)
        {
            deck.Add(new Card((CardColor) i, CardType.ZERO));
        }
        for (int i = 1; i < 13; i++)
        {
            deck.Add(new Card(CardColor.BLUE, (CardType) i));
            deck.Add(new Card(CardColor.RED, (CardType) i));
            deck.Add(new Card(CardColor.GREEN, (CardType) i));
            deck.Add(new Card(CardColor.YELLOW, (CardType) i));
            deck.Add(new Card(CardColor.BLUE, (CardType) i));
            deck.Add(new Card(CardColor.RED, (CardType) i));
            deck.Add(new Card(CardColor.GREEN, (CardType) i));
            deck.Add(new Card(CardColor.YELLOW, (CardType) i));
        }
        for (int i = 0; i < 4; i++)
        {
            deck.Add(new Card(CardColor.WILD, CardType.DRAW4));
            deck.Add(new Card(CardColor.WILD, CardType.WILD));
        }
        this.drawPile = new Stack<Card>(deck);
        Shuffle();
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
        this.drawPile = new Stack<Card>(list);
        this.discardPile = new Stack<Card>();
        this.discardPile.Push(this.drawPile.Pop());
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

    public int deckCount()
    {
        return this.drawPile.Count() + this.discardPile.Count();
    }
}