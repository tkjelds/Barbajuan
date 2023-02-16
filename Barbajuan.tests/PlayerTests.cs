namespace Barbajuan.tests;
using static CardColor;
using static CardType;
public class PlayerTests
{
    [Fact]
    public void ActionReturnDrawOneWhenNoMoves()
    {
        var discardStack = new Stack<Card>();
        discardStack.Push(new Card(BLUE, DRAW1));
        var deck = new Deck(new Stack<Card>(), discardStack);
        var player = new TestPlayer(new List<Card>() { new Card(GREEN, ZERO), new Card(RED, ONE), new Card(YELLOW, TWO) });
        var gameState = new GameState(new List<Player>() { player }, deck);
        //When
        var actual = player.action(gameState);
        //Then
        Assert.True(actual.cardType == DRAW1);
    }
}