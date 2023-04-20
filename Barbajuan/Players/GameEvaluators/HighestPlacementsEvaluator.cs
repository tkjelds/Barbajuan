
class HighestPlacementsEvaluator : IgameEvaluator
{
    public int Evaluate(GameState gs, string playerName)
    {
        var totalPlayers = gs.GetPlayers().Count() + gs.GetScoreBoard().Count();
        var remainingPlayers = gs.GetPlayers().Count();
        var highestScore = totalPlayers;
        for (int i = 0; i < gs.GetScoreBoard().Count(); i++)
        {
            if(gs.GetScoreBoard()[i].GetName() == playerName) return highestScore-i;
        }

        return 0;
    }
}