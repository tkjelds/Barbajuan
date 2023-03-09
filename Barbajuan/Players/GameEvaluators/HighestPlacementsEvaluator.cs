[Serializable]
class HighestPlacementsEvaluator : IgameEvaluator
{
    public int evaluate(GameState gs, string playerName)
    {
        var totalPlayers = gs.GetPlayers().Count() + gs.getScoreBoard().Count();
        var remainingPlayers = gs.GetPlayers().Count();
        var highestScore = totalPlayers;
        for (int i = 0; i < gs.getScoreBoard().Count(); i++)
        {
            if(gs.getScoreBoard()[i].getName() == playerName) return highestScore-i;
        }
        return 0;
    }
}