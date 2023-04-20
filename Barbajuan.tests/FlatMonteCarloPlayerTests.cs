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
        players[0].AddCardsToHand(gs.GetDeck().Draw(1)); // bot 1 has 8 cards
        players[1].AddCardsToHand(gs.GetDeck().Draw(2)); // bot 2 has 9 cards
        
        var copyGameState = MonteCarloPlayer.CreateDetermination(gs);
        // When
        
        var playerOneHand = gs.GetPlayers().Find(x => x.GetName() == "bot 1")!.GetHand();
        var playerTwoHand = gs.GetPlayers().Find(x => x.GetName() == "bot 2")!.GetHand();
        var monteCarloPlayerHand = gs.GetPlayers().Find(x => x.GetName() == MonteCarloPlayer.GetName())!.GetHand();
        var copyPlayerOneHand = copyGameState.GetPlayers().Find(x => x.GetName() == "bot 1")!.GetHand();
        var copyPlayerTwoHand = copyGameState.GetPlayers().Find(x => x.GetName() == "bot 2")!.GetHand();
        var copyMonteCarloPlayerHand = copyGameState.GetPlayers().Find(x => x.GetName() == MonteCarloPlayer.GetName())!.GetHand();
        
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
        players[0].AddCardsToHand(gs.GetDeck().Draw(1)); // bot 1 has 8 cards
        players[1].AddCardsToHand(gs.GetDeck().Draw(2)); // bot 2 has 9 cards
        
        var copyGameState = MonteCarloPlayer.CreateDetermination(gs);
        // When
        
        var playerOneHand = gs.GetPlayers().Find(x => x.GetName() == "bot 1")!.GetHand();
        var playerTwoHand = gs.GetPlayers().Find(x => x.GetName() == "bot 2")!.GetHand();
        var monteCarloPlayerHand = gs.GetPlayers().Find(x => x.GetName() == MonteCarloPlayer.GetName())!.GetHand();
        var copyPlayerOneHand = copyGameState.GetPlayers().Find(x => x.GetName() == "bot 1")!.GetHand();
        var copyPlayerTwoHand = copyGameState.GetPlayers().Find(x => x.GetName() == "bot 2")!.GetHand();
        var copyMonteCarloPlayerHand = copyGameState.GetPlayers().Find(x => x.GetName() == MonteCarloPlayer.GetName())!.GetHand();
        
        // Then
        playerOneHand.Should().NotBeEquivalentTo(copyPlayerOneHand);
        playerTwoHand.Should().NotBeEquivalentTo(copyPlayerTwoHand);
        monteCarloPlayerHand.Should().BeEquivalentTo(copyMonteCarloPlayerHand);
    }
}