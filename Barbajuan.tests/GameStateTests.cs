namespace Barbajuan.tests;

public class GameStateTests
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
        
    [Fact (Skip="not implemented")]
    public void NextPlayerReturnsCorrectPlayer()
    {
        //given
        var Deck = new Deck();
        Deck.setup();
        var Player1 = new TestPlayer(Deck.draw(7));
        var Player2 = new TestPlayer(Deck.draw(7));
        var Player3 = new TestPlayer(Deck.draw(7));
        var gameState = new gameState(new List<Player>(){Player1,Player2,Player3});
        //when 
        var actual = gameState.GetPlayers()[gameState.nextPlayer()];
        //then
        Assert.Equal(Player2, actual);
    }
    
    [Fact]
    public void IsGameOverOnePlayer()
    {
        var players = new List<Player>();
        var hand = new List<Card>();
        players.Add(new RandomPlayer(hand));

        var gameState = new gameState(players);

        // When

        var actual = gameState.IsGameOver();

        // Then
        Assert.True(actual);
    }

    [Fact]
    public void TestIsGameOverMultiplePlayers()
    {
        // Given
        var players = new List<Player>();
        
        for(int i = 0; i < 10; i++) 
        {
            players.Add(new RandomPlayer(new List<Card>()));
        }

        var gameState = new gameState(players);
        // When

        var actual = gameState.IsGameOver();

        // Then
        Assert.False(actual);
    } 
}