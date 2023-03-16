public class Node
{
    Node? Parent = null;
    List<Node> Children = new List<Node>();

    GameState? GameState;

    List<Card> Action = new List<Card>();

    double Visits = 0;

    double Value = 0;

    public Node(Node? parent, List<Node> children, GameState? gameState, List<Card> action, double visits, double value)
    {
        Parent = parent;
        Children = children;
        GameState = gameState;
        Action = action;
        Visits = visits;
        Value = value;
    }

    public double getvalue(){
        return Value;
    }

    public double getVisits(){
        return Visits;
    }
    
    public void addChild(Node node){
        setParent(node);
        Children.Add(node);
    }

    public void setParent(Node node){
        node.Parent = this;
    }

    public bool isRoot(){
        return this.Parent == null ? true : false;
    }

    public bool isTerminal(){
        return GameState.IsGameOver();
    }

    public List<Node> getChildren(){
        return Children;
    }

    public Node? getParent(){
        return Parent;
    }

    public double getUCT(){
        if (Parent == null) return 0;
        double firstTerm = Value/Visits;
        double secondTerm = Math.Sqrt( Math.Log(Parent.Visits) / Visits );
        double constant = Math.Sqrt(2.0);
        return firstTerm + (constant * secondTerm);
    }

    public void update(double value){
        this.Value = this.Value + value;
        this.Visits = this.Visits + 1;
        if(isRoot()) return;
        Parent.update(value);
    }
}