using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Node
{

    public override NodeState Evaluate()
    {

        return NodeState.Running;
    }
}
