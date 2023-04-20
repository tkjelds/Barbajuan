namespace Barbajuan.tests;

public class DeckTests
{
    public static Random rng = new();
    public static Stack<Card> generateCards(int n)
    {
        var Stack = new Stack<Card>();
        for (var i = 0; i < n; i++)
        {
            Stack.Push(new Card((CardColor)rng.Next(3), (CardType)rng.Next(12)));
        }
        return Stack;
    }

    [Fact]
    public void TestNeedShuffleTrue()
    {
        var deck = new Deck(generateCards(5), new Stack<Card>());

        Assert.True(deck.NeedsShuffle(10));
    }

    [Fact]
    public void TestNeedShuffleFalse()
    {
        // Given
        var deck = new Deck(generateCards(10), new Stack<Card>());
        // When

        // Then
        Assert.False(deck.NeedsShuffle(2));
    }

    [Fact]
    public void ShuffleReturnsSameSizeDeck()
    {
        // Given
        var deck = new Deck(generateCards(10), generateCards(5));
        // Then
        var deckSize = deck.discardPile.Count + deck.drawPile.Count;

        Assert.Equal(15, deckSize);
    }
    [Fact]
    public void ShuffleReturnsShuffled()
    {
        // Given
        var deck = new Deck(generateCards(100), generateCards(0));
        // When
        deck.PopTopDrawPushDiscard();
        var preShufflePeek = deck.GetTopCard();
        deck.Shuffle();
        deck.PopTopDrawPushDiscard();
        var postShufflePeek = deck.GetTopCard();
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

    [Fact]
    public void Draw4CardsWhenDrawPileIs3()
    {
        // Given
        var deck = new Deck(generateCards(3), generateCards(2));
        // When
        var actual = deck.Draw(4);
        // Then
        Assert.Equal(4, actual.Count);
    }
    [Fact]
    public void DeckShufflesCorrectly()
    {
        // Given
        var deck = new Deck(generateCards(1), generateCards(10));
        // When
        deck.Draw(2);
        // Then
        Assert.Equal(8, deck.drawPile.Count);
    }
    [Fact]
    public void DeckShufflesCorrectlyAndPlacesTopDrawCardOntoDiscardPile()
    {
        // Given
        var deck = new Deck(generateCards(1), generateCards(10));
        // When
        deck.Draw(2);
        // Then
        Assert.Single(deck.discardPile);
    }

    [Fact]
    public void DeckHas108CardsAfterSetUp()
    {
        // Given
        var deck = new Deck();
        // When
        deck.Setup();
        // Then
        Assert.Equal(108, deck.DeckCount());
    }

    [Fact]
    public void DeckSetupGivesFirstCardInDiscardPile()
    {
        // Given
        var deck = new Deck();
        // When
        deck.Setup();
        // Then
        Assert.Single(deck.discardPile);
    }

    [Fact]
    public void ShuffleDrawPileTest(){
        var deck = new Deck(generateCards(100),generateCards(100));
        var topCardOfOriginalDeck = deck.drawPile.Peek();
        
        
        deck.ShuffleDrawPile();

        var actual = deck.drawPile.Peek();

        Assert.Equal(100,deck.drawPile.Count());
        Assert.NotEqual(topCardOfOriginalDeck,actual);
    }

    [Fact]
    public void ShuffleDrawPileTestDoesNotAffectDiscard(){
        var deck = new Deck(generateCards(100),generateCards(100));
        var topCardOfOriginalDiscardPile = deck.discardPile.Peek();
        
        
        deck.ShuffleDrawPile();

        var actual = deck.discardPile.Peek();

        Assert.Equal(100,deck.discardPile.Count());
        Assert.Equal(topCardOfOriginalDiscardPile,actual);
    }

    [Fact]
    public void ClonedDeckIsDifferentObject()
    {
        // Given
        var deck = new Deck(generateCards(20), generateCards(20));
        // When
        var actual = deck.Clone();
        // Then
        // Different REFs for object
        Assert.NotEqual(deck,actual);
        // Same of total amount of cards
        Assert.Equal(deck.DeckCount(),actual.DeckCount());
        // Correct Amount of cards in discard 
        Assert.Equal(deck.discardPile.Count(),actual.discardPile.Count());
        // Correct amoutn of cards in drawppile
        Assert.Equal(deck.drawPile.Count(),actual.drawPile.Count());
    }

    [Fact]
    public void ClonedDeckIsTheSameOrder()
    {
        // Given
    
        // When
    
        // Then
    }
}
