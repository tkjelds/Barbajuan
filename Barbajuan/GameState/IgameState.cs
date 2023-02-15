interface IgameState
{
    public void run();
    public bool IsGameOver();
    public IgameState apply(Card c);

}