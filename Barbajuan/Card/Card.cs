[Serializable]
public class Card : IComparable
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

    public Card Clone(){
        return new Card(this.cardColor,this.cardType);
    }



    // public int CompareTo(object obj) {
    //     if (obj == null) return 1;

    //     Temperature otherTemperature = obj as Temperature;
    //     if (otherTemperature != null)
    //         return this.temperatureF.CompareTo(otherTemperature.temperatureF);
    //     else
    //        throw new ArgumentException("Object is not a Temperature");
    // }

    // public double Fahrenheit
    // {
    //     get
    //     {
    //         return this.temperatureF;
    //     }
    //     set 
    //     {
    //         this.temperatureF = value;
    //     }
    // }

    // public double Celsius
    // {
    //     get
    //     {
    //         return (this.temperatureF - 32) * (5.0/9);
    //     }
    //     set
    //     {
    //         this.temperatureF = (value * 9.0/5) + 32;
    //     }
    // }
    

    /*
    Less than zero	The current instance precedes the object specified by the CompareTo method in the sort order.
    Zero	This current instance occurs in the same position in the sort order as the object specified by the CompareTo method.
    Greater than zero	This current instance follows the object specified by the CompareTo method in the sort order.
    */
    
    public int CompareTo(object? obj)
    {
        if (obj == null) return 1; 

        Card other = (Card) obj;

        if (this.cardType == other.cardType && this.cardColor == other.cardColor) return 0;

        if (this.cardType == other.cardType) return (this.cardColor - other.cardColor);

        return (this.cardType - other.cardType);
        
        //if(this.cardType < other.cardType || this.cardColor < other.cardColor) { return -1;} 
        //throw new NotImplementedException();
    }
}
