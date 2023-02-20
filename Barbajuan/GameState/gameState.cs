public class GameState : IgameState
{
    private Player currentPlayer;
    private int nextPlayerIndex;
    private int currentPlayerIndex;
    private List<Player> players;
    private Deck deck;
    private bool playDirectionClockwise = true;
    private readonly List<Player> scoreBoard;
    public GameState(List<Player> players)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
    }

    public GameState(List<Player> players, Deck deck)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
    }
    public GameState(List<Player> players, Deck deck, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
        this.playDirectionClockwise = playDirection;
    }

    public GameState(Player currentPlayer, int currentPlayerIndex, List<Player> players, Deck deck)
    {
        this.currentPlayer = currentPlayer;
        this.players = players;
        this.deck = deck;
        this.scoreBoard = new List<Player>();
        this.currentPlayerIndex = currentPlayerIndex;
    }

    public GameState(List<Player> players, Deck deck, Player currentPlayer, int currentPlayerIndex, int nextPlayerIndex, List<Player> scoreBoard, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = currentPlayer;
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = scoreBoard;
        this.nextPlayerIndex = nextPlayerIndex;
        this.playDirectionClockwise = playDirection;
    }



    public IgameState apply(Card card)
    {
        var cardType = card.cardType;
        switch (cardType)
        {
            case CardType.DRAW1:
                this.currentPlayer.hand.AddRange(this.deck.draw(1));
                break;
            case CardType.DRAW2:
                this.players[this.nextPlayer()].hand.AddRange(this.deck.draw(2));
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.DRAW4:
                this.players[this.nextPlayer()].hand.AddRange(this.deck.draw(4));
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.SKIP:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                
                break;
            case CardType.REVERSE:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);

                break;
            case CardType.ZERO:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.ONE:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.TWO:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.THREE:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.FOUR:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.FIVE:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.SIX:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.SEVEN:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.EIGHT:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.NINE:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            case CardType.SELECTCOLOR:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);
                break;
            default:
                this.currentPlayer.hand.Remove(card);
                this.deck.discardPile.Push(card);

                break;
        }
        return this;
    }

    public IgameState apply(List<Card> Cards)
    {
        
        foreach (var card in Cards)
        {
            this.apply(card);
        }
        
        return this;
    }

    public int getCurrentPlayerIndex() => this.currentPlayerIndex;

    public Deck getDeck() => this.deck;

    public bool getPlayDirection() => this.playDirectionClockwise;

    public List<Player> GetPlayers() => this.players;

    public bool IsGameOver() => this.players.Count == 1;

    public int nextPlayer()
    {
        var indexIncrement = this.playDirectionClockwise ? 1 : -1;
        if ((this.currentPlayerIndex + indexIncrement) > this.players.Count - 1)
        {
            return 0;
        }
        if ((this.currentPlayerIndex + indexIncrement) < 0)
        {
            return this.players.Count - 1;
        }
        return this.currentPlayerIndex + indexIncrement;
    }

    public void run()
    {
        var notGameOver = true;
        while (notGameOver)
        {
            // TODO
            //currentPlayer.hand.AddRange(deck.draw(1));
            var action = this.currentPlayer.action(this);
            var newGameState = this.apply(action);
            this.players = newGameState.GetPlayers();
            this.deck = newGameState.getDeck();
            this.playDirectionClockwise = newGameState.getPlayDirection();
            if (this.currentPlayer.hand.Count == 0)
            {
                this.scoreBoard.Add(this.currentPlayer);
                this.players.Remove(this.currentPlayer);
            }
            this.currentPlayer = this.players[this.nextPlayer()];
            if (this.IsGameOver())
            {
                notGameOver = false;
            }
        }

    }
}
