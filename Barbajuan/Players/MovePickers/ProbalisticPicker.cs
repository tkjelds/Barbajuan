class ProbalisticPicker : ImovePicker
{
    List<(ImovePicker,int)> MoveProbability = new List<(ImovePicker, int)>();

    public ProbalisticPicker(List<(ImovePicker, int)> moveProbability)
    {
        MoveProbability = moveProbability;
    }

    public List<Card> pick(GameState gameState)
    {
        var rng = new Random();
        var pick = rng.Next(SumProbability());
        var firstBoundary = 0;
        var secondBoundary = 0;
        foreach (var moveProb in MoveProbability)
        {
            secondBoundary += moveProb.Item2-1;
            if(moveProb.Item2 >= firstBoundary && moveProb.Item2 <= secondBoundary) return moveProb.Item1.pick(gameState);
            firstBoundary = secondBoundary;
        }
        return MoveProbability[0].Item1.pick(gameState);
    }

    private int SumProbability(){
        var sum = 0;
        foreach (var item in MoveProbability)
        {
            sum += item.Item2;
        }
        return sum;
    }

    public List<List<Card>> getLegalMoves(Card topCard, List<Card> hand)
    {
        var rng = new Random();
        var pick = rng.Next(SumProbability());
        var firstBoundary = 0;
        var secondBoundary = 0;
        foreach (var moveProb in MoveProbability)
        {
            secondBoundary += moveProb.Item2-1;
            if(moveProb.Item2 >= firstBoundary && moveProb.Item2 <= secondBoundary) return moveProb.Item1.getLegalMoves(topCard,hand);
            firstBoundary = secondBoundary;
        }
        var legalMoves = MoveProbability[0].Item1.getLegalMoves(topCard,hand);
        if(legalMoves.Count == 0) return new List<List<Card>>() { new List<Card>(){new Card(WILD, DRAW1)} }; 
        legalMoves.Distinct();
        return legalMoves;
    }
}