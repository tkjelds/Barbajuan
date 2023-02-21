using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameState : IgameState
{
    private Player currentPlayer;
    private int nextPlayerIndex;
    private int currentPlayerIndex = 0;
    private List<Player> players;
    private Deck deck;
    private bool playDirectionClockwise = true;
    private List<Player> scoreBoard;

    public GameState(List<Player> players)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Player> players, Deck deck)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }
    public GameState(List<Player> players, Deck deck, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex,this );
    }

    public GameState(Player currentPlayer, int currentPlayerIndex, List<Player> players, Deck deck)
    {
        this.currentPlayer = currentPlayer;
        this.players = players;
        this.deck = deck;
        this.scoreBoard = new List<Player>();
        this.currentPlayerIndex = currentPlayerIndex;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Player> players, Deck deck, Player currentPlayer, int currentPlayerIndex, int nextPlayerIndex, List<Player> scoreBoard, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = currentPlayer;
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = scoreBoard;
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(GameState gamestate) 
    {
        this.players = gamestate.players;
        this.deck = new Deck(new Stack<Card> (gamestate.deck.drawPile), new Stack<Card> (gamestate.deck.discardPile));
        this.currentPlayer = (Player) gamestate.currentPlayer.Clone();
        this.currentPlayerIndex = gamestate.currentPlayerIndex;
        this.scoreBoard = gamestate.scoreBoard;
        this.nextPlayerIndex = gamestate.nextPlayerIndex;
        this.playDirectionClockwise = gamestate.playDirectionClockwise;
    }


public IgameState apply(GameState gs, Card card)
    {   
        var foundCard = gs.currentPlayer.hand.Find(c => c.cardColor == card.cardColor && c.cardType == card.cardType);
        switch (card.cardType)
        {
            case CardType.DRAW1:
                gs.currentPlayer.hand.AddRange(gs.deck.draw(1));
                break;
            case CardType.DRAW2: // Tested
                gs.players[nextPlayer(gs.currentPlayerIndex, gs)]
                .hand.AddRange(gs.deck.draw(2));
                
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
            case CardType.DRAW4: // Tested
                gs.players[nextPlayer(gs.currentPlayerIndex, gs)].hand.AddRange(gs.deck.draw(4));
                foundCard = gs.currentPlayer.hand.Find(c => c.cardType == CardType.DRAW4);
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            case CardType.SKIP: // Tested
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.nextPlayerIndex = nextPlayer(gs.nextPlayerIndex, gs);

                break;
            case CardType.REVERSE: // Tested
                //If the number of players are 2, reverse functions like a skip card.
                if(gs.GetPlayers().Count() == 2) {
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                
                gs.nextPlayerIndex = nextPlayer(gs.nextPlayerIndex, gs);
                break;
                } 
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.playDirectionClockwise = !gs.playDirectionClockwise;
                gs.nextPlayerIndex = nextPlayer(gs.currentPlayerIndex, gs);
                break;
            case CardType.SELECTCOLOR: 
                foundCard = gs.currentPlayer.hand.Find(c => c.cardType == CardType.SELECTCOLOR);
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            default:
                gs.currentPlayer.hand.Remove(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
        }
        return gs;
    }

    public IgameState apply(List<Card> cards)
    {
        GameState gs = DeepClone<GameState>(this);
       


        if(gs.currentPlayer.hand.Count() == 0)  {
            gs.scoreBoard.Add(gs.currentPlayer);
            gs.players.Remove(gs.currentPlayer);
            if(gs.currentPlayerIndex == gs.nextPlayerIndex) {
                gs.nextPlayerIndex = gs.nextPlayer(gs.nextPlayerIndex, gs);
            }
            if(gs.players.Count() == 1) return gs;
            gs.currentPlayerIndex = gs.nextPlayer(gs.currentPlayerIndex,gs);
            gs.nextPlayerIndex = gs.nextPlayer(gs.currentPlayerIndex,gs);
            gs.currentPlayer = gs.players[gs.currentPlayerIndex];
        
            return gs;
        }  
    

        foreach (var card in cards)
        {
            gs.apply(gs,card);
        }
        



        if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) {gs.nextPlayerIndex = gs.nextPlayer(gs.nextPlayerIndex, gs);}
        if(gs.players.Count() > 1) {  
        gs.currentPlayer = gs.players[gs.nextPlayerIndex];
        gs.currentPlayerIndex = gs.nextPlayerIndex;
        gs.nextPlayerIndex = gs.nextPlayer();
        } 

        return gs;
    }

    public int getCurrentPlayerIndex() => this.currentPlayerIndex;

    public Deck getDeck() => this.deck;

    public bool getPlayDirection() => this.playDirectionClockwise;

    public List<Player> GetPlayers() => this.players;

    public bool IsGameOver() => this.players.Count == 1;

    public int getNextPlayerIndex() => this.nextPlayerIndex;

    public List<Player> getScoreBoard() => this.scoreBoard;

    public Player getCurrentPlayer() => this.currentPlayer;
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

    public int nextPlayer(int index, GameState gs) 
    {
        var indexIncrement = gs.playDirectionClockwise ? 1 : -1;

        if ((index + indexIncrement) > gs.players.Count - 1)
        {
            return 0;
        }
        if ((index + indexIncrement) < 0)
        {
            return gs.players.Count - 1;
        }
        return index + indexIncrement;
    } 

    public void update(IgameState gs){
        this.currentPlayer = gs.getCurrentPlayer();
        this.nextPlayerIndex = gs.getNextPlayerIndex();
        this.currentPlayerIndex = gs.getCurrentPlayerIndex();;
        this.players = gs.GetPlayers();
        this.deck = gs.getDeck();
        this.playDirectionClockwise = gs.getPlayDirection();
        this.scoreBoard = gs.getScoreBoard();
    }

    public void run()
    {
        foreach (var player in players)
        {
            player.hand.AddRange(deck.draw(7));
        }
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {   
            counter++;
            // TODO
            //Console.WriteLine("Current player: " + this.currentPlayerIndex);
            var action = this.currentPlayer.action(this);
            var newGameState = this.apply(action);
            foreach (var card in action)
            {
                //Console.WriteLine("Current Play: " + card.cardColor.ToString() + "   " + card.cardType.ToString());
            }
            // Console.WriteLine("Amount of cards in hand: " + currentPlayer.hand.Count());
            // Console.WriteLine("Current top card of discard pile: " + newGameState.getDeck().discardPile.Peek().ToString());
            // Console.WriteLine("");
            // Console.WriteLine("Number of plays: " + counter);
            // Console.WriteLine("");
            update(newGameState);
            if (newGameState.IsGameOver())
            {
                notGameOver = false;
                Console.WriteLine("Loser: " + newGameState.getNextPlayerIndex());
            }

        }

    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            #pragma warning disable SYSLIB0011
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }
}
