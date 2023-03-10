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
        var gameState = new GameState(new List<Iplayer>() { player }, deck);
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
        var player = new RandomPlayer(new List<Card>() { new Card(BLUE, ZERO),
        new Card(RED, FOUR),
        new Card(YELLOW, FOUR),
        new Card(BLUE, FOUR),
        new Card(WILD, DRAW4),
        new Card(WILD, SELECTCOLOR) });
        // When
        var stackingMoves = player.getStackingActions(toBePlayedOn);
        // Then
        Assert.Equal(14, stackingMoves.Count());
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

    [Fact]
    public void CloneOfRandomPlayerIsDifferentObject()
    {
        // Given
        var player = new RandomPlayer(new List<Card>(){new Card(YELLOW,FOUR) ,new Card(YELLOW,THREE)}, "carl");
        var playerHand = player.getHand();
        // When
        var actual = player.clone();
        var actualHand = actual.getHand();
        // Then
        Assert.Equal(player.getHand().Count() , actual.getHand().Count());
        Assert.Equal(player.getName(), actual.getName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player,actual);
        Assert.Equal(player,player);
    }

    [Fact]
    public void CloneOfPlayerIsDifferentObject()
    {
        // Given
        var player = new Player(new List<Card>(){new Card(YELLOW,FOUR) ,new Card(YELLOW,THREE)}, "carl");
        var playerHand = player.getHand();
        // When
        var actual = player.clone();
        var actualHand = actual.getHand();
        // Then
        Assert.Equal(player.getHand().Count() , actual.getHand().Count());
        Assert.Equal(player.getName(), actual.getName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player,actual);
        Assert.Equal(player,player);
    }

    [Fact]
    public void CloneOfRandomStackingPlayerIsDifferentObject()
    {
        // Given
        var player = new RandomStackingPlayer ( "carl",new List<Card>(){new Card(YELLOW,FOUR) ,new Card(YELLOW,THREE)});
        var playerHand = player.getHand();
        // When
        var actual = player.clone();
        var actualHand = actual.getHand();
        // Then
        Assert.Equal(player.getHand().Count() , actual.getHand().Count());
        Assert.Equal(player.getName(), actual.getName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player,actual);
        Assert.Equal(player,player);
    }
    [Fact]
    public void CloneOfMonteCarloPlayerIsDifferentObject()
    {
        // Given
        var player = new FlatMonteCarloPlayer (new List<Card>(){new Card(YELLOW,FOUR) ,new Card(YELLOW,THREE)},10,10,"carl");
        var playerHand = player.getHand();
        // When
        var actual = player.clone();
        var actualHand = actual.getHand();
        // Then
        Assert.Equal(player.getHand().Count() , actual.getHand().Count());
        Assert.Equal(player.getName(), actual.getName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player,actual);
        Assert.Equal(player,player);
    }
}
