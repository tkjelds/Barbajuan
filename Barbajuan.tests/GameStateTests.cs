namespace Barbajuan.tests;

public class GameStateTests
{
    [Fact]
    public void TestIsGameOverOnePlayer()
    {
        // Given
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