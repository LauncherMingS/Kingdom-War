using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/UnitData", fileName = "UnitData")]
public class UnitData : ScriptableObject
{
    public GameObject prefab;

    public string unitType;
    public int priority;
    public int combatValue;
    public float recruitTime;
    public int cost;

    public float maxHP;
    public float moveSpeed;
}
