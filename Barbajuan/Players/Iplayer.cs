public interface Iplayer
{
    public Card action(IgameState gameState);
    public List<Card> getActions(Card topCard);

}
