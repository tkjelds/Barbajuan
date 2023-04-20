public class Node
{
    Node? parent = null;
    List<Node> children = new List<Node>();

    GameState gameState {get;}

    List<Card> action = new List<Card>();

    double visits = 0;

    List<Double> value = new List<double>();

    int playerIndex = 0;

    public Node(Node? parent, List<Node> children, GameState gameState, List<Card> action, double visits, List<double> value, int playerIndex)
    {
        this.parent = parent;
        this.children = children;
        this.gameState = gameState;
        this.action = action;
        this.visits = visits;
        this.value = value;
        this.playerIndex = playerIndex;
    }

    public int GetPlayerIndex(){
        return playerIndex;
    }

    public bool IsLeaf(){
        return children.Count() == 0;
    }
    
    public GameState GetGameState(){
        return this.gameState;
    }

    public List<Double> Getvalue(){
        return value;
    }

    public double GetPlayerValue(int playerIndex){
        return value[playerIndex];
    }

    public double GetNodePlayerValue(){
        return value[playerIndex];
    }

    public double GetVisits(){
        return visits;
    }

    
    public void AddChild(Node node){
        SetParent(node);
        children.Add(node);
    }

    public void SetParent(Node node){
        node.parent = this;
    }

    public bool IsRoot(){
        return this.parent == null ? true : false;
    }

    public bool IsTerminal(){
        if(parent is null) return false;
        if(parent.GetGameState().GetPlayers().Count > this.gameState.GetPlayers().Count) return true;
        return false;
    }

    public List<Node> GetChildren(){
        return children;
    }

    public Node? GetParent(){
        return parent;
    }

    public List<Card> GetAction(){
        return action;
    }

    public double GetUCT(){
        double epsilon = 1e-6;
        var rng = new Random();
        double firstTerm =  value[playerIndex]/(visits+epsilon); // Change to not use parent playerindex (Done)
        double secondTerm = Math.Sqrt( Math.Log(parent!.visits+1.0) / (visits+epsilon) );
        double constant = Math.Sqrt(2.0);
        return firstTerm + (constant * secondTerm) + ( rng.NextDouble() + epsilon) ;
    }

    public void BackPropagate(double value, int playerIndex){
        this.value[playerIndex] = this.value[playerIndex] + value;
        this.visits = this.visits + 1;
        if(IsRoot()) return;
        parent!.BackPropagate(value,playerIndex);
    }

    public void Expand(){
        var topCard = gameState.GetDeck().GetTopCard();
        var hand = gameState.GetCurrentPlayer().GetHand();
        var legalMoves = new RandomMovePicker().GetLegalMoves(topCard,hand);
        foreach (var move in legalMoves)
        {
            var clonedGameState = gameState.Clone();
            clonedGameState.ApplyNoClone(move);
            var expandedNode = new Node(null,new List<Node>(),clonedGameState,move,0.0,CreateEmptyValueList(),gameState.GetCurrentPlayerIndex()); // change to 
            this.AddChild(expandedNode);
        }
    }

    public void Expand(ImovePicker picker){
        var topCard = gameState.GetDeck().GetTopCard();
        var hand = gameState.GetCurrentPlayer().GetHand();
        var legalMoves = picker.GetLegalMoves(topCard,hand);
        foreach (var move in legalMoves)
        {
            var clonedGameState = gameState.Clone();
            clonedGameState.ApplyNoClone(move);
            var expandedNode = new Node(null,new List<Node>(),clonedGameState,move,0,CreateEmptyValueList(),clonedGameState.GetCurrentPlayerIndex());
            this.AddChild(expandedNode);
        }
    }

    public List<double> CreateEmptyValueList(){
        List<Double> valueList = new List<double>();
        for (int i = 0; i < gameState.GetPlayers().Count(); i++)
        {
            valueList.Add(0);
        }
        return valueList;
    }

}