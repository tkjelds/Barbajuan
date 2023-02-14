interface Iplayer
{
    public IgameState action();
    public List<Card> getActions(IgameState gameState);
}