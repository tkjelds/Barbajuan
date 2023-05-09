using System.Diagnostics.CodeAnalysis;

public class CardsComparer : EqualityComparer<List<Card>>
{
    public override bool Equals(List<Card>? x, List<Card>? y)
    {

        if (x == null && y == null) return true;
        // if null, return false
        if (x == null || y == null) return false;
        // if length of lists differ, return false
        if (x.Count() != y.Count()) return false;

        x.Sort();
        y.Sort();

        // we know now that lists are same length. check card enum types in lists against each other
        for (int i = 0; i < x.Count(); i++)
        {
            if (x[i].cardColor != y[i].cardColor) return false;
            if (x[i].cardType != y[i].cardType) return false;
        }
        return true;
    }

    public override int GetHashCode([DisallowNull] List<Card> obj)
    {
        var sumOfCards = 0;
        foreach (var card in obj)
        {
            sumOfCards = sumOfCards + ((int)card.cardColor ^ (int)card.cardType);
        }
        var hashCode = obj.Count() ^ sumOfCards;

        return hashCode.GetHashCode();
    }
}