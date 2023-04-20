public interface Iplayer
{
    public List<Card> Action(GameState gameState);
    public List<Card> GetHand();
    public void RemoveCardFromHand(Card cards);
    public void AddCardsToHand(List<Card> cards);
    public string GetName();
    public Iplayer Clone();
    List<List<Card>> GetLegalMoves(Card topCard);
}
