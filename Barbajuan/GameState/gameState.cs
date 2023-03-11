using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
[Serializable]
public class GameState : IgameState
{
    private Iplayer currentPlayer;
    private int nextPlayerIndex;
    private int currentPlayerIndex = 0;
    private List<Iplayer> players;
    private Deck deck;
    private bool playDirectionClockwise = true;
    private List<Iplayer> scoreBoard;

    public GameState(List<Iplayer> players)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, int currentPlayerIndex)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.currentPlayerIndex = currentPlayerIndex;
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, int currentPlayerIndex, bool playDirection)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, bool playDirection)
    {
        this.players = players;
        var deck = new Deck();
        deck.setup();
        this.deck = deck;
        this.currentPlayer = players[currentPlayerIndex];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(List<Iplayer> players, Deck deck)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }
    public GameState(List<Iplayer> players, Deck deck, bool playDirection)
    {
        this.players = players;
        this.deck = deck;
        this.currentPlayer = players[0];
        this.scoreBoard = new List<Iplayer>();
        this.playDirectionClockwise = playDirection;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
    }

    public GameState(Iplayer currentPlayer, int currentPlayerIndex, List<Iplayer> players, Deck deck)
    {
        this.currentPlayer = currentPlayer;
        this.players = players;
        this.deck = deck;
        this.scoreBoard = new List<Iplayer>();
        this.currentPlayerIndex = currentPlayerIndex;
        this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
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

    public IgameState apply(GameState gs, Card card)
    {
        var foundCard = gs.currentPlayer.getHand().Find(c => c.cardColor == card.cardColor && c.cardType == card.cardType);
        switch (card.cardType)
        {
            case CardType.DRAW1:
                gs.currentPlayer.addCardsToHand(gs.deck.draw(1));
                break;
            case CardType.DRAW2: // Tested
                gs.players[nextPlayer(gs.currentPlayerIndex, gs)]
                .addCardsToHand(gs.deck.draw(2));

                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
            case CardType.DRAW4: // Tested
                gs.players[nextPlayer(gs.currentPlayerIndex, gs)].addCardsToHand(gs.deck.draw(4));
                foundCard = gs.currentPlayer.getHand().Find(c => c.cardType == CardType.DRAW4);
                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            case CardType.SKIP: // Tested
                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.nextPlayerIndex = nextPlayer(gs.nextPlayerIndex, gs);

                break;
            case CardType.REVERSE: // Tested
                //If the number of players are 2, reverse functions like a skip card.
                if (gs.GetPlayers().Count() == 2)
                {
                    gs.currentPlayer.removeCardFromHand(foundCard!);
                    gs.deck.discardPile.Push(foundCard!);

                    gs.nextPlayerIndex = nextPlayer(gs.nextPlayerIndex, gs);
                    break;
                }
                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                gs.playDirectionClockwise = !gs.playDirectionClockwise;
                gs.nextPlayerIndex = nextPlayer(gs.currentPlayerIndex, gs);
                break;
            case CardType.SELECTCOLOR:
                foundCard = gs.currentPlayer.getHand().Find(c => c.cardType == CardType.SELECTCOLOR);
                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(card);
                break;
            default:
                gs.currentPlayer.removeCardFromHand(foundCard!);
                gs.deck.discardPile.Push(foundCard!);
                break;
        }
        return gs;
    }

    public IgameState apply(List<Card> cards)
    {
        //GameState gs = DeepClone<GameState>(this);
        GameState gs = Clone();
        foreach (var card in cards)
        {
            gs.apply(gs, card);
        }
        if (gs.currentPlayer.getHand().Count() == 0)
        {
            if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { gs.nextPlayerIndex = gs.nextPlayer(gs.nextPlayerIndex, gs); }
            gs.playerKnockOut(gs.currentPlayer);
            return gs;
        }


        if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { gs.nextPlayerIndex = gs.nextPlayer(gs.nextPlayerIndex, gs); }
        if (gs.players.Count() > 1)
        {
            gs.currentPlayer = gs.players[prevPlayer(nextPlayer(nextPlayerIndex,gs),gs)];
            gs.currentPlayerIndex = gs.nextPlayerIndex;
            gs.nextPlayerIndex = gs.nextPlayer();
        }

        return gs;
    }

    public IgameState applyNoClone(List<Card> cards)
    {

        foreach (var card in cards)
        {
            apply(this, card);
        }
        if (currentPlayer.getHand().Count() == 0)
        {
            if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { nextPlayerIndex = nextPlayer(nextPlayerIndex, this); }
            playerKnockOut(this.currentPlayer);
            return this;
        }


        if (cards.First().cardType == CardType.DRAW2 || cards.First().cardType == CardType.DRAW4) { nextPlayerIndex = nextPlayer(nextPlayerIndex, this); }
        if (players.Count() > 1)
        {
            this.currentPlayer = this.players[prevPlayer(nextPlayer(nextPlayerIndex,this),this)];
            currentPlayerIndex = nextPlayerIndex;
            nextPlayerIndex = nextPlayer();
        }

        return this;
    }

    public void playerKnockOut(Iplayer player)
    {
        //Console.WriteLine(" i have been removed from the game, name: " + player.getName());
        var amountOfPlayersBeforeRemove = players.Count();
        this.players.Remove(player);
        this.scoreBoard.Add(player);
        if (this.getCurrentPlayerIndex() == 0 && this.playDirectionClockwise)
        {
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.getCurrentPlayerIndex() == 0 && !this.playDirectionClockwise)
        {
            this.currentPlayerIndex = nextPlayer(this.currentPlayerIndex, this);
            this.nextPlayerIndex = nextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.getCurrentPlayerIndex() == amountOfPlayersBeforeRemove - 1 && this.playDirectionClockwise)
        {
            if (this.nextPlayerIndex > this.GetPlayers().Count() - 1) this.currentPlayerIndex = 0;
            else this.currentPlayerIndex = this.nextPlayerIndex;


            this.nextPlayerIndex = nextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (this.getCurrentPlayerIndex() == amountOfPlayersBeforeRemove - 1 && !this.playDirectionClockwise)
        {
            this.currentPlayerIndex = nextPlayer(currentPlayerIndex, this);
            this.nextPlayerIndex = nextPlayer(currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }
        if (!playDirectionClockwise)
        {
            this.currentPlayerIndex = nextPlayer(this.currentPlayerIndex, this);
            this.nextPlayerIndex = nextPlayer(this.currentPlayerIndex, this);
            this.currentPlayer = this.players[currentPlayerIndex];
            return;
        }


    }
    public int getCurrentPlayerIndex() => this.currentPlayerIndex;

    public Deck getDeck() => this.deck;

    public bool getPlayDirection() => this.playDirectionClockwise;

    public List<Iplayer> GetPlayers() => this.players;

    public bool IsGameOver() => this.players.Count == 1;

    public int getNextPlayerIndex() => this.nextPlayerIndex;

    public List<Iplayer> getScoreBoard() => this.scoreBoard;

    public Iplayer getCurrentPlayer() => this.currentPlayer;
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
    public int prevPlayer(int index, GameState gs)
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
    public void update(IgameState gs)
    {
        this.currentPlayer = gs.getCurrentPlayer();
        this.nextPlayerIndex = gs.getNextPlayerIndex();
        this.currentPlayerIndex = gs.getCurrentPlayerIndex(); ;
        this.players = gs.GetPlayers();
        this.deck = gs.getDeck();
        this.playDirectionClockwise = gs.getPlayDirection();
        this.scoreBoard = gs.getScoreBoard();
    }


    public void DealSevenToEachPlayer(){
            foreach (var player in players)
        {
            player.addCardsToHand(deck.draw(7));
        }
    }
    public void run()
    {
        DealSevenToEachPlayer();
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {
            counter++;
            // TODO
            Console.WriteLine("Current player: " + this.currentPlayer.getName());
            var action = this.currentPlayer.action(this);
            var newGameState = this.apply(action);
            foreach (var card in action)
            {
                Console.WriteLine("Current Play: " + card.cardColor.ToString() + "   " + card.cardType.ToString());
            }
            Console.WriteLine();
            // Console.WriteLine("Amount of cards in hand: " + currentPlayer.hand.Count());
            // Console.WriteLine("Current top card of discard pile: " + newGameState.getDeck().discardPile.Peek().ToString());
            // Console.WriteLine("");
            Console.WriteLine("Number of plays: " + counter);
            // Console.WriteLine("");
            update(newGameState);
            if (IsGameOver())
            {
                notGameOver = false;
                Console.WriteLine("Loser: " + newGameState.GetPlayers()[nextPlayer(getNextPlayerIndex(), (GameState)newGameState)].getName());
            }

        }

        for (int playerIndex = 0; playerIndex < scoreBoard.Count(); playerIndex++)
        {
            Console.WriteLine("Placement :" + (playerIndex + 1) + "   Name :" + scoreBoard[playerIndex].getName());
        }

    }


public int runReturnNumberOfTurns()
    {
        DealSevenToEachPlayer();
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {
            counter++;
            var action = this.currentPlayer.action(this);
            var newGameState = this.applyNoClone(action);
            if (IsGameOver())
            {
                scoreBoard.Add(players[0]);
                notGameOver = false;
            }

        }

        return counter;
    }
    public List<Iplayer> runReturnScoreBoard()
    {
        foreach (var player in players)
        {
            player.addCardsToHand(deck.draw(7));
        }
        var counter = 0;
        var notGameOver = true;
        while (notGameOver)
        {
            counter++;
            // TODO
            //Console.WriteLine("Current player: " + this.currentPlayer.getName());
            var action = this.currentPlayer.action(this);
            var newGameState = this.applyNoClone(action);
            // foreach (var card in action)
            // {
            //     Console.WriteLine("Current Play: " + card.cardColor.ToString() + "   " + card.cardType.ToString());
            // }
            // Console.WriteLine();
            // Console.WriteLine("Amount of cards in hand: " + currentPlayer.hand.Count());
            // Console.WriteLine("Current top card of discard pile: " + newGameState.getDeck().discardPile.Peek().ToString());
            // Console.WriteLine("");
            // Console.WriteLine("Number of plays: " + counter);
            // Console.WriteLine("");
            //update(newGameState);
            if (IsGameOver())
            {
                scoreBoard.Add(players[0]);
                notGameOver = false;
                // Console.WriteLine("Loser: " + newGameState.GetPlayers()[nextPlayer(getNextPlayerIndex(), (GameState)newGameState)].getName());
            }

        }

        // for (int playerIndex = 0; playerIndex < scoreBoard.Count(); playerIndex++)
        // {
        //     Console.WriteLine("Placement :" + (playerIndex + 1) + "   Name :" + scoreBoard[playerIndex].getName());
        // }
        return scoreBoard;
    }

//     public static T DeepCloneJson<T>(T self)
//     {
//         var serialized = JsonConvert.SerializeObject(self);
//         return JsonConvert.DeserializeObject<T>(serialized);
//     }

//     public T DeepClone<T>(T obj)
//     {
//         using (var ms = new MemoryStream())
//         {
//             var formatter = new BinaryFormatter();
// #pragma warning disable SYSLIB0011
//             formatter.Serialize(ms, obj);
//             ms.Position = 0;
//             return (T)formatter.Deserialize(ms);
//         }
//     }

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
            clonedListOfPlayers.Add(player.clone());
        }

        var clonedScoreboard = new List<Iplayer>();

        foreach (var player in scoreBoard)
        {
            clonedScoreboard.Add(player.clone());
        }
        var clonedCurrentPlayer = clonedListOfPlayers[this.currentPlayerIndex];

        var clonedDeck = this.deck.Clone();

        // List<Iplayer> players, Deck deck, Iplayer currentPlayer, int currentPlayerIndex, int nextPlayerIndex, List<Iplayer> scoreBoard, bool playDirection
        var clonedGS = new GameState(clonedListOfPlayers,clonedDeck,clonedCurrentPlayer,this.currentPlayerIndex,this.nextPlayerIndex,clonedScoreboard,this.playDirectionClockwise);
    
        return clonedGS;
    }
}
