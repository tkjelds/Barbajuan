class FlatMonteCarloPlayer : Iplayer
{

    List<Card> Hand;

    int Determinzations;

    int Iterations;

    public FlatMonteCarloPlayer(List<Card> hand, int determinzations, int iterations)
    {
        Hand = hand;
        Determinzations = determinzations;
        Iterations = iterations;
    }

    public List<Card> action(IgameState gameState)
    {
        throw new NotImplementedException();
    }

    public void addCardsToHand(List<Card> cards)
    {
        throw new NotImplementedException();
    }

    public List<Card> getHand()
    {
        throw new NotImplementedException();
    }

    public string getName()
    {
        throw new NotImplementedException();
    }

    public void removeCardFromHand(Card cards)
    {
        throw new NotImplementedException();
    }
}