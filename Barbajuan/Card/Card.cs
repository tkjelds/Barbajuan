public class Card
{
    public CardColor cardColor;
    public CardType cardType;

    public Card(CardColor cardColor, CardType cardType)
    {
        this.cardColor = cardColor;
        this.cardType = cardType;
    }

    public bool canBePlayedOn(Card card1, Card card2){
        //TODO
        return false;
    }
}