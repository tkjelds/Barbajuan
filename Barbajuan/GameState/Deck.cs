[Serializable]
public class Deck
{
    public Stack<Card> drawPile;
    public Stack<Card> discardPile;

    private static readonly Random rng = new();

    public Deck(Stack<Card> DrawPile, Stack<Card> DiscardPile)
    {
        this.drawPile = DrawPile;
        this.discardPile = DiscardPile;
    }

    public Deck()
    {
        this.drawPile = new Stack<Card>();
        this.discardPile = new Stack<Card>();
    }

    public void setup()
    {
        var deck = new List<Card>();
        for (var i = 0; i < 4; i++)
        {
            deck.Add(new Card((CardColor)i, CardType.ZERO));
        }
        for (var i = 1; i < 13; i++)
        {
            deck.Add(new Card(CardColor.BLUE, (CardType)i));
            deck.Add(new Card(CardColor.RED, (CardType)i));
            deck.Add(new Card(CardColor.GREEN, (CardType)i));
            deck.Add(new Card(CardColor.YELLOW, (CardType)i));
            deck.Add(new Card(CardColor.BLUE, (CardType)i));
            deck.Add(new Card(CardColor.RED, (CardType)i));
            deck.Add(new Card(CardColor.GREEN, (CardType)i));
            deck.Add(new Card(CardColor.YELLOW, (CardType)i));
        }
        for (var i = 0; i < 4; i++)
        {
            deck.Add(new Card(CardColor.WILD, CardType.DRAW4));
            deck.Add(new Card(CardColor.WILD, CardType.SELECTCOLOR));
        }
        this.drawPile = new Stack<Card>(deck);
        this.Shuffle();
    }

    public void popTopDrawPushDiscard()
    {
        var card = this.drawPile.Pop();
        this.discardPile.Push(card);
    }

    public Card getTopCard() => this.discardPile.Peek();
    public bool needsShuffle(int n) => this.drawPile.Count < n;
    
    public void ShuffleDrawPile(){
        var list = new List<Card>();
        list.AddRange(this.drawPile.ToList());
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
        this.drawPile = new Stack<Card>(list);
    }
    
    public void Shuffle()
    {
        var list = new List<Card>();
        list.AddRange(this.discardPile.ToList());
        list.AddRange(this.drawPile.ToList());
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
        this.drawPile = new Stack<Card>(list);
        this.discardPile = new Stack<Card>();
        this.discardPile.Push(this.drawPile.Pop());
    }

    public List<Card> draw(int n)
    {
        var cards = new List<Card>();
        for (var i = 0; i < n; i++)
        {
            if (this.needsShuffle(1))
            {
                this.Shuffle();
            }

            cards.Add(this.drawPile.Pop());
        }
        return cards;
    }
    public List<Card> draw()
    {
        var cards = new List<Card>();
        if (this.needsShuffle(1))
        {
            this.Shuffle();
        }

        cards.Add(this.drawPile.Pop());
        return cards;
    }

    public int deckCount() => this.drawPile.Count + this.discardPile.Count;

    

    public Deck Clone()
    {
        var clonedDrawPile = new Stack<Card>();
        var clonedDiscardPile = new Stack<Card>();

        foreach (var card in this.drawPile)
        {
            clonedDrawPile.Push(card.Clone());
        }
        //clonedDrawPile.Reverse();

        foreach (var card in this.discardPile)
        {
            clonedDiscardPile.Push(card.Clone());
        }

        //clonedDiscardPile.Reverse();
        return new Deck(clonedDrawPile,clonedDiscardPile);
    }

}
