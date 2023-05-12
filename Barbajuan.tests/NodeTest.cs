public class NodeTest
{
    [Fact]
    public void UpdateNodeUpdatesAllParents()
    {
        // Given
        var rootNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 0);
        var firstGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 1);
        var firstGenChildNode2 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 1);
        var firstGenChildNode3 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 1);
        var secondGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 0);
        var secondGenChildNode2 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 0);
        var thirdGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 1 }, 1);
        rootNode.AddChild(firstGenChildNode1);
        rootNode.AddChild(firstGenChildNode2);
        rootNode.AddChild(firstGenChildNode3);
        firstGenChildNode1.AddChild(secondGenChildNode1);
        firstGenChildNode2.AddChild(secondGenChildNode2);
        secondGenChildNode1.AddChild(thirdGenChildNode1);

        // When
        thirdGenChildNode1.BackPropagate(10, 0);

        // Then
        Assert.Equal(11, rootNode.GetPlayerValue(0));
        Assert.Equal(2, rootNode.GetVisits());
    }

    [Fact]
    public void UpdateUpdatesRootNodeOneGeneration()
    {
        // Given
        var rootNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 2 }, 1);
        var firstGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 2 }, 1);
        rootNode.AddChild(firstGenChildNode1);
        // When
        firstGenChildNode1.BackPropagate(10, 0);
        // Then
        Assert.Equal(2, rootNode.GetVisits());
        Assert.Equal(11, rootNode.Getvalue()[0]);
    }

    [Fact]
    public void UpdateUpdatesRootNodeTwoGenerations()
    {
        // Given
        var rootNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 2 }, 1);
        var firstGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 2 }, 1);
        var secondGenChildNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 1, new List<double>() { 1, 2 }, 1);
        rootNode.AddChild(firstGenChildNode1);
        firstGenChildNode1.AddChild(secondGenChildNode);
        // When
        secondGenChildNode.BackPropagate(10, 0);
        // Then
        Assert.Single(rootNode.GetChildren());
        Assert.True(rootNode.IsRoot());
        Assert.Null(rootNode.GetParent());
        Assert.NotNull(firstGenChildNode1.GetParent());
        Assert.NotNull(secondGenChildNode.GetParent());
        Assert.Equal(2, rootNode.GetVisits());
        Assert.Equal(11, rootNode.Getvalue()[0]);
    }

    [Fact]
    public void IsTerminalNode()
    {
        // Given
        var rootNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);
        var firstGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);
        // When
        rootNode.AddChild(firstGenChildNode1);
        // Then
        Assert.True(rootNode.IsRoot());
        Assert.False(firstGenChildNode1.IsRoot());
    }
    [Fact]
    public void AddChildNode()
    {
        // Given
        var rootNode = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);
        var firstGenChildNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);
        // When
        rootNode.AddChild(firstGenChildNode1);
        // Then
        Assert.Equivalent(rootNode, firstGenChildNode1.GetParent());
    }


    [Fact(Skip = "Changed UCT, TEST NO LONGER VALID")]
    public void UCTMathTest()
    {
        // Given
        var rootNode1 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);
        var Node2 = new Node(null, new List<Node>(), new GameState(), new List<Card>(), 2, new List<double>() { 1, 2 }, 1);

        rootNode1.AddChild(Node2);

        // When
        var actual = Node2.GetUCT();
        double math = 1.8325546111576978;
        // Then
        Assert.Equal(math, actual);
    }
}