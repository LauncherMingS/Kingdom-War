using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/ArcherData", fileName = "ArcherData")]
public class ArcherData : UnitData
{
    public float atkDamage;
    public float atkFrequence;
    public float atkRange;

    public float detectRangeOnAtk;
    public float detectRangeOnDef;

    public GameObject arrow;
}