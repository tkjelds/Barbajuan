using static CardColor;
public static class CardExtensions
{
    public static bool canBePlayedOn(this Card card1, Card card2)
    {
        if (card2.cardColor is WILD || card1.cardColor is WILD)
        {
            return true;
        }

        if (card1.cardType is CardType.DRAW4 || card1.cardType is CardType.SELECTCOLOR)
        {
            return true;
        }

        if (card2.cardColor == card1.cardColor)
        {
            return true;
        }

        if (card2.cardType == card1.cardType)
        {
            return true;
        }
        //TODO
        return false;
    }
}
