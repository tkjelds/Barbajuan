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

    public GameState(GameState gamestate) 
    {
        this.players = gamestate.players;
        this.deck = gamestate.deck;
        this.currentPlayer = gamestate.currentPlayer;
        this.currentPlayerIndex = gamestate.currentPlayerIndex;
        this.scoreBoard = gamestate.scoreBoard;
        this.nextPlayerIndex = gamestate.nextPlayerIndex;
        this.playDirectionClockwise = gamestate.playDirectionClockwise;
    }



    public IgameState apply(Card card)
    {
        var gs = new GameState(this);
        var cardType = card.cardType;
        switch (cardType)
        {
            case CardType.DRAW1:
                gs.currentPlayer.hand.AddRange(gs.deck.draw(1));
                break;
            case CardType.DRAW2:
                gs.players[gs.nextPlayer()].hand.AddRange(gs.deck.draw(2));
                gs.currentPlayer.hand.Remove(card);
                gs.deck.discardPile.Push(card);
                break;
            case CardType.DRAW4:
                gs.players[gs.nextPlayer()].hand.AddRange(gs.deck.draw(4));
                gs.currentPlayer.hand.Remove(card);
                gs.deck.discardPile.Push(card);
                break;
            case CardType.SKIP:
                gs.currentPlayer.hand.Remove(card);
                gs.deck.discardPile.Push(card);
                
                nextPlayerIndex = nextPlayer(nextPlayerIndex);
                break;
            case CardType.REVERSE:
                gs.currentPlayer.hand.Remove(card);
                gs.deck.discardPile.Push(card);

                gs.playDirectionClockwise = !gs.playDirectionClockwise;
                break;
            
            default:
                gs.currentPlayer.hand.Remove(card);
                gs.deck.discardPile.Push(card);

                break;
        }
        return gs;
    }

    public IgameState apply(List<Card> Cards)
    {
        var gs = new GameState(this);
        
        foreach (var card in Cards)
        {
            gs.apply(card);
        }
        
        return gs;
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

    public int nextPlayer(int index) 
    {
        var indexIncrement = this.playDirectionClockwise ? 1 : -1;

        if ((index + indexIncrement) > this.players.Count - 1)
        {
            return 0;
        }
        if ((index + indexIncrement) < 0)
        {
            return this.players.Count - 1;
        }
        return index + indexIncrement;
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

            this.currentPlayer = players[nextPlayerIndex];
            this.currentPlayerIndex = nextPlayerIndex;

            if (this.IsGameOver())
            {
                notGameOver = false;
            }
        }

    }
}
