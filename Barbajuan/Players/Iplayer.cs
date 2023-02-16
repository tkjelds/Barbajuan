interface Iplayer
{
    public Card action(IgameState gameState);
    public List<Card> getActions(IgameState gameState);
}