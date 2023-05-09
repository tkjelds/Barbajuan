namespace Barbajuan.tests;

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
        var actual = player.Action(gameState).First();
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
        var stackingMoves = player.GetStackingActions(toBePlayedOn);
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
        var stackingMoves = player.GetStackingActions(toBePlayedOn);
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
        var actual = player.GetStackingActions(toBePlayedOn);
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
        var player = new RandomPlayer(new List<Card>() { new Card(YELLOW, FOUR), new Card(YELLOW, THREE) }, "carl");
        var playerHand = player.GetHand();
        // When
        var actual = player.Clone();
        var actualHand = actual.GetHand();
        // Then
        Assert.Equal(player.GetHand().Count(), actual.GetHand().Count());
        Assert.Equal(player.GetName(), actual.GetName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player, actual);
        Assert.Equal(player, player);
    }

    [Fact]
    public void CloneOfPlayerIsDifferentObject()
    {
        // Given
        var player = new RandomPlayer(new List<Card>() { new Card(YELLOW, FOUR), new Card(YELLOW, THREE) }, "carl");
        var playerHand = player.GetHand();
        // When
        var actual = player.Clone();
        var actualHand = actual.GetHand();
        // Then
        Assert.Equal(player.GetHand().Count(), actual.GetHand().Count());
        Assert.Equal(player.GetName(), actual.GetName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player, actual);
        Assert.Equal(player, player);
    }

    [Fact]
    public void CloneOfRandomStackingPlayerIsDifferentObject()
    {
        // Given
        var player = new RandomStackingPlayer("carl", new List<Card>() { new Card(YELLOW, FOUR), new Card(YELLOW, THREE) });
        var playerHand = player.GetHand();
        // When
        var actual = player.Clone();
        var actualHand = actual.GetHand();
        // Then
        Assert.Equal(player.GetHand().Count(), actual.GetHand().Count());
        Assert.Equal(player.GetName(), actual.GetName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player, actual);
        Assert.Equal(player, player);
    }
    [Fact]
    public void CloneOfMonteCarloPlayerIsDifferentObject()
    {
        // Given
        var player = new FlatMonteCarloPlayer(new List<Card>() { new Card(YELLOW, FOUR), new Card(YELLOW, THREE) }, 10, 10, "carl");
        var playerHand = player.GetHand();
        // When
        var actual = player.Clone();
        var actualHand = actual.GetHand();
        // Then
        Assert.Equal(player.GetHand().Count(), actual.GetHand().Count());
        Assert.Equal(player.GetName(), actual.GetName());
        playerHand.Should().BeEquivalentTo(actualHand);
        Assert.NotEqual(player, actual);
        Assert.Equal(player, player);
    }

    [Fact]
    public void ListofPlayersIsClonedInCorrectOrder()
    {
        // Given
        var players = new List<Iplayer>() { new RandomPlayer("Player0"), new RandomPlayer("Player1"), new RandomPlayer("Player2"), new RandomPlayer("Player3") };
        var actual = new List<Iplayer>();
        // When
        foreach (var player in players)
        {
            actual.Add(player.Clone());
        }
        // Then
        Assert.Equal(4, actual.Count());
        Assert.Equal("Player0", actual[0].GetName());
        Assert.Equal("Player1", actual[1].GetName());
        Assert.Equal("Player2", actual[2].GetName());
        Assert.Equal("Player3", actual[3].GetName());
    }
}
