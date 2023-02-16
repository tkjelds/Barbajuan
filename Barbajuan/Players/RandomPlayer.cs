public class RandomPlayer : Player
{
    public RandomPlayer(List<Card> hand) : base(hand) => this.hand = hand;

    public new List<Card> action(IgameState gameState)
    {
        var moves = this.getActions(gameState.getDeck().discardPile.Peek());
        if (moves.Count == 0)
        {
            return new List<Card>() { new Card(CardColor.WILD, CardType.DRAW1) };
        }

        var random = new Random();

        var randomMove = random.Next(moves.Count);

        return moves[randomMove];
    }
}
