
public class FactorialEvaluator : IgameEvaluator
{
    public int Evaluate(GameState gs, string playerName)
    {
        var totalPlayers = gs.GetPlayers().Count() + gs.GetScoreBoard().Count();
        for (int i = 0; i < gs.GetScoreBoard().Count(); i++)
        {
            if(gs.GetScoreBoard()[i].GetName() == playerName) return factorial(totalPlayers-i);
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