public class RandomMovePicker : ImovePicker
{
    public List<Card> pick(List<List<Card>> moves)
    {
        var random = new Random();
        return moves[random.Next(moves.Count())];
    }
}