using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Node
{
    protected List<Node> nodes = new List<Node>();
    public Parallel(List<Node> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        bool anyNodeIsRunning = false;
        int numNodeFail = 0;
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Running:
                    anyNodeIsRunning = true;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    numNodeFail++;
                    continue;
                default:
                    break;
            }
        }
        if (numNodeFail == nodes.Count) _nodeState = NodeState.Failure;
        else _nodeState = anyNodeIsRunning ? NodeState.Running : NodeState.Success;
        return _nodeState;
    }
}
