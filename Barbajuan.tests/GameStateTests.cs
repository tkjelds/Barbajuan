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
        
    [Fact]
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
}