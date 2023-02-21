public interface IgameState
{
    public void run();
    public bool IsGameOver();
    public IgameState apply(GameState gs,Card card);
    public IgameState apply(List<Card> Cards);
    public int nextPlayer(int index, GameState gs);
    public Deck getDeck();
    public List<Player> GetPlayers();
    public bool getPlayDirection();
    public int getCurrentPlayerIndex();

    public Player getCurrentPlayer();

    public int getNextPlayerIndex();

    public List<Player> getScoreBoard();
}
