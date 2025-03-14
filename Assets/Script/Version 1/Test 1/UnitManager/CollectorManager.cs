using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorManager : UnitManager
{
    private Collector _collector;
    public override Unit Unit 
    {
        get => _collector;
        set => _collector = (value is Collector) ? (Collector)value : null;
    }
    private void Update()
    {
        if (_collector.collectCD > 0) _collector.collectCD -= Time.deltaTime;
        switch (Unit.order)
        {
            case Unit.Order.Attack:
            case Unit.Order.Defend:
                if (_collector.conveyBack)
                {
                    ConveyBack();
                }
                else if (!_collector.resourceTransform)
                {
                    an.SetBool("collect", false);
                    MoveTo(_collector.destination);
                    DetectMineral();
                }
                else
                {
                    Collect();
                }
                break;
            case Unit.Order.Retreat:
                an.SetBool("collect", false);
                MoveTo(_collector.retSpot);
                break;
        }
    }
    private void Collect()
    {
        float _mineralDistance = Vector3.Distance(transform.position, _collector.resourceTransform.position);
        if (_collector.currentCarry >= _collector.maxCarry)//運送資源回主堡
        {
            an.SetBool("collect", false);
            _collector.conveyBack = true;
            _collector.resourceScript.RemoveOccupiedList(_collector);
            return;
        }
        else if (transform.position != _collector.resourcePos)//移動到資源點
        {
            an.SetBool("collect", false);
            MoveTo(_collector.resourcePos);
        }
        else//採集資源
        {
            an.SetBool("move",false);
            an.SetBool("collect", true);
            an.SetInteger("carry", _collector.currentCarry);
            Face(_collector.resourceTransform.position);
            if (_collector.collectCD <= 0)
            {
                _collector.resourceScript.UnderCollect(_collector.collectAmout);
                _collector.currentCarry += _collector.collectAmout;
                _collector.collectCD += _collector.collectFrequence;
            }
        }
    }
    private void ConveyBack()
    {
        float _nexusDistance = Vector3.Distance(transform.position, _collector.allyNexusTransform.position) - 5f;
        if (_nexusDistance > _collector.conveyDistance)
        {
            an.SetBool("collect", false);
            MoveTo(_collector.allyNexusTransform.position);
        }
        else
        {
            _collector.allyController.money += _collector.currentCarry;
            _collector.currentCarry = 0;
            _collector.conveyBack = false;
        }
    }

    public void DetectMineral()
    {
        _collector.detectResources = Physics.OverlapSphere(transform.position, _collector.resourceDetectRange, LayerMask.GetMask("resource"));
        if (_collector.detectResources.Length < 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider _mineral in _collector.detectResources)
        {
            if (_mineral.gameObject.layer.Equals("occupied")) continue;

            float _mineralDistance = Vector3.Distance(_mineral.transform.position, transform.position);
            if (_mineralDistance < _minDistance)
            {
                if (_collector.resourceTransform != null && !_collector.resourceTransform.Equals(_mineral.transform))
                {
                    _collector.resourceScript.RemoveOccupiedList(_collector);
                }
                _minDistance = _mineralDistance;
                _collector.resourceTransform = _mineral.transform;
                _collector.resourceScript = _mineral.GetComponent<Resource>();
                _collector.resourceScript.AddOccupiedList(_collector);
            }
        }
    }
}