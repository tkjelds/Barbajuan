public class gameState : IgameState
{
    Player currentPlayer; 
    int currentPlayerIndex = 0;
    List<Player> players;
    Deck deck;
    bool playDirectionClockwise = true;
    List<Player> scoreBoard;
    public gameState(List<Player> players)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
    }

    public gameState(List<Player> players, Deck deck) 
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
    }
    public gameState(List<Player> players, Deck deck, bool playDirection) 
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Player>();
        this.playDirectionClockwise = playDirection;
    }

    public gameState(Player currentPlayer, int currentPlayerIndex, List<Player> players, Deck deck)
    {
        this.currentPlayer = currentPlayer;
        this.players = players;
        this.deck = deck;
        this.scoreBoard = new List<Player>();
        this.currentPlayerIndex = currentPlayerIndex;
    }

    public gameState(List<Player> players, Deck deck, Player currentPlayer, int currentPlayerIndex, List<Player>  scoreBoard, bool playDirection){
        this.players = players; 
        this.deck = deck;
        this.currentPlayer = currentPlayer;
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = scoreBoard;
        this.playDirectionClockwise = playDirection;
    }   
    
    

    public IgameState apply(Card card)
    {
        throw new NotImplementedException();
    }

    public IgameState apply(List<Card> Cards)
    {
        throw new NotImplementedException();
    }

    public int getCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public Deck getDeck()
    {
        return this.deck;
    }

    public bool getPlayDirection()
    {
        return playDirectionClockwise;
    }

    public List<Player> GetPlayers()
    {
        return this.players;
    }

    public bool IsGameOver()
    {
        return (this.players.Count() == 1) ? true : false;
    }

    public int nextPlayer()
    {
        var indexIncrement = (playDirectionClockwise) ?  1 :  -1;
        if((currentPlayerIndex + indexIncrement) > players.Count()-1) {
            return 0;
        }
        if((currentPlayerIndex + indexIncrement) < 0) {
            return players.Count()-1;
        }
        return currentPlayerIndex + indexIncrement;
    }

    public void run()
    {
        bool notGameOver = true;
        while(notGameOver){
            // TODO
            //currentPlayer.hand.AddRange(deck.draw(1));
            var action = currentPlayer.action(this);
            var newgameState = apply(action);
            this.players = newgameState.GetPlayers();
            this.deck = newgameState.getDeck();
            this.playDirectionClockwise = newgameState.getPlayDirection();
            if (currentPlayer.hand.Count() == 0) {
                scoreBoard.Add(currentPlayer);
                players.Remove(currentPlayer);
            }
            this.currentPlayer = players[nextPlayer()];
            if (IsGameOver()) notGameOver = false;
        }
    }
}