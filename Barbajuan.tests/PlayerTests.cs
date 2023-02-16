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
        var actual = player.action(gameState).First();
        //Then
        Assert.True(actual.cardType == DRAW1);
    }

    [Fact]
    public void getCorrectNumberOfStackingMoves()
    {
        // Given
        var toBePlayedOn = new Card(RED, ZERO);
        var player = new RandomPlayer(new List<Card>() { new Card(BLUE, ZERO), new Card(RED, FOUR), new Card(YELLOW, FOUR), new Card(BLUE, FOUR) });
        // When
        var stackingMoves = player.getStackingActions(toBePlayedOn);
        // Then
        Assert.Equal(6, stackingMoves.Count());
    }
    [Fact]
    public void getCorrectNumberOfStackingMovesWithWildCardInHand()
    {
        // Given
        var toBePlayedOn = new Card(RED, ZERO);
        var player = new RandomPlayer(new List<Card>() { new Card(BLUE, ZERO), new Card(RED, FOUR), new Card(YELLOW, FOUR), new Card(BLUE, FOUR), new Card(WILD, DRAW4) });
        // When
        var stackingMoves = player.getStackingActions(toBePlayedOn);
        // Then
        Assert.Equal(7, stackingMoves.Count());
    }

    [Fact]
    public void stackignMovesReturnsCorrectMoves()
    {
        // Given
        var toBePlayedOn = new Card(RED, ZERO);
        var player = new RandomPlayer(new List<Card>() { new Card(BLUE, ZERO), new Card(RED, FOUR), new Card(YELLOW, FOUR), new Card(BLUE, FOUR) });
        // When
        var actual = player.getStackingActions(toBePlayedOn);
        var expected = new List<List<Card>>(){
            new List<Card>() {new Card(BLUE, ZERO)},
            new List<Card>() {new Card(RED, FOUR)},
            new List<Card>() {new Card(RED, FOUR), new Card(YELLOW, FOUR)},
            new List<Card>() {new Card(RED, FOUR), new Card(YELLOW, FOUR), new Card(BLUE,FOUR)},
            new List<Card>() {new Card(RED, FOUR), new Card(BLUE, FOUR)},
            new List<Card>() {new Card(RED, FOUR), new Card(BLUE, FOUR), new Card(YELLOW,FOUR)}
        };
        // Then
        expected.Should().BeEquivalentTo(actual);
    }
}
