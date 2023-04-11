public class Epsilon_Greedy : ImovePicker
{

    int epsilon = 5;
    
    RandomMovePicker randomMove = new RandomMovePicker();

    StackingMovePicker greedyMove = new StackingMovePicker();


    public Epsilon_Greedy(int epsilon)
    {
        this.epsilon = epsilon;
    }


    public Epsilon_Greedy(){}

    public List<Card> pick(GameState gameState)
    {
        var rng = new Random();
        var pick = rng.Next(100);

        if (pick<epsilon) return greedyMove.pick(gameState);

        return randomMove.pick(gameState);
    }


    public List<List<Card>> getLegalMoves(Card topCard, List<Card> hand)
    {
        return randomMove.getLegalMoves(topCard, hand);
    }
}