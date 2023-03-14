namespace Barbajuan.tests;
public class FlatMonteCarloPlayerTests
{
    [Fact]
    public void CreateDeterminationDoesNotChangeHandSize()
    {
        var MonteCarloPlayer = new FlatMonteCarloPlayer();
        var players = new List<Iplayer>(){
                new RandomPlayer("bot 1"),
                new RandomPlayer("bot 2"),
                MonteCarloPlayer
        };
        
        var gs = new GameState(players);
        gs.DealSevenToEachPlayer();
        // MonteCarloPlayer has 7 cards in hand
        players[0].addCardsToHand(gs.getDeck().draw(1)); // bot 1 has 8 cards
        players[1].addCardsToHand(gs.getDeck().draw(2)); // bot 2 has 9 cards
        
        var copyGameState = MonteCarloPlayer.createDetermination(gs);
        // When
        
        var playerOneHand = gs.GetPlayers().Find(x => x.getName() == "bot 1")!.getHand();
        var playerTwoHand = gs.GetPlayers().Find(x => x.getName() == "bot 2")!.getHand();
        var monteCarloPlayerHand = gs.GetPlayers().Find(x => x.getName() == MonteCarloPlayer.getName())!.getHand();
        var copyPlayerOneHand = copyGameState.GetPlayers().Find(x => x.getName() == "bot 1")!.getHand();
        var copyPlayerTwoHand = copyGameState.GetPlayers().Find(x => x.getName() == "bot 2")!.getHand();
        var copyMonteCarloPlayerHand = copyGameState.GetPlayers().Find(x => x.getName() == MonteCarloPlayer.getName())!.getHand();
        
        // Then
        Assert.Equal(playerOneHand.Count(),copyPlayerOneHand.Count());
        Assert.Equal(playerTwoHand.Count(),copyPlayerTwoHand.Count());
        Assert.Equal(copyMonteCarloPlayerHand.Count(),monteCarloPlayerHand.Count());
    }   

    [Fact]
    public void CreateDeterminationGivesNewCardsToCopyPlayers()
    {
        // Given
        var MonteCarloPlayer = new FlatMonteCarloPlayer();
        var players = new List<Iplayer>(){
                new RandomPlayer("bot 1"),
                new RandomPlayer("bot 2"),
                MonteCarloPlayer
        };
        
        var gs = new GameState(players);
        gs.DealSevenToEachPlayer();
        // MonteCarloPlayer has 7 cards in hand
        players[0].addCardsToHand(gs.getDeck().draw(1)); // bot 1 has 8 cards
        players[1].addCardsToHand(gs.getDeck().draw(2)); // bot 2 has 9 cards
        
        var copyGameState = MonteCarloPlayer.createDetermination(gs);
        // When
        
        var playerOneHand = gs.GetPlayers().Find(x => x.getName() == "bot 1")!.getHand();
        var playerTwoHand = gs.GetPlayers().Find(x => x.getName() == "bot 2")!.getHand();
        var monteCarloPlayerHand = gs.GetPlayers().Find(x => x.getName() == MonteCarloPlayer.getName())!.getHand();
        var copyPlayerOneHand = copyGameState.GetPlayers().Find(x => x.getName() == "bot 1")!.getHand();
        var copyPlayerTwoHand = copyGameState.GetPlayers().Find(x => x.getName() == "bot 2")!.getHand();
        var copyMonteCarloPlayerHand = copyGameState.GetPlayers().Find(x => x.getName() == MonteCarloPlayer.getName())!.getHand();
        
        // Then
        playerOneHand.Should().NotBeEquivalentTo(copyPlayerOneHand);
        playerTwoHand.Should().NotBeEquivalentTo(copyPlayerTwoHand);
        monteCarloPlayerHand.Should().BeEquivalentTo(copyMonteCarloPlayerHand);
    }
}