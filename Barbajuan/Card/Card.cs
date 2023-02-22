[Serializable]
public class Card
{
    public CardColor cardColor;
    public CardType cardType;

    public Card(CardColor cardColor, CardType cardType)
    {
        this.cardColor = cardColor;
        this.cardType = cardType;
    }

    public override string ToString()
    {
        return "Color: " + this.cardColor.ToString() + "  Card type: " + this.cardType.ToString();
    }

}
