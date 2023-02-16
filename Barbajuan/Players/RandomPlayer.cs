public class RandomPlayer : Player
{
    public RandomPlayer(List<Card> hand) : base(hand)
    {
        this.hand = hand;
    }

    public new Card action(IgameState gameState)
    {
        var moves = getActions(gameState);
        if (moves.Count() == 0) return new Card(CardColor.WILD,CardType.DRAW1);

        var random = new Random();

        var randomMove = random.Next(moves.Count());

        return moves[randomMove];
    }
}