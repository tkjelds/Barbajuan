
public class FactorialEvaluator : IgameEvaluator
{
    public int evaluate(GameState gs, string playerName)
    {
        var totalPlayers = gs.GetPlayers().Count() + gs.getScoreBoard().Count();
        for (int i = 0; i < gs.getScoreBoard().Count(); i++)
        {
            if(gs.getScoreBoard()[i].getName() == playerName) return factorial(totalPlayers-i);
        }
        return 0;
    }

    public int factorial(int n) {
        var sum = n;
        for (int i = 1; i < n; i++)
        {
            n = n*i;
        }
        return sum;
    }
}