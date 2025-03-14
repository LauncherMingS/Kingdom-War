using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryManager : UnitManager
{
    private Infantry _infantry;
    public override Unit Unit
    {
        get { return _infantry; }
        set { _infantry = (value is Infantry) ? (Infantry)value : null; }
    }
    private void Update()
    {
        if (_infantry.atkCD > 0) _infantry.atkCD -= Time.deltaTime;
        switch (Unit.order)
        {
            case Unit.Order.Attack:
                DetectEnemy(transform.position, _infantry.detectRangeOnAtk);
                if (!_infantry.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_infantry.destination);
                }
                else {
                    if (_infantry.targetTransform.CompareTag("retreat")) _infantry.targetTransform = null;
                    GoFight(_infantry.detectRangeOnAtk); 
                }
                break;
            case Unit.Order.Defend:
                DetectEnemy(_infantry.defSpot, _infantry.detectRangeOnDef);
                if (!_infantry.targetTransform)
                {
                    an.SetBool("atk", false);
                    MoveTo(_infantry.defSpot);
                }
                else
                {
                    if (_infantry.targetTransform.CompareTag("retreat")) _infantry.targetTransform = null;
                    GoFight(_infantry.detectRangeOnDef);
                }
                break;
            case Unit.Order.Retreat:
                an.SetBool("atk", false);
                MoveTo(_infantry.retSpot);
                break;
        }
    }
    private void GoFight(float missingRange)
    {
        if (!_infantry.targetTransform) return;
        float _enemyDistance = Vector3.Distance(transform.position, _infantry.targetTransform.position);
        if (_enemyDistance > _infantry.atkRange)
        {
            an.SetBool("atk", false);
            MoveTo(_infantry.targetTransform.position);
        }
        else
        {
            Face(_infantry.targetTransform.position);
            an.SetBool("idle", false);
            an.SetBool("move", false);
            an.SetBool("atk", true);
            if (_infantry.atkCD <= 0)
            {
                if (_infantry.targetTransform.CompareTag("nexus"))
                {
                    _infantry.targetTransform.GetComponent<Nexus>().UnderAttack(_infantry.atkDamage);
                }
                else
                {
                    _infantry.targetTransform.GetComponent<UnitManager>().UnderAttack(_infantry.atkDamage);
                }
                _infantry.atkCD = _infantry.atkFrequence;
            }
        }
        if (_enemyDistance > missingRange)
        {
            _infantry.targetTransform = null;
        }
    }
    private void DetectEnemy(Vector3 pos, float detectRange)
    {
        _infantry.detectEnemies = Physics.OverlapSphere(pos, detectRange, LayerMask.GetMask(_infantry.enemy));
        if (_infantry.detectEnemies.Length == 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider enemyCollider in _infantry.detectEnemies)
        {
            if (enemyCollider.CompareTag("retreat")) continue;
            //優先攻擊非主堡的單位
            if (enemyCollider.CompareTag("nexus") && _infantry.targetTransform != null && !(_infantry.targetTransform.CompareTag("nexus")))
            {
                continue;
            }

            float _enemyDistance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                _infantry.targetTransform = enemyCollider.transform;
                if (_infantry.targetTransform.CompareTag("nexus")) 
                {
                    _infantry.atkRange = 4f;
                }
                else _infantry.atkRange = 2f;
            }
        }
    }
}