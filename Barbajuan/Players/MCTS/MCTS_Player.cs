public class MCTS_Player : Iplayer
{

    List<Card> Hand = new List<Card>();

    int Determinations = 0;

    int Iterations = 0;

    string Name = new Random().Next(int.MaxValue).ToString();

    IgameEvaluator Evaluator = new FactorialEvaluator();
    
    ImovePicker Picker = new RandomMovePicker();

    public MCTS_Player(List<Card> hand, int determinations, int iterations, string name, IgameEvaluator evaluator, ImovePicker picker)
    {
        Hand = hand;
        Determinations = determinations;
        Iterations = iterations;
        Name = name;
        Evaluator = evaluator;
        Picker = picker;
    }

    public void addCardsToHand(List<Card> cards)
    {
        Hand.AddRange(cards);
    }

    public List<Card> getHand()
    {
        return Hand;
    }

    public string getName()
    {
        return Name;
    }

    public void removeCardFromHand(Card cards)
    {
        Hand.Remove(cards);
    }
    
    public List<Card> action(IgameState gameState)
    {
        throw new NotImplementedException();
    }



    public Iplayer clone()
    {
        var clonedHand = new List<Card>();
        foreach(var card in this.Hand){
            clonedHand.Add(card.Clone());
        }
        var clonedPlayer = new FlatMonteCarloPlayer(clonedHand,this.Determinations,this.Iterations,this.Name,this.Evaluator,this.Picker);
        return clonedPlayer;
    }
}
