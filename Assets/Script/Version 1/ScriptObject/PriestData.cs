using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/PriestData", fileName = "PriestData")]
public class PriestData : UnitData
{
    public float atkDamage;
    public float atkFrequence;
    public float atkRange;

    public float detectRangeOnAtk;
    public float detectRangeOnDef;
}