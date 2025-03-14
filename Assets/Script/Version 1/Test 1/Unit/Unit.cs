using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit
{
    public enum Order
    {
        Attack,
        Defend,
        Retreat
    }
    public Order order;

    [Header("Unit")]
    public Controller allyController;
    public string enemy;
    public GameObject gameObject;
    public string unitType;
    public int combatValue;
    public int priority;
    public string id;

    public float maxHP;
    public float currentHP;
    public float moveSpeed;

    public Vector3 defSpot;
    public Vector3 retSpot;

    public Transform targetTransform;
    public Vector3 destination;
    public Unit (UnitData data, GameObject controller)
    {
        allyController = controller.GetComponent<Controller>();
        enemy = allyController.enemyController.group;
        unitType = data.unitType;
        combatValue = data.combatValue;
        priority = data.priority;

        maxHP = data.maxHP;
        currentHP = data.maxHP;
        moveSpeed = data.moveSpeed;

        retSpot = controller.transform.position;
        
        gameObject = GameObject.Instantiate(data.prefab, controller.transform);
        gameObject.transform.position = controller.transform.position;
        gameObject.GetComponent<UnitManager>().Initialize(this);
        gameObject.layer = LayerMask.NameToLayer(allyController.group);

        destination = new Vector3(GameObject.Find(enemy + "_Nexus").transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 2f);
    }
    public void SwitchOrder(Order order)
    {
        targetTransform = null;
        switch (order)
        {
            case Order.Attack:
                this.order = Order.Attack;
                gameObject.tag = unitType;
                break;
            case Order.Defend:
                this.order = Order.Defend;
                gameObject.tag = unitType;
                break;
            case Order.Retreat:
                this.order = Order.Retreat;
                gameObject.tag = "retreat";
                break;
        }
    }
}