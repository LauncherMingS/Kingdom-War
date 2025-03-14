using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public float Frequence;
    public float waited = 0;
    public List<AI> AIs = new List<AI>();
    public RecruitAI produceAI;
    private void Start()
    {
        
        foreach (AI ai in GetComponents<AI>())
        {
            AIs.Add(ai);
        }
        produceAI = GetComponent<RecruitAI>();
        this.enabled = false;
    }
    private void Update()
    {
        waited += Time.deltaTime;
        if (waited < Frequence) return;

        produceAI.Execute();
        double _bestAIValue = float.MinValue;
        AI bestAI = null;
        foreach (AI ai in AIs)
        {
            ai.timePassed += waited;
            double _aiValue = ai.GetWeight() + Random.Range(5f, 10f);
            if (_aiValue > _bestAIValue)
            {
                _bestAIValue = _aiValue;
                bestAI = ai;
            }
        }
        //bestAI.Print();
        //Debug.Log(_bestAIValue);
        bestAI.Execute();
        waited = 0;
    }
}