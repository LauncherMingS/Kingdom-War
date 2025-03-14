using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collector : Unit
{
    public int collectAmout = 5;//一次的採集量
    public float collectFrequence = 1.5f;//採集的頻率
    public float collectCD = 0f;//採集的冷卻時間
    public int maxCarry = 35;//資源攜帶的最大值
    public int currentCarry = 0;//當前的資源攜帶量
    public float resourceDetectRange = 5f;//偵測資源的範圍
    public bool conveyBack = false;

    public Transform allyNexusTransform;
    public float conveyDistance = 0.5f;//在運回資源時，單位與主堡接觸的距離

    public Collider[] detectResources;
    public Transform resourceTransform;
    public Resource resourceScript;
    public Vector3 resourcePos;

    public Collector(CollectorData data, GameObject controller) : base(data, controller)
    {
        collectAmout = data.collectAmout;
        collectFrequence = data.collectFrequence;
        collectCD = 0;
        maxCarry = data.maxCarry;
        currentCarry = 0;
        resourceDetectRange = data.resourceDetectRange;
        conveyBack = false;

        allyNexusTransform = GameObject.Find(allyController.group + "_Nexus").transform;
        conveyDistance = data.conveyDistance;
    }
}