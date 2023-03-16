public class Node
{
    Node? Parent = null;
    List<Node> Children = new List<Node>();

    GameState GameState;

    List<Card> Action = new List<Card>();

    double Visits = 0;

    List<Double> Value = new List<double>();

    int PlayerIndex = 0;

    public Node(Node? parent, List<Node> children, GameState gameState, List<Card> action, double visits, List<double> value, int playerIndex)
    {
        Parent = parent;
        Children = children;
        GameState = gameState;
        Action = action;
        Visits = visits;
        Value = value;
        PlayerIndex = playerIndex;
    }

    public bool isLeaf(){
        return Children.Count() == 0;
    }
    
    public List<Double> getvalue(){
        return Value;
    }

    public double getPlayerValue(int playerIndex){
        return Value[playerIndex];
    }

    public double getNodePlayerValue(){
        return Value[PlayerIndex];
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
        double firstTerm = Value[PlayerIndex]/Visits;
        double secondTerm = Math.Sqrt( Math.Log(Parent.Visits) / Visits );
        double constant = Math.Sqrt(2.0);
        return firstTerm + (constant * secondTerm);
    }

    public void update(double value, int playerIndex){
        this.Value[playerIndex] = this.Value[playerIndex] + value;
        this.Visits = this.Visits + 1;
        if(isRoot()) return;
        Parent.update(value,playerIndex);
    }

    public void expand(){
        var topCard = GameState.getDeck().drawPile.Peek();
        var hand = GameState.getCurrentPlayer().getHand();
        var legalMoves = getStackingActions(topCard,hand);
        foreach (var move in legalMoves)
        {
            var clonedGameState = GameState.Clone();
            clonedGameState.applyNoClone(move);
            var expandedNode = new Node(null,new List<Node>(),clonedGameState,move,1,createEmptyValueList(),clonedGameState.getCurrentPlayerIndex());
            this.addChild(expandedNode);
        }
    }

        public List<List<Card>> getStackingActions(Card topCard, List<Card> hand)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
        {
            if (card.canBePlayedOn(topCard))
            {
                if (card.cardType == DRAW4)
                {
                    moves.Add(new List<Card>() { new Card(GREEN, DRAW4) });
                    moves.Add(new List<Card>() { new Card(BLUE, DRAW4) });
                    moves.Add(new List<Card>() { new Card(YELLOW, DRAW4) });
                    moves.Add(new List<Card>() { new Card(RED, DRAW4) });
                }
                if (card.cardType == SELECTCOLOR)
                {
                    moves.Add(new List<Card>() { new Card(GREEN, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(BLUE, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(YELLOW, SELECTCOLOR) });
                    moves.Add(new List<Card>() { new Card(RED, SELECTCOLOR) });
                }
                if (card.cardType != SELECTCOLOR && card.cardType != DRAW4)
                {
                    var nextHand = new List<Card>(hand);
                    moves.Add(new List<Card>() { card });
                    nextHand.Remove(card);
                    moves.AddRange(getCardOfSameType(card, nextHand, new List<Card>() { card }));
                }

            }
        }
        return moves;
    }
    public List<List<Card>> getCardOfSameType(Card toBePlayedOn, List<Card> hand, List<Card> currentStack)
    {
        var moves = new List<List<Card>>();
        foreach (var card in new List<Card>(hand))
        {
            var nextHand = new List<Card>(hand);
            if (card.cardType == toBePlayedOn.cardType)
            {
                var moveStack = new List<Card>(currentStack);
                moveStack.Add(card);
                moves.Add(moveStack);
                nextHand.Remove(card);
                moves.AddRange(getCardOfSameType(card, nextHand, moveStack));

            }
        }
        return moves;
    }

    public List<double> createEmptyValueList(){
        var playerCount = GameState.GetPlayers().Count() + GameState.getScoreBoard().Count();
        List<Double> valueList = new List<double>();
        for (int i = 0; i < playerCount; i++)
        {
            valueList.Add(0);
        }
        return valueList;
    }
}