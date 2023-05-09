class ProbalisticPicker : ImovePicker
{
    List<(ImovePicker, int)> moveProbability = new List<(ImovePicker, int)>();

    public ProbalisticPicker(List<(ImovePicker, int)> _moveProbability)
    {
        moveProbability = _moveProbability;
    }

    public List<Card> Pick(GameState gameState)
    {
        var rng = new Random();
        var pick = rng.Next(SumProbability());
        var firstBoundary = 0;
        var secondBoundary = 0;
        foreach (var moveProb in moveProbability)
        {
            secondBoundary += moveProb.Item2 - 1;
            if (moveProb.Item2 >= firstBoundary && moveProb.Item2 <= secondBoundary) return moveProb.Item1.Pick(gameState);
            firstBoundary = secondBoundary;
        }
        return moveProbability[0].Item1.Pick(gameState);
    }

    private int SumProbability()
    {
        var sum = 0;
        foreach (var item in moveProbability)
        {
            sum += item.Item2;
        }
        return sum;
    }

    public List<List<Card>> GetLegalMoves(Card topCard, List<Card> hand)
    {
        var rng = new Random();
        var pick = rng.Next(SumProbability());
        var firstBoundary = 0;
        var secondBoundary = 0;
        foreach (var moveProb in moveProbability)
        {
            secondBoundary += moveProb.Item2 - 1;
            if (moveProb.Item2 >= firstBoundary && moveProb.Item2 <= secondBoundary) return moveProb.Item1.GetLegalMoves(topCard, hand);
            firstBoundary = secondBoundary;
        }
        var legalMoves = moveProbability[0].Item1.GetLegalMoves(topCard, hand);
        if (legalMoves.Count == 0) return new List<List<Card>>() { new List<Card>() { new Card(WILD, DRAW1) } };
        legalMoves.Distinct();
        return legalMoves;
    }
}