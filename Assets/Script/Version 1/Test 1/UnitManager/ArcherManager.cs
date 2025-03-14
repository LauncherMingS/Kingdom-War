using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManager : UnitManager
{
    Archer _archer;
    public override Unit Unit
    {
        get { return _archer; }
        set { _archer = (value is Archer) ? (Archer)value : null; }
    }
    private void Update()
    {
        if (_archer.atkCD > 0) _archer.atkCD -= Time.deltaTime;
        switch (Unit.order)
        {
            case Unit.Order.Attack:
                DetectEnemy(transform.position, _archer.detectRangeOnAtk);
                if (!_archer.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_archer.destination);
                }
                else
                {
                    if (_archer.targetTransform.CompareTag("retreat")) _archer.targetTransform = null;
                    GoFight(_archer.detectRangeOnAtk);
                }
                break;
            case Unit.Order.Defend:
                DetectEnemy(_archer.defSpot, _archer.detectRangeOnDef);
                if (!_archer.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_archer.defSpot);
                }
                else
                {
                    if (_archer.targetTransform.CompareTag("retreat")) _archer.targetTransform = null;
                    GoFight(_archer.detectRangeOnDef);
                }
                break;
            case Unit.Order.Retreat:
                an.SetBool("atk", false);
                MoveTo(_archer.retSpot);
                break;
        }
    }
    private void GoFight(float missingRange)
    {
        if (!_archer.targetTransform) return;
        float _enemyDistance = Vector3.Distance(transform.position, _archer.targetTransform.position);
        if (_enemyDistance > _archer.atkRange)
        {
            an.SetBool("atk", false);
            MoveTo(_archer.targetTransform.position);
        }
        else
        {
            Face(_archer.targetTransform.position);
            if (_archer.atkCD <= 0)
            {
                an.SetBool("idle", false);
                an.SetBool("move", false);
                an.SetBool("atk", true);
                //if (_archer.targetTransform.CompareTag("nexus"))
                //{
                //    _archer.targetTransform.GetComponent<Nexus>().UnderAttack(_archer.atkDamage);
                //}
                //else
                //{
                //    _archer.targetTransform.GetComponent<UnitManager>().UnderAttack(_archer.atkDamage);
                //}
                _archer.atkCD = _archer.atkFrequence;
            }
        }
        if (_enemyDistance > missingRange)
        {
            _archer.targetTransform = null;
        }
    }
    public void Shoot()
    {
        GameObject _arrow = Instantiate(_archer.arrow);
        _arrow.transform.position = transform.position;
        _arrow.GetComponent<Arrow>().atkDamage = _archer.atkDamage;
        _arrow.GetComponent<Arrow>().enemy = _archer.enemy;
        switch (_archer.allyController.group)
        {
            case "SYWS":
                _arrow.GetComponent<Arrow>().speed = 10f;
                break;
            case "NLI":
                _arrow.GetComponent<Arrow>().speed = -10f;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
    private void DetectEnemy(Vector3 pos, float detectRange)
    {
        _archer.detectEnemies = Physics.OverlapSphere(pos, detectRange, LayerMask.GetMask(_archer.enemy));
        if (_archer.detectEnemies.Length == 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider enemyCollider in _archer.detectEnemies)
        {
            if (enemyCollider.CompareTag("retreat")) continue;
            //優先攻擊非主堡的單位
            if (enemyCollider.CompareTag("nexus") && _archer.targetTransform != null && !(_archer.targetTransform.CompareTag("nexus")))
            {
                continue;
            }
            float _enemyDistance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                _archer.targetTransform = enemyCollider.transform;
            }
        }
    }
}