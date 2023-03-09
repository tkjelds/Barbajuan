[Serializable]
// Returns 1 if firstplace, returns 0 if not
public class NaiveMoveEvaluator : ImoveEvaluator
{
    public int evaluate(GameState gs, string playerName)
    {
        var scoreBoard = gs.getScoreBoard();
        var score = scoreBoard[0].getName() == playerName ? 1 : 0;
        return score;
    }
}