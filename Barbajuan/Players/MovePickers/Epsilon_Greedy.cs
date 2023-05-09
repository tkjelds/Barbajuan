public class Epsilon_Greedy : ImovePicker
{

    int epsilon = 5;

    RandomMovePicker randomMove = new RandomMovePicker();

    StackingMovePicker greedyMove = new StackingMovePicker();


    public Epsilon_Greedy(int epsilon)
    {
        this.epsilon = epsilon;
    }


    public Epsilon_Greedy() { }

    public List<Card> Pick(GameState gameState)
    {
        var rng = new Random();
        var pick = rng.Next(100);

        if (pick < epsilon) return randomMove.Pick(gameState);

        return greedyMove.Pick(gameState);
    }


    public List<List<Card>> GetLegalMoves(Card topCard, List<Card> hand)
    {
        return randomMove.GetLegalMoves(topCard, hand);
    }
}