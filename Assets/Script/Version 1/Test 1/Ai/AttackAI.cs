using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAI : AI
{
    private void Start()
    {
        NLI_Controller = GetComponent<Controller>();
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }
    public override double GetWeight()
    {
        if (NLI_Controller.totalCombatValue < 26f) return 0;
        if (NLI_Controller.totalCombatValue >= gameState.SYWS_Controller.totalCombatValue*1.5f)
        {
            weight = 200;
        }
        else
        {
            weight = (NLI_Controller.totalCombatValue / gameState.SYWS_Controller.totalCombatValue) * 17.5;
        }
        if (NLI_Controller.order.Equals(Controller.Order.Defend))
        {
            weight *= 0.85f;
        }
        else weight *= 1.15f;
        return weight;
    }
    public override void Execute()
    {
        NLI_Controller.SetOrder("Attack");
    }
    public override void Print()
    {
        Debug.Log("AttackAI");
    }
}