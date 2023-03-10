public interface Iplayer
{
    public List<Card> action(IgameState gameState);
    public List<Card> getHand();
    public void removeCardFromHand(Card cards);
    public void addCardsToHand(List<Card> cards);
    public string getName();

    public Iplayer clone();
}
