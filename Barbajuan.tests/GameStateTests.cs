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
        deck.Setup();
        var Player1 = new TestPlayer(deck.Draw(7));
        var Player2 = new TestPlayer(deck.Draw(7));
        var Player3 = new TestPlayer(deck.Draw(7));
        var gs = new GameState(new List<Iplayer>() { Player1, Player2, Player3 }, deck);
        //when 
        var actual = gs.GetPlayers()[gs.NextPlayer(gs.GetCurrentPlayerIndex(), gs)];
        //then
        Assert.Equal(Player2, actual);
    }

    [Fact]
    public void NextPlayerReturnsCorrectPlayerWithWrapAround()
    {
        // Given
        var deck = new Deck();
        deck.Setup();
        var player1 = new TestPlayer(deck.Draw(7));
        var player2 = new TestPlayer(deck.Draw(7));
        var player3 = new TestPlayer(deck.Draw(7));
        var gs = new GameState(player3, 2, new List<Iplayer>() { player1, player2, player3 }, deck);
        // When
        var actual = gs.GetPlayers()[gs.NextPlayer(gs.GetCurrentPlayerIndex(), gs)];
        // Then
        Assert.Equal(player1, actual);
    }

    [Fact]
    public void NextPlayerReturnsExpectedWhenCounterClockwisePlayDirection()
    {
        // Given
        var deck = new Deck();
        deck.Setup();
        var player1 = new TestPlayer(deck.Draw(7));
        var player2 = new TestPlayer(deck.Draw(7));
        var player3 = new TestPlayer(deck.Draw(7));
        var GameState = new GameState(new List<Iplayer>() { player1, player2, player3 }, deck, false);
        // When
        var actual = GameState.GetPlayers()[GameState.NextPlayer()];
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

        var actual = gamestate.Apply(new List<Card>() { new Card(CardColor.YELLOW, CardType.SKIP) });
        // Then
        Assert.Equal(2, actual.GetCurrentPlayerIndex());
        Assert.Equal(3, actual.GetNextPlayerIndex());
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

        var actual = gamestate.Apply(new List<Card>() { new Card(CardColor.YELLOW, CardType.SKIP), new Card(CardColor.BLUE, CardType.SKIP) });
        // Then
        Assert.Equal(1, actual.GetNextPlayerIndex());
        Assert.Equal(0, actual.GetCurrentPlayerIndex());
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
        var actual = gamestate.Apply(new List<Card>() { new Card(CardColor.BLUE, CardType.SKIP) });
        // Then
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(0, actual.GetNextPlayerIndex());
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
        var actual = gamestate.Apply(new List<Card> { new Card(BLUE, SKIP), new Card(BLUE, SKIP) });
        // Then
        Assert.Single(actual.GetPlayers()[0].GetHand());
        Assert.Equal(12, actual.GetDeck().discardPile.Count());
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(0, actual.GetNextPlayerIndex());
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
        var actual = gamestate.Apply(new List<Card> { new Card(WILD, DRAW4) });
        // Then
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(8, actual.GetPlayers()[1].GetHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.GetCurrentPlayerIndex());
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
        var actual = gamestate.Apply(new List<Card> { new Card(RED, DRAW2) });
        // Then
        //Check if player 1 has played a card from hand
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        //Check if player 2 is skipped correctly
        Assert.Equal(2, actual.GetCurrentPlayerIndex());
        //Check if player 2 has drawn the 2 cards
        Assert.Equal(9, actual.GetPlayers()[1].GetHand().Count());
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
        var actual = gamestate.Apply(new List<Card> { new Card(WILD, DRAW4), new Card(WILD, DRAW4) });
        // Then
        // Check to see if the original gamestate is intact.
        Assert.Equal(0, gamestate.GetCurrentPlayerIndex());
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(12, actual.GetPlayers()[1].GetHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.GetCurrentPlayerIndex());
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
        var actual = gamestate.Apply(new List<Card> { new Card(WILD, DRAW4), new Card(WILD, DRAW4) });
        // Then
        // Check if first player card is removed from hand
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        // Check if next player has 4 more cards
        Assert.Equal(12, actual.GetPlayers()[1].GetHand().Count());
        // Check nextPlayerIsCorrect
        Assert.Equal(2, actual.GetCurrentPlayerIndex());
        // Check remainingCard in Deck
        Assert.Equal(3, actual.GetDeck().drawPile.Count());
        // Check Correct number of cards in discard
        Assert.Equal(3, actual.GetDeck().discardPile.Count());
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
        var actual = gamestate.Apply(new List<Card> { new Card(RED, REVERSE) });
        var expected = new Card(RED, REVERSE);
        // Then
        Assert.Equal(gamestate.GetDeck().discardPile.Count() + 1, actual.GetDeck().discardPile.Count());
        Assert.Equal(3, actual.GetCurrentPlayerIndex());
        Assert.Equal(2, actual.GetNextPlayerIndex());
        Assert.Equal(4, actual.GetPlayers()[0].GetHand().Count());
        expected.Should().BeEquivalentTo(actual.GetDeck().discardPile.Peek());

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
        var actual = gamestate.Apply(new List<Card> { new Card(RED, REVERSE), new Card(RED, REVERSE) });
        var expected = new Card(RED, REVERSE);
        // Then
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(2, actual.GetNextPlayerIndex());
        Assert.Equal(4, actual.GetPlayers()[0].GetHand().Count());
        expected.Should().BeEquivalentTo(actual.GetDeck().discardPile.Peek());
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
        var actual = gameState.Apply(new List<Card> { new Card(RED, DRAW1) });
        // Then
        // Player draws one card
        Assert.Equal(5, actual.GetPlayers()[0].GetHand().Count());
        // Next player is updated
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(2, actual.GetNextPlayerIndex());

        // The card is drawn from draw pile and the discard pile is untouched
        Assert.Equal(9, actual.GetDeck().drawPile.Count());
        Assert.Equal(10, actual.GetDeck().discardPile.Count());

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
        var actual = gamestate.Apply(new List<Card> { card });
        var expected = card;
        // Then
        Assert.Equal(gamestate.GetDeck().discardPile.Count() + 1, actual.GetDeck().discardPile.Count());
        expected.Should().BeEquivalentTo(actual.GetDeck().discardPile.Peek());
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(2, actual.GetNextPlayerIndex());
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
        Assert.Single(gamestate.GetDeck().discardPile);
        // When
        var actual = gamestate.Apply(new List<Card> { card1, card2 });
        var expected = card2;
        // Then
        Assert.Equal(5, gamestate.GetCurrentPlayer().GetHand().Count());
        expected.Should().BeEquivalentTo(actual.GetDeck().discardPile.Peek());
        Assert.Equal((gamestate.GetDeck().discardPile.Count() + 2), actual.GetDeck().discardPile.Count());
        Assert.NotEqual(gamestate.GetCurrentPlayerIndex(), actual.GetCurrentPlayerIndex());
        Assert.NotEqual(gamestate.GetNextPlayerIndex(), actual.GetNextPlayerIndex());
        Assert.Equal(3, actual.GetPlayers()[0].GetHand().Count());
        Assert.Equal(1, actual.GetCurrentPlayerIndex());
        Assert.Equal(2, actual.GetNextPlayerIndex());
    }

    [Fact]
    public void FirstPlayerKnockOutReturnsCorrectIndexes()
    {
        var players = new List<Iplayer>{
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer()
        };
        // Given
        var gs = new GameState(players);
        // When
        gs.PlayerKnockOut(gs.GetCurrentPlayer());
        // Then
        Assert.Equal(0, gs.GetCurrentPlayerIndex());
        Assert.Equal(1, gs.GetNextPlayerIndex());
    }

    [Fact]
    public void FirstPlayerKnockOutReversePlayDirection()
    {
        // Given
        var players = new List<Iplayer>{
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer()
        };
        var gs = new GameState(players, false);
        // When
        gs.PlayerKnockOut(gs.GetCurrentPlayer());
        // Then
        Assert.Equal(2, gs.GetCurrentPlayerIndex());
        Assert.Equal(1, gs.GetNextPlayerIndex());
    }

    [Fact]
    public void LastPlayerKnockOutReturnsCorrectPlayer()
    {
        // Given
        var players = new List<Iplayer>{
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer()
        };
        var gs = new GameState(players, 3);
        // When
        gs.PlayerKnockOut(gs.GetCurrentPlayer());
        // Then
        Assert.Equal(0, gs.GetCurrentPlayerIndex());
        Assert.Equal(1, gs.GetNextPlayerIndex());
    }
    [Fact]
    public void LastPlayerKnockOutWhenReversePlayDirection()
    {
        // Given
        var players = new List<Iplayer>{
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer(),
            new TestPlayer()
        };
        var gs = new GameState(players, 3, false);
        // When
        gs.PlayerKnockOut(gs.GetCurrentPlayer());
        // Then
        Assert.Equal(2, gs.GetCurrentPlayerIndex());
        Assert.Equal(1, gs.GetNextPlayerIndex());
    }
}
