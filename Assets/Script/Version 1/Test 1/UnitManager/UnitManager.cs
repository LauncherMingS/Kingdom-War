using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public virtual Unit Unit { get; set; }
    public SpriteRenderer spr;
    public Animator an;
    public void Initialize(Unit unit)
    {
        spr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        Unit = unit;

        switch (Unit.allyController.order)
        {
            case Controller.Order.Attack:
                Unit.SwitchOrder(Unit.Order.Attack);
                break;
            case Controller.Order.Defend:
                Unit.SwitchOrder(Unit.Order.Defend);
                break;
            case Controller.Order.Retreat:
                Unit.SwitchOrder(Unit.Order.Retreat);
                break;
        }
    }
    protected void Face(Vector3 p)
    {
        bool face = true;
        if (Unit.enemy == "SYWS") face = false;
        if (transform.position.x < p.x) spr.flipX = true;
        else if (transform.position.x == p.x) spr.flipX = face;
        else spr.flipX = false;
    }
    protected void MoveTo(Vector3 p)
    {
        if (transform.position != p)
        {
            an.SetBool("idle",false);
            an.SetBool("move", true);
            Face(p);
            transform.position = Vector3.MoveTowards(transform.position, p, Unit.moveSpeed * Time.deltaTime);
        }
        else
        {
            an.SetBool("idle", true);
            an.SetBool("move", false);
            Face(p);
        }
    }
    public void UnderAttack(float dmg)
    {
        StartCoroutine(hitFlash());
        Unit.currentHP -= dmg;
        if (Unit.currentHP <= 0)
        {
            Unit.allyController.totalCombatValue -= Unit.combatValue;
            Unit.allyController.DiedUnit(Unit.unitType);
            if (Unit.allyController.group.Equals("NLI"))
            {
                FindObjectOfType<GameState>().score += 5;
            }
            Destroy(gameObject);
        }
    }
    IEnumerator hitFlash()
    {
        spr.color = new Color32(255, 150, 150, 255);
        yield return new WaitForSeconds(0.15f);
        spr.color = new Color32(255, 255, 255, 255);
    }
}