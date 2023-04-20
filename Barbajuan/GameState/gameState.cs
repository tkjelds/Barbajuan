public class GameState
{
    private Iplayer currentPlayer;
    private int nextPlayerIndex;
    private int currentPlayerIndex = 0;
    private List<Iplayer> players;
    private Deck deck;
    private bool playDirectionClockwise = true;
    private List<Iplayer> scoreBoard;

    public GameState(){}
    
    public GameState(List<Iplayer> players)
    {
        this.players = players;
        var deck = new Deck();
        deck.Setup();
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, int currentPlayerIndex)
    {
        this.players = players;
        var deck = new Deck();
        deck.Setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, int currentPlayerIndex, bool playDirection)
    {
        this.players = players;
        var deck = new Deck();
        deck.Setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, bool playDirection)
    {
        this.players = players;
        var deck = new Deck();
        deck.Setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, Deck deck)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }
    public GameState(List<Iplayer> players, Deck deck, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(Iplayer currentPlayer, int currentPlayerIndex, List<Iplayer> players, Deck deck)
    {
        this.currentPlayer = currentPlayer;
        this.players = players;
        this.deck = deck;
        this.scoreBoard = new List<Iplayer>();
        this.currentPlayerIndex = currentPlayerIndex;
        this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, Deck deck, Iplayer currentPlayer, int currentPlayerIndex, int nextPlayerIndex, List<Iplayer> scoreBoard, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = currentPlayer;
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = scoreBoard;
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayerIndex;
    }

    public GameState Apply(GameState gs, Card card)
    {
        var foundCard = gs.currentPlayer.GetHand().Find(c => c.cardColor == card.cardColor && c.cardType == card.cardType);
        switch (card.cardType)
        {
            case CardType.DRAW1:
                gs.currentPlayer.AddCardsToHand(gs.deck.Draw(1));
                break;
            case CardType.DRAW2: // Tested
                gs.players[NextPlayer(gs.currentPlayerIndex, gs)]
                .AddCardsToHand(gs.deck.Draw(2));

                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
            case CardType.DRAW4: // Tested
                gs.players[NextPlayer(gs.currentPlayerIndex, gs)].AddCardsToHand(gs.deck.Draw(4));
                foundCard = gs.currentPlayer.GetHand().Find(c => c.cardType == CardType.DRAW4);
                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            case CardType.SKIP: // Tested
                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.nextPlayerIndex = NextPlayer(gs.nextPlayerIndex, gs);

                break;
            case CardType.REVERSE: // Tested
                //If the number of players are 2, reverse functions like a skip card.
                if (gs.GetPlayers().Count() == 2)
                {
                    gs.currentPlayer.RemoveCardFromHand(foundCard!);
                    gs.deck.discardPile.Push(foundCard!);

                    gs.nextPlayerIndex = NextPlayer(gs.nextPlayerIndex, gs);
                    break;
                }
                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.playDirectionClockwise = !gs.playDirectionClockwise;
                gs.nextPlayerIndex = NextPlayer(gs.currentPlayerIndex, gs);
                break;
            case CardType.SELECTCOLOR:
                foundCard = gs.currentPlayer.GetHand().Find(c => c.cardType == CardType.SELECTCOLOR);
                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            default:
                gs.currentPlayer.RemoveCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
        }
        return gs;
    }


// TODO Rename to applyWithClone ELLER NOGET!
    public GameState Apply(List<Card> cards)
    {
        GameState gs = Clone();

        gs.ApplyNoClone(cards);

        return gs;
    }

    public GameState ApplyNoClone(List<Card> cards)
    {

        foreach (var card in cards)
        {
            Apply(this, card);
        }
        if (currentPlayer.GetHand().Count() == 0)
        {
            if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { nextPlayerIndex = NextPlayer(nextPlayerIndex, this); }
            PlayerKnockOut(this.currentPlayer);
            return this;
        }


        if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { nextPlayerIndex = NextPlayer(nextPlayerIndex, this); }
        if (players.Count() > 1)
        {
            this.currentPlayer = this.players[PrevPlayer(NextPlayer(nextPlayerIndex,this),this)];
            currentPlayerIndex = nextPlayerIndex;
            nextPlayerIndex = NextPlayer();
        }

        return this;
    }

    public void PlayerKnockOut(Iplayer player)
    {
        var amountOfPlayersBeforeRemove = players.Count();
        this.players.Remove(player);
        this.scoreBoard.Add(player);
        if (this.GetCurrentPlayerIndex() == 0 && this.playDirectionClockwise)
        {
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.GetCurrentPlayerIndex() == 0 && !this.playDirectionClockwise)
        {
            this.currentPlayerIndex = NextPlayer(this.currentPlayerIndex, this);
            this.nextPlayerIndex = NextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.GetCurrentPlayerIndex() == amountOfPlayersBeforeRemove - 1 && this.playDirectionClockwise)
        {
            if (this.nextPlayerIndex > this.GetPlayers().Count() - 1) this.currentPlayerIndex = 0;
            else this.currentPlayerIndex = this.nextPlayerIndex;


            this.nextPlayerIndex = NextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.GetCurrentPlayerIndex() == amountOfPlayersBeforeRemove - 1 && !this.playDirectionClockwise)
        {
            this.currentPlayerIndex = NextPlayer(currentPlayerIndex, this);
            this.nextPlayerIndex = NextPlayer(currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (!playDirectionClockwise)
        {
            this.currentPlayerIndex = NextPlayer(this.currentPlayerIndex, this);
            this.nextPlayerIndex = NextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }


    }
    public int GetCurrentPlayerIndex() => this.currentPlayerIndex;

    public Deck GetDeck() => this.deck;

    public bool GetPlayDirection() => this.playDirectionClockwise;

    public List<Iplayer> GetPlayers() => this.players;

    public bool IsGameOver() => this.players.Count == 1;

    public int GetNextPlayerIndex() => this.nextPlayerIndex;

    public List<Iplayer> GetScoreBoard() => this.scoreBoard;

    public Iplayer GetCurrentPlayer() => this.currentPlayer;
    public int NextPlayer()
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

    public int NextPlayer(int index, GameState gs)
    {
        var indexIncrement = gs.playDirectionClockwise ? 1 : -1;

        if ((index + indexIncrement) > (gs.players.Count() - 1))
        {
            return 0;
        }
        if ((index + indexIncrement) < 0)
        {
            return (gs.players.Count() - 1);
        }
        return index + indexIncrement;
    }
    public int PrevPlayer(int index, GameState gs)
    {
        var indexIncrement = gs.playDirectionClockwise ? -1 : 1;

        if ((index + indexIncrement) > (gs.players.Count() - 1))
        {
            return 0;
        }
        if ((index + indexIncrement) < 0)
        {
            return (gs.players.Count() - 1);
        }
        return index + indexIncrement;
    }
    public void Update(GameState gs)
    {
        this.currentPlayer = gs.GetCurrentPlayer();
        this.nextPlayerIndex = gs.GetNextPlayerIndex();
        this.currentPlayerIndex = gs.GetCurrentPlayerIndex(); ;
        this.players = gs.GetPlayers();
        this.deck = gs.GetDeck();
        this.playDirectionClockwise = gs.GetPlayDirection();
        this.scoreBoard = gs.GetScoreBoard();
    }


    public void DealSevenToEachPlayer(){
            foreach (var player in players)
        {
            player.AddCardsToHand(deck.Draw(7));
        }
    }
    public void Run()
    {
        DealSevenToEachPlayer();
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {
            counter++;
            // TODO
            Console.WriteLine("Current player: " + this.currentPlayer.GetName());
            var action = this.currentPlayer.Action(this);
            var newGameState = this.Apply(action);
            foreach (var card in action)
            {
                Console.WriteLine("Current Play: " + card.cardColor.ToString() + "   " + card.cardType.ToString());
            }
            Console.WriteLine();
            
            Console.WriteLine("Number of plays: " + counter);

            Update(newGameState);
            if (IsGameOver())
            {
                notGameOver = false;
                Console.WriteLine("Loser: " + newGameState.GetPlayers()[NextPlayer(GetNextPlayerIndex(), (GameState)newGameState)].GetName());
            }

        }

        for (int playerIndex = 0; playerIndex < scoreBoard.Count(); playerIndex++)
        {
            Console.WriteLine("Placement :" + (playerIndex + 1) + "   Name :" + scoreBoard[playerIndex].GetName());
        }

    }


public int RunReturnNumberOfTurns()
    {
        DealSevenToEachPlayer();
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {
            counter++;
            var action = this.currentPlayer.Action(this);
            var newGameState = this.ApplyNoClone(action);
            if (IsGameOver())
            {
                scoreBoard.Add(players[0]);
                notGameOver = false;
            }

        }

        return counter;
    }
    public List<Iplayer> RunReturnScoreBoard()
    {
        foreach (var player in players)
        {
            player.AddCardsToHand(deck.Draw(7));
        }
        var counter = 0;
        var notGameOver = true;

        while (notGameOver)
        {
            counter++;
            
            var action = this.currentPlayer.Action(this);

            var newGameState = this.ApplyNoClone(action);

            if (IsGameOver())
            {
                scoreBoard.Add(players[0]);
                notGameOver = false;
            }

        }

        return scoreBoard;
    }

    public List<(int, String, List<Card>)> RunReturnTimeline()
    {
        foreach (var player in players)
        {
            player.AddCardsToHand(deck.Draw(7));
        }
        var counter = 0;
        var notGameOver = true;

        var timeLine = new List<(int, String, List<Card>)>();
        while (notGameOver)
        {
            counter++;
            
            var action = this.currentPlayer.Action(this);
            timeLine.Add((counter, this.currentPlayer.GetName(), action));
            
            var newGameState = this.ApplyNoClone(action);

            if (IsGameOver())
            {
                scoreBoard.Add(players[0]);
                notGameOver = false;
            }

        }

        return timeLine;
    }

    /*
    private Iplayer currentPlayer;
    private int nextPlayerIndex;
    private int currentPlayerIndex = 0;
    private List<Iplayer> players;
    private Deck deck;
    private bool playDirectionClockwise = true;
    private List<Iplayer> scoreBoard;
    */
    public GameState Clone(){ // TODO

        var clonedListOfPlayers = new List<Iplayer>();

        foreach (var player in players)
        {
            clonedListOfPlayers.Add(player.Clone());
        }

        var clonedScoreboard = new List<Iplayer>();

        foreach (var player in scoreBoard)
        {
            clonedScoreboard.Add(player.Clone());
        }
        
        
        var clonedCurrentPlayerIndex = EnsureValidIndex(this.currentPlayerIndex);
        var clonedNextPlayerIndex = EnsureValidIndex(this.nextPlayerIndex); 

        var clonedCurrentPlayer = clonedListOfPlayers[clonedCurrentPlayerIndex];
        
        var clonedDeck = this.deck.Clone();

        // List<Iplayer> players, Deck deck, Iplayer currentPlayer, int currentPlayerIndex, int nextPlayerIndex, List<Iplayer> scoreBoard, bool playDirection
        var clonedGS = new GameState(clonedListOfPlayers,clonedDeck,clonedCurrentPlayer,clonedCurrentPlayerIndex,clonedNextPlayerIndex,clonedScoreboard,this.playDirectionClockwise);
    
        return clonedGS;
    }

    public int EnsureValidIndex(int index) {
        return PrevPlayer(NextPlayer(index,this),this);
    }
}
