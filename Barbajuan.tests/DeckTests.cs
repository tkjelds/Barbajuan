namespace Barbajuan.tests;

public class DeckTests
{
    public static Random rng = new Random();
    public Stack<Card> generateCards(int n)
    {
        var Stack = new Stack<Card>();
        for (int i = 0; i < n; i++)
        {
            Stack.Push(new Card((CardColor)rng.Next(3), (CardType)rng.Next(12)));
        }
        return Stack;
    }

    [Fact]
    public void TestNeedShuffleTrue()
    {
        var deck = new Deck(generateCards(5), new Stack<Card>());

        Assert.True(deck.needsShuffle(10));
    }

    [Fact]
    public void TestNeedShuffleFalse()
    {
        // Given
        var deck = new Deck(generateCards(10), new Stack<Card>());
        // When

        // Then
        Assert.False(deck.needsShuffle(2));
    }

    [Fact]
    public void ShuffleReturnsSameSizeDeck()
    {
        // Given
        var deck = new Deck(generateCards(10), generateCards(5));
        // Then
        var deckSize = deck.getDiscardPile().Count() + deck.getDrawPile().Count();

        Assert.Equal(15, deckSize);
    }
    [Fact]
    public void ShuffleReturnsShuffled()
    {
        // Given
        var deck = new Deck(generateCards(100), generateCards(0));
        // When
        deck.popTopDrawPushDiscard();
        var preShufflePeek = deck.getTopCard();
        deck.Shuffle();
        deck.popTopDrawPushDiscard();
        var postShufflePeek = deck.getTopCard();
        // Then
        Assert.NotEqual(preShufflePeek, postShufflePeek);
    }
    [Fact]
    public void TwoNewDecksAreNotTheSame()
    {
        // Given
        var deck1 = new Deck(generateCards(1), generateCards(0));
        var deck2 = new Deck(generateCards(2), generateCards(0));
        // When

        // Then
        Assert.NotEqual(deck1, deck2);
    }
}