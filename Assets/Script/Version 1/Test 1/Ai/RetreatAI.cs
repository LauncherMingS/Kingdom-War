using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatAI : AI
{
    private void Start()
    {
        NLI_Controller = GetComponent<Controller>();
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }
    public override double GetWeight()
    {
        if (gameState.SYWS_Controller.totalCombatValue <= 227.5) return 0;
        weight = gameState.SYWS_Controller.totalCombatValue / NLI_Controller.totalCombatValue;
        if (weight >= 3f)
        {
            return weight * 2f;
        }
        else return 0;
    }
    public override void Execute()
    {
        NLI_Controller.SetOrder("Retreat");
    }
    public override void Print()
    {
        Debug.Log("RereatAI");
    }
}