public interface ImovePicker
{

    List<List<Card>> GetLegalMoves(Card topCard, List<Card> hand);
    List<Card> Pick(GameState gameState);
}