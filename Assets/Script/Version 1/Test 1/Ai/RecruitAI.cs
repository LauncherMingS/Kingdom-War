using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitAI : AI
{
    public int p,d;
    public CollectorData collectorData;
    public int collectNum;
    public InfantryData infantryData;
    public ArcherData archerData;
    public PriestData priestData;
    private void Start()
    {
        NLI_Controller = GetComponent<Controller>();
        collectNum = 0;
    }
    public override double GetWeight()
    {
        return -3500;
    }
    public override void Execute()
    {
        foreach (UnitData u in NLI_Controller.unitSchedule)
        {
            if (u.unitType.Equals("collector")) collectNum++;
        }
        if (NLI_Controller.collectorList.Count + collectNum < 3) p = 100;
        else if (NLI_Controller.collectorList.Count + collectNum <= 5) p = 75;
        else if (NLI_Controller.collectorList.Count + collectNum < 8) p = 15;
        else  p = 5;

        collectNum = 0;

        d = Random.Range(0, 100);
        if (d >= p)
        {
            if (d >= 75)
            {
                if (priestData != null) NLI_Controller.AddInSchedule(priestData);
            }
            else if (d >= 60)
            {
                if (archerData != null) NLI_Controller.AddInSchedule(archerData);
            }
            else NLI_Controller.AddInSchedule(infantryData);
        }
        else NLI_Controller.AddInSchedule(collectorData);
    }
    public override void Print()
    {
        
    }
}