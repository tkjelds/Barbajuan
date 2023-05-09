public class CardTest
{
    [Fact]
    public void CardsComparerComparesTwoListsOfSameCards()
    {
        // Given
        var cardsComparer = new CardsComparer();
        var cardList1 = new List<Card>() { new Card(RED, FOUR), new Card(BLUE, TWO), new Card(YELLOW, THREE), new Card(WILD, DRAW4) };
        var cardList2 = new List<Card>() { new Card(RED, FOUR), new Card(BLUE, TWO), new Card(YELLOW, THREE), new Card(WILD, DRAW4) };
        // When
        var actual = cardsComparer.Equals(cardList1, cardList2);
        // Then
        Assert.True(actual);
    }

    [Fact]
    public void CardsComparerComparesTwoEmptyLists()
    {
        // Given
        var cardsComparer = new CardsComparer();
        var cardList1 = new List<Card>();
        var cardList2 = new List<Card>();
        // When
        var actual = cardsComparer.Equals(cardList1, cardList2);
        // Then
        Assert.True(actual);
    }

    [Fact]
    public void CardsComparerComparesTwoListsOfSameCardsWithDifferentOrder()
    {
        // Given
        var cardsComparer = new CardsComparer();
        var cardList1 = new List<Card>() { new Card(RED, FOUR), new Card(BLUE, TWO), new Card(YELLOW, THREE), new Card(WILD, DRAW4) };
        var cardList2 = new List<Card>() { new Card(RED, FOUR), new Card(YELLOW, THREE), new Card(WILD, DRAW4), new Card(BLUE, TWO) };
        // When
        var actual = cardsComparer.Equals(cardList1, cardList2);
        // Then
        Assert.True(actual);
    }

    [Fact]
    public void ListOfCardsSortedCorrectly()
    {
        // Given
        var card1 = new Card(GREEN, ONE);
        var card2 = new Card(YELLOW, ONE);
        var card3 = new Card(BLUE, THREE);
        var card4 = new Card(RED, THREE);
        var card5 = new Card(WILD, DRAW4);
        var hand1 = new List<Card>(){
            card1,card5,card3,card2,card4
        };
        // When
        hand1.Sort();
        var expected = new List<Card>(){
            card1,card2,card3,card4,card5
        };
        // Then
        Assert.Equal(expected, hand1);
    }


}