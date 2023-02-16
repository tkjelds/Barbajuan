using static CardColor;
using static CardType;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var toBePlayedOn = new Card(RED, ZERO);
        var player = new RandomPlayer(new List<Card>() { 
            new Card(BLUE, ZERO), 
            new Card(RED, FOUR), 
            new Card(YELLOW, FOUR), 
            new Card(BLUE, FOUR)});
        // When
        var stackingMoves = player.getStackingActions(toBePlayedOn);
        foreach (var move in stackingMoves)
        {
            foreach (var m in move)
            {
                Console.Write("   Card: " + m.cardColor.ToString() + "  " + m.cardType.ToString());
            }
            Console.WriteLine("");
        }
    }
}