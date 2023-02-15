using static CardColor;
using static CardType;
namespace Barbajuan.tests;

public class CardTests
{
    [Fact]
    public void Red1PlayedOnBlue1ReturnTrue()
    {
        //Given
        var red1 = new Card(RED, ONE);
        var blue1 = new Card(BLUE, ONE);
        //When
        var actual = blue1.canBePlayedOn(red1);
        //Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    [InlineData(1, 5)]
    [InlineData(2, 6)]
    [InlineData(3, 7)]
    [InlineData(4, 8)]
    [InlineData(1, 9)]
    [InlineData(2, 10)]
    [InlineData(3, 11)]
    [InlineData(0, 12)]
    [InlineData(4, 13)]
    [InlineData(4, 14)]
    public void WildCardCanBePlayedOnAnything(int cardColor, int cardType)
    {
        // Given
        var card = new Card((CardColor)cardColor, (CardType)cardType);
        var wildCard = new Card(WILD, SELECTCOLOR);
        // When
        var actual = wildCard.canBePlayedOn(card);
        // Then
        Assert.True(actual);
    }
    [Fact]
    public void Red1CantBePlayedOnBlue4()
    {
        // Given
        var red1 = new Card(RED, ONE);
        var blue4 = new Card(BLUE, FOUR);
        // When
        var actual = red1.canBePlayedOn(blue4);
        // Then
        Assert.False(actual);
    }
}