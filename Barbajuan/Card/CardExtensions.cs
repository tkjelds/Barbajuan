using static CardColor;
public static class CardExtensions
{
    public static bool canBePlayedOn(this Card card1, Card card2)
    {
        if (card2.cardColor == CardColor.WILD || card1.cardColor == CardColor.WILD) return true;
        if (card2.cardType == CardType.DRAW4 || card2.cardType == CardType.SELECTCOLOR) return true;
        if (card2.cardColor == card1.cardColor) return true;
        if (card2.cardType == card1.cardType) return true;
        //TODO
        return false;
    }
}