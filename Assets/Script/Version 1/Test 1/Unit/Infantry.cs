using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit
{
    public InfantryData data;
    public float atkDamage = 3f;
    public float atkFrequence = 2.5f;
    public float atkCD = 0f;
    public float atkRange = 2f;
    public float detectRangeOnAtk = 5f;
    public float detectRangeOnDef = 3f;

    public Collider[] detectEnemies;
    public Infantry(InfantryData data, GameObject controller) : base(data, controller)
    {
        atkDamage = data.atkDamage;
        atkFrequence = data.atkFrequence;
        atkCD = 0;
        atkRange = data.atkRange;

        detectRangeOnAtk = data.detectRangeOnAtk;
        detectRangeOnDef = data.detectRangeOnDef;
    }
}
