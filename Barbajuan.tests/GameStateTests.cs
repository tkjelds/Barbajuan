namespace Barbajuan.tests;

using static CardColor;
using static CardType;

public class GameStateTests
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
    public void NextPlayerReturnsCorrectPlayer()
    {
        //given
        var deck = new Deck();
        deck.setup();
        var Player1 = new TestPlayer(deck.draw(7));
        var Player2 = new TestPlayer(deck.draw(7));
        var Player3 = new TestPlayer(deck.draw(7));
        var gs = new GameState(new List<Iplayer>() { Player1, Player2, Player3 }, deck);
        //when 
        var actual = gs.GetPlayers()[gs.nextPlayer(gs.getCurrentPlayerIndex(), gs)];
        //then
        Assert.Equal(Player2, actual);
    }

    [Fact]
    public void NextPlayerReturnsCorrectPlayerWithWrapAround()
    {
        // Given
        var deck = new Deck();
        deck.setup();
        var player1 = new TestPlayer(deck.draw(7));
        var player2 = new TestPlayer(deck.draw(7));
        var player3 = new TestPlayer(deck.draw(7));
        var gs = new GameState(player3, 2, new List<Iplayer>() { player1, player2, player3 }, deck);
        // When
        var actual = gs.GetPlayers()[gs.nextPlayer(gs.getCurrentPlayerIndex(), gs)];
        // Then
        Assert.Equal(player1, actual);
    }

    [Fact]
    public void NextPlayerReturnsExpectedWhenCounterClockwisePlayDirection()
    {
        // Given
        var deck = new Deck();
        deck.setup();
        var player1 = new TestPlayer(deck.draw(7));
        var player2 = new TestPlayer(deck.draw(7));
        var player3 = new TestPlayer(deck.draw(7));
        var GameState = new GameState(new List<Iplayer>() { player1, player2, player3 }, deck, false);
        // When
        var actual = GameState.GetPlayers()[GameState.nextPlayer()];
        // Then
        Assert.Equal(player3, actual);
    }
    [Fact]
    public void IsGameOverOnePlayer()
    {
        var players = new List<Iplayer>();
        var hand = new List<Card>();
        players.Add(new RandomPlayer(hand));

        var GameState = new GameState(players);

        // When

        var actual = GameState.IsGameOver();

        // Then
        Assert.True(actual);
    }

    [Fact]
    public void IsGameOverMultiplePlayers()
    {
        // Given
        var players = new List<Iplayer>();

        for (var i = 0; i < 10; i++)
        {
            players.Add(new RandomPlayer(new List<Card>()));
        }

        var GameState = new GameState(players);
        // When

        var actual = GameState.IsGameOver();

        // Then
        Assert.False(actual);
    }

    [Fact(Skip = "No playes added")]
    public void ApplyTests()
    {
        // Given
        var players = new List<Iplayer>();


        var GameState = new GameState(players);
        // When

        var actual = GameState.IsGameOver();

        // Then
        Assert.True(actual);
    }
    /*
    Gamestate 
        CurrentPlayer = 0
        NextPlayer = 1

    No Skip NextGameState 
        CurrentPlayer = 1
        NextPlayer = 2

    With Skip NextGameState 
        CurrentPlayer = 2
        NextPlayer = 3
    */
    [Fact]
    public void SkipCardApplyOnGameState()
    {

        // Given
        var players = new List<Iplayer>(){
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
        };
        var gamestate = new GameState(players);
        // When

        var actual = gamestate.apply(new List<Card>() { new Card(CardColor.YELLOW, CardType.SKIP) });
        // Then
        Assert.Equal(2, actual.getCurrentPlayerIndex());
        Assert.Equal(3, actual.getNextPlayerIndex());
    }
    [Fact]
    public void DoubleSkipCardApplyOnGameState()
    {

        // Given
        var players = new List<Iplayer>(){
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players);
        // When

        var actual = gamestate.apply(new List<Card>() { new Card(CardColor.YELLOW, CardType.SKIP), new Card(CardColor.BLUE, CardType.SKIP) });
        // Then
        Assert.Equal(1, actual.getNextPlayerIndex());
        Assert.Equal(0, actual.getCurrentPlayerIndex());
    }
    [Fact]
    public void SkipOnReverseDirectionGameState()
    {
        // Given
        var players = new List<Iplayer>(){
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        // When
        var gamestate = new GameState(players, new Deck(generateCards(10), generateCards(10)), false);
        var actual = gamestate.apply(new List<Card>() { new Card(CardColor.BLUE, CardType.SKIP) });
        // Then
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(0, actual.getNextPlayerIndex());
    }

    [Fact]
    public void DoubleSkipOnReverseDirectionGameState()
    {
        var firstPlayerHand = new List<Card>(){
            new Card(BLUE, SKIP),
            new Card(BLUE, SKIP),
            new Card(RED, ZERO)

        };
        // Given
        var players = new List<Iplayer>(){
            new RandomPlayer(firstPlayerHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        // When
        var gamestate = new GameState(players, new Deck(generateCards(10), generateCards(10)), false);
        var actual = gamestate.apply(new List<Card> { new Card(BLUE, SKIP), new Card(BLUE, SKIP) });
        // Then
        Assert.Single(actual.GetPlayers()[0].getHand());
        Assert.Equal(12, actual.getDeck().discardPile.Count());
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(0, actual.getNextPlayerIndex());
    }

    [Fact]
    public void ApplyDraw4OnGame()
    {
        var firstPlayeHand = new List<Card>{
            new Card(WILD,DRAW4),
            new Card(RED,ZERO),
            new Card(RED,ZERO),
            new Card(RED,ZERO)
        };
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayeHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players, new Deck(generateCards(10), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(WILD, DRAW4) });
        // Then
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(8, actual.GetPlayers()[1].getHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.getCurrentPlayerIndex());
    }

    [Fact]
    public void ApplyDraw2OnGame()
    {
        // Given
        var firstPlayerHand = new List<Card>(){
            new Card(RED, DRAW2),
            new Card(BLUE, ONE),
            new Card(RED, TWO),
            new Card(GREEN, THREE)

        };

        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayerHand),
            new RandomPlayer(generateCards(7).ToList()),
            new RandomPlayer(generateCards(7).ToList()),
            new RandomPlayer(generateCards(7).ToList())
        };

        var gamestate = new GameState(players, new Deck(generateCards(10), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(RED, DRAW2) });
        // Then
        //Check if player 1 has played a card from hand
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        //Check if player 2 is skipped correctly
        Assert.Equal(2, actual.getCurrentPlayerIndex());
        //Check if player 2 has drawn the 2 cards
        Assert.Equal(9, actual.GetPlayers()[1].getHand().Count());
    }
    [Fact]
    public void ApplyTwoDraw4OnGame()
    {
        var firstPlayeHand = new List<Card>{
            new Card(WILD,DRAW4),
            new Card(WILD,DRAW4),
            new Card(RED,ZERO),
            new Card(RED,ZERO),
            new Card(RED,ZERO)
        };
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayeHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players, new Deck(generateCards(10), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(WILD, DRAW4), new Card(WILD, DRAW4) });
        // Then
        // Check to see if the original gamestate is intact.
        Assert.Equal(0, gamestate.getCurrentPlayerIndex());
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(12, actual.GetPlayers()[1].getHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.getCurrentPlayerIndex());
    }
    [Fact]
    public void ApplyTwoDraw4OnGameAndReshuffle()
    {
        var firstPlayeHand = new List<Card>{
            new Card(WILD,DRAW4),
            new Card(WILD,DRAW4),
            new Card(RED,ZERO),
            new Card(RED,ZERO),
            new Card(RED,ZERO)
        };
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayeHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players, new Deck(generateCards(2), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(WILD, DRAW4), new Card(WILD, DRAW4) });
        // Then
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(12, actual.GetPlayers()[1].getHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.getCurrentPlayerIndex());
        // Check remainingCard in Deck
        Assert.Equal(3, actual.getDeck().drawPile.Count());
        // Check Correct number of cards in discard
        Assert.Equal(3, actual.getDeck().discardPile.Count());
    }

    [Fact]
    public void ApplyReverseCardToGameState()
    {
        var firstPlayeHand = new List<Card>{
            new Card(RED,REVERSE),
            new Card(WILD,DRAW4),
            new Card(RED,ZERO),
            new Card(RED,ZERO),
            new Card(RED,ZERO)
        };
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayeHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players, new Deck(generateCards(2), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(RED, REVERSE) });
        var expected = new Card(RED, REVERSE);
        // Then
        Assert.Equal(gamestate.getDeck().discardPile.Count() + 1, actual.getDeck().discardPile.Count());
        Assert.Equal(3, actual.getCurrentPlayerIndex());
        Assert.Equal(2, actual.getNextPlayerIndex());
        Assert.Equal(4, actual.GetPlayers()[0].getHand().Count());
        expected.Should().BeEquivalentTo(actual.getDeck().discardPile.Peek());

    }
    [Fact]
    public void ApplyDoubleReverseCardToGameState()
    {
        var firstPlayeHand = new List<Card>{
            new Card(RED,REVERSE),
            new Card(RED,REVERSE),
            new Card(WILD,DRAW4),
            new Card(RED,ZERO),
            new Card(RED,ZERO),
            new Card(RED,ZERO)
        };
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayeHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players, new Deck(generateCards(2), generateCards(10)));
        // When
        var actual = gamestate.apply(new List<Card> { new Card(RED, REVERSE), new Card(RED, REVERSE) });
        var expected = new Card(RED, REVERSE);
        // Then
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(2, actual.getNextPlayerIndex());
        Assert.Equal(4, actual.GetPlayers()[0].getHand().Count());
        expected.Should().BeEquivalentTo(actual.getDeck().discardPile.Peek());
    }

    [Fact]
    public void AppyDraw1OnGameState()
    {
        // Given
        var players = new List<Iplayer>{
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gameState = new GameState(players, new Deck(generateCards(10), generateCards(10)));
        // When
        var actual = gameState.apply(new List<Card> { new Card(RED, DRAW1) });
        // Then
        // Player draws one card
        Assert.Equal(5, actual.GetPlayers()[0].getHand().Count());
        // Next player is updated
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(2, actual.getNextPlayerIndex());

        // The card is drawn from draw pile and the discard pile is untouched
        Assert.Equal(9, actual.getDeck().drawPile.Count());
        Assert.Equal(10, actual.getDeck().discardPile.Count());

    }

    [Theory]
    [InlineData(0, 0)] // BLUE ZERO 
    [InlineData(1, 1)] // GREEN ONE
    [InlineData(2, 2)] // RED TWO 
    [InlineData(3, 3)] // YELLOW THREE
    [InlineData(0, 4)] // BLUE FOUR
    [InlineData(1, 5)] // GREEN FIVE
    [InlineData(2, 6)] // RED SIX
    [InlineData(3, 7)] // YELLOW SEVEN 
    [InlineData(0, 8)] // BLUE EIGHT
    [InlineData(1, 9)] // GREEN NINE
    [InlineData(2, 0)] // RED ZERO
    [InlineData(3, 1)] // YELLOW ONE
    public void NormalCardApply(int cardColor, int cardType)
    {
        // Given
        var card = new Card((CardColor)cardColor, (CardType)cardType);
        var firstPlayerHand = new List<Card>{
            card,
            new Card(RED, DRAW2),
            new Card(RED, DRAW2),
            new Card(RED, DRAW2)
        };
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayerHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players);
        // When
        var actual = gamestate.apply(new List<Card> { card });
        var expected = card;
        // Then
        Assert.Equal(gamestate.getDeck().discardPile.Count() + 1, actual.getDeck().discardPile.Count());
        expected.Should().BeEquivalentTo(actual.getDeck().discardPile.Peek());
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(2, actual.getNextPlayerIndex());
    }

    [Theory]
    [InlineData(0, 0, 0, 0)] // BLUE ZERO 
    [InlineData(1, 1, 1, 1)] // GREEN ONE
    [InlineData(2, 2, 2, 2)] // RED TWO 
    [InlineData(3, 3, 3, 3)] // YELLOW THREE
    [InlineData(0, 4, 0, 4)] // BLUE FOUR
    [InlineData(1, 5, 1, 5)] // GREEN FIVE
    [InlineData(2, 6, 2, 6)] // RED SIX
    [InlineData(3, 7, 3, 7)] // YELLOW SEVEN 
    [InlineData(0, 8, 0, 8)] // BLUE EIGHT
    [InlineData(1, 9, 1, 9)] // GREEN NINE
    [InlineData(2, 0, 2, 0)] // RED ZERO
    [InlineData(3, 1, 3, 1)] // YELLOW ONE
    public void TwoNormalCardsApply(int cardColor1, int cardType1, int cardColor2, int cardType2)
    {
        // Given
        var card1 = new Card((CardColor)cardColor1, (CardType)cardType1);
        var card2 = new Card((CardColor)cardColor2, (CardType)cardType2);
        var firstPlayerHand = new List<Card>{
            card1,
            card2,
            new Card(RED, DRAW2),
            new Card(RED, DRAW2),
            new Card(RED, DRAW2)
        };
        var players = new List<Iplayer>{
            new RandomPlayer(firstPlayerHand),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList()),
            new RandomPlayer(generateCards(4).ToList())
        };
        var gamestate = new GameState(players);
        Assert.Single(gamestate.getDeck().discardPile);
        // When
        var actual = gamestate.apply(new List<Card> { card1, card2 });
        var expected = card2;
        // Then
        Assert.Equal(5, gamestate.getCurrentPlayer().getHand().Count());
        expected.Should().BeEquivalentTo(actual.getDeck().discardPile.Peek());
        Assert.Equal((gamestate.getDeck().discardPile.Count() + 2), actual.getDeck().discardPile.Count());
        Assert.NotEqual(gamestate.getCurrentPlayerIndex(), actual.getCurrentPlayerIndex());
        Assert.NotEqual(gamestate.getNextPlayerIndex(), actual.getNextPlayerIndex());
        Assert.Equal(3, actual.GetPlayers()[0].getHand().Count());
        Assert.Equal(1, actual.getCurrentPlayerIndex());
        Assert.Equal(2, actual.getNextPlayerIndex());
    }
}
