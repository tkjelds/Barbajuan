public interface ImovePicker
{
    
    List<List<Card>> getLegalMoves(Card topCard, List<Card> hand);
    List<Card> pick(GameState gameState);
}