public class NodeTest
{
    [Fact]
    public void UpdateNodeUpdatesAllParents()
    {
        // Given
        var rootNode = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},0);
        var firstGenChildNode1 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},1);
        var firstGenChildNode2 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},1);
        var firstGenChildNode3 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},1);
        var secondGenChildNode1 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},0);
        var secondGenChildNode2 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},0);
        var thirdGenChildNode1 = new Node(null,new List<Node>(),null,new List<Card>(),1,new List<double>(){1,1},1);
        rootNode.addChild(firstGenChildNode1);
        rootNode.addChild(firstGenChildNode2);
        rootNode.addChild(firstGenChildNode3);
        firstGenChildNode1.addChild(secondGenChildNode1);
        firstGenChildNode2.addChild(secondGenChildNode2);
        secondGenChildNode1.addChild(thirdGenChildNode1);
        
        // When
        thirdGenChildNode1.update(10,0);
    
        // Then
        Assert.Equal(11,rootNode.getPlayerValue(0));
        Assert.Equal(2,rootNode.getVisits());
    }

    // [Fact]
    // public void UpdateUpdatesRootNodeOneGeneration()
    // {
    //     // Given
    //     var rootNode = new Node(null,new List<Node>(),null,new List<Card>(),1,1);
    //     var firstGenChildNode1 = new Node(null, new List<Node>(),null,new List<Card>(), 1, 1);
    //     rootNode.addChild(firstGenChildNode1);
    //     // When
    //     firstGenChildNode1.update(10);
    //     // Then
    //     Assert.Equal(2,rootNode.getVisits());
    //     Assert.Equal(11,rootNode.getvalue());
    // }

    // [Fact]
    // public void UpdateUpdatesRootNodeTwoGenerations()
    // {
    //     // Given
    //     var rootNode = new Node(null,new List<Node>(),null,new List<Card>(),1,1);
    //     var firstGenChildNode1 = new Node(null, new List<Node>(),null,new List<Card>(), 1, 1);
    //     var secondGenChildNode = new Node(null, new List<Node>(),null,new List<Card>(), 1, 1);
    //     rootNode.addChild(firstGenChildNode1);
    //     firstGenChildNode1.addChild(secondGenChildNode);
    //     // When
    //     secondGenChildNode.update(10);
    //     // Then
    //     Assert.Equal(1,rootNode.getChildren().Count);
    //     Assert.True(rootNode.isRoot());
    //     Assert.Null(rootNode.getParent());
    //     Assert.NotNull(firstGenChildNode1.getParent());
    //     Assert.NotNull(secondGenChildNode.getParent());
    //     //Assert.NotNull(secondGenChildNode.getParent().getParent().getParent());
    //     Assert.Equal(2,rootNode.getVisits());
    //     Assert.Equal(11,rootNode.getvalue());
    // }
    
    [Fact]
    public void IsTerminalNode()
    {
        // Given
        var rootNode = new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);
        var firstGenChildNode1 = new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);
        // When
        rootNode.addChild(firstGenChildNode1);
        // Then
        Assert.True(rootNode.isRoot());
        Assert.False(firstGenChildNode1.isRoot());
    }
    [Fact]
    public void addChildNode()
    {
        // Given
        var rootNode =new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);
        var firstGenChildNode1 =new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);
        // When
        rootNode.addChild(firstGenChildNode1);
        // Then
        Assert.Equivalent(rootNode,firstGenChildNode1.getParent());
    }


    [Fact]
    public void UCTMathTest()
    {
        // Given
        // Node? parent, List<Node> children, GameState? gameState, List<Card> action, double visits, double value
        var rootNode1 = new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);
        var Node2 = new Node(null,new List<Node>(),null,new List<Card>(),2,new List<double>(){1,2},1);

        rootNode1.addChild(Node2);
        
        // When
        var actual = Node2.getUCT();
        double math = 1.8325546111576978;
        // Then
        Assert.Equal(math, actual);
    }
}