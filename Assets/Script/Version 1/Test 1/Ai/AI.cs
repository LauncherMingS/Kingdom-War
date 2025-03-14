using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour
{
    public float timePassed = 0;
    public double weight;
    public Controller NLI_Controller;
    public GameState gameState;
    public abstract double GetWeight();
    public abstract void Execute();
    public abstract void Print();
}