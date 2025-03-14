using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestManager : UnitManager
{
    private Priest _priest;
    public override Unit Unit
    {
        get { return _priest; }
        set { _priest = (value is Priest) ? (Priest)value : null; }
    }
    private void Update()
    {
        if (_priest.atkCD > 0) _priest.atkCD -= Time.deltaTime;
        switch (Unit.order)
        {
            case Unit.Order.Attack:
                DetectEnemy(transform.position, _priest.detectRangeOnAtk);
                if (!_priest.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_priest.destination);
                }
                else
                {
                    if (_priest.targetTransform.CompareTag("retreat")) _priest.targetTransform = null;
                    Recover(_priest.detectRangeOnAtk);
                }
                break;
            case Unit.Order.Defend:
                DetectEnemy(_priest.defSpot, _priest.detectRangeOnDef);
                if (!_priest.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_priest.defSpot);
                }
                else
                {
                    if (_priest.targetTransform.CompareTag("retreat")) _priest.targetTransform = null;
                    Recover(_priest.detectRangeOnDef);
                }
                break;
            case Unit.Order.Retreat:
                an.SetBool("atk", false);
                MoveTo(_priest.retSpot);
                break;
        }
    }
    private void Recover(float missingRange)
    {
        if (!_priest.targetTransform) return;
        else if (_priest.targetTransform.GetComponent<UnitManager>().Unit.currentHP >= _priest.targetTransform.GetComponent<UnitManager>().Unit.maxHP)
        {
            _priest.targetTransform = null;
            return;
        }
        float _enemyDistance = Vector3.Distance(transform.position, _priest.targetTransform.position);
        if (_enemyDistance > _priest.atkRange)
        {
            an.SetBool("atk", false);
            MoveTo(_priest.targetTransform.position);
        }
        else
        {
            Face(_priest.targetTransform.position);
            if (_priest.atkCD <= 0)
            {
                an.SetBool("idle", false);
                an.SetBool("move", false);
                an.SetBool("atk", true);
                _priest.targetTransform.GetComponent<UnitManager>().UnderAttack(_priest.atkDamage);
                _priest.atkCD = _priest.atkFrequence;
            }
        }
        if (_enemyDistance > missingRange)
        {
            _priest.targetTransform = null;
        }
    }
    private void DetectEnemy(Vector3 pos, float detectRange)
    {
        _priest.detectEnemies = Physics.OverlapSphere(pos, detectRange, LayerMask.GetMask(_priest.allyController.group));
        if (_priest.detectEnemies.Length == 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider enemyCollider in _priest.detectEnemies)
        {
            if (enemyCollider.CompareTag("retreat") || enemyCollider.CompareTag("nexus")) continue;
            //優先攻擊非主堡的單位
            else if (enemyCollider.gameObject.GetComponent<UnitManager>().Unit.currentHP >= enemyCollider.gameObject.GetComponent<UnitManager>().Unit.maxHP)
            {
                continue;
            }
            float _enemyDistance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                _priest.targetTransform = enemyCollider.transform;
            }
        }
    }
}