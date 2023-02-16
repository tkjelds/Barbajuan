public interface IgameState
{
    public void run();
    public bool IsGameOver();
    public IgameState apply(Card card);
    public IgameState apply(List<Card> Cards);
    public int nextPlayer();
    public Deck getDeck();
    public List<Player> GetPlayers();
    public bool getPlayDirection();
    public int getCurrentPlayerIndex();
}
