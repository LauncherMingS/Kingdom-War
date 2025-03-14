using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingAI : AI
{
    public override double GetWeight()
    {
        return 0.3f;
    }
    public override void Execute()
    {
        Debug.Log("do nothing");
    }
    public override void Print()
    {
        
    }
}