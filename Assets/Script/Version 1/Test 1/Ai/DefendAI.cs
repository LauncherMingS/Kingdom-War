using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendAI : AI
{
    private void Start()
    {
        NLI_Controller = GetComponent<Controller>();
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }
    public override double GetWeight()
    {
        if (NLI_Controller.totalCombatValue <= 35) return 6;
        weight = (NLI_Controller.totalCombatValue / gameState.SYWS_Controller.totalCombatValue) * 17.5;
        return weight;
    }
    public override void Execute()
    {
        NLI_Controller.SetOrder("Defend");
    }
    public override void Print()
    {
        Debug.Log("DefendAI");
    }
}