using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderManager : UnitManager
{
    private Commander _commander;
    public override Unit Unit
    {
        get { return _commander; }
        set { _commander = (value is Commander) ? (Commander)value : null; }
    }
    private void Update()
    {
        if (_commander.atkCD > 0) _commander.atkCD -= Time.deltaTime;
        DetectEnemy(transform.position, 10f);
        if (!_commander.targetTransform)
        {
            an.SetBool("atk", false);
            MoveTo(_commander.destination);
        }
        else
        {
            if (_commander.targetTransform.CompareTag("retreat")) _commander.targetTransform = null;
            GoFight(10f);
        }
    }
    private void GoFight(float missingRange)
    {
        if (!_commander.targetTransform) return;
        float _enemyDistance = Vector3.Distance(transform.position, _commander.targetTransform.position);
        if (_enemyDistance > _commander.atkRange)
        {
            an.SetBool("atk", false);
            MoveTo(_commander.targetTransform.position);
        }
        else
        {
            Face(_commander.targetTransform.position);
            an.SetBool("idle", false);
            an.SetBool("move", false);
            an.SetBool("atk", true);
            if (_commander.atkCD <= 0)
            {
                if (_commander.targetTransform.CompareTag("nexus"))
                {
                    _commander.targetTransform.GetComponent<Nexus>().UnderAttack(_commander.atkDamage);
                }
                else
                {
                    _commander.targetTransform.GetComponent<UnitManager>().UnderAttack(_commander.atkDamage);
                }
                _commander.atkCD = _commander.atkFrequence;
            }
        }
        if (_enemyDistance > missingRange)
        {
            _commander.targetTransform = null;
        }
    }
    private void DetectEnemy(Vector3 pos, float detectRange)
    {
        _commander.detectEnemies = Physics.OverlapSphere(pos, detectRange, LayerMask.GetMask(_commander.enemy));
        if (_commander.detectEnemies.Length == 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider enemyCollider in _commander.detectEnemies)
        {
            if (enemyCollider.CompareTag("retreat")) continue;
            //優先攻擊非主堡的單位
            if (enemyCollider.CompareTag("nexus") && _commander.targetTransform != null && !(_commander.targetTransform.CompareTag("nexus")))
            {
                continue;
            }

            float _enemyDistance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                _commander.targetTransform = enemyCollider.transform;
                if (_commander.targetTransform.CompareTag("nexus"))
                {
                    _commander.atkRange = 4f;
                }
                else _commander.atkRange = 2f;
            }
        }
    }
}
