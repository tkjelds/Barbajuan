class RandomPlayer : Iplayer
{
    private Hand hand;

    public RandomPlayer(Hand hand)
    {
        this.hand = hand;
    }

    public IgameState action()
    {
        throw new NotImplementedException();
    }

    public List<Card> getActions(IgameState gameState)
    {
        throw new NotImplementedException();
    }
}