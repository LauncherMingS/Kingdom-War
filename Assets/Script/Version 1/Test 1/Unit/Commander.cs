using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : Unit
{
    CommanderData commanderData;
    public float atkDamage = 3f;
    public float atkFrequence = 2.5f;
    public float atkCD = 0f;
    public float atkRange = 2f;

    public Collider[] detectEnemies;
    public Commander(CommanderData data, GameObject controller) : base(data, controller)
    {
        atkDamage = data.atkDamage;
        atkFrequence = data.atkFrequence;
        atkCD = 0;
        atkRange = data.atkRange;
    }
}