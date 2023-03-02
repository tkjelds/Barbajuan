[Serializable]
public class NaiveMovePicker : ImovePicker
{
    public List<Card> pick(List<List<Card>> moves)
    {
        moves.Sort((x, y) => x.Count().CompareTo(y.Count()));
        return moves.Last();
    }
}