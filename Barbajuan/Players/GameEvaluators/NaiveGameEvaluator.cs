
// Returns 1 if firstplace, returns 0 if not
public class NaiveGameEvaluator : IgameEvaluator
{
    public int Evaluate(GameState gs, string playerName)
    {
        var scoreBoard = gs.GetScoreBoard();
        var score = scoreBoard[0].GetName() == playerName ? 1 : 0;
        return score;
    }
}