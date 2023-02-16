namespace Barbajuan.tests;

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
        var GameState = new GameState(new List<Player>() { Player1, Player2, Player3 }, deck);
        //when 
        var actual = GameState.GetPlayers()[GameState.nextPlayer()];
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
        var GameState = new GameState(player3, 2, new List<Player>() { player1, player2, player3 }, deck);
        // When
        var actual = GameState.GetPlayers()[GameState.nextPlayer()];
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
        var GameState = new GameState(new List<Player>() { player1, player2, player3 }, deck, false);
        // When
        var actual = GameState.GetPlayers()[GameState.nextPlayer()];
        // Then
        Assert.Equal(player3, actual);
    }
    [Fact]
    public void IsGameOverOnePlayer()
    {
        var players = new List<Player>();
        var hand = new List<Card>();
        players.Add(new RandomPlayer(hand));

        var GameState = new GameState(players);

        // When

        var actual = GameState.IsGameOver();

        // Then
        Assert.True(actual);
    }

    [Fact]
    public void TestIsGameOverMultiplePlayers()
    {
        // Given
        var players = new List<Player>();

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
}
