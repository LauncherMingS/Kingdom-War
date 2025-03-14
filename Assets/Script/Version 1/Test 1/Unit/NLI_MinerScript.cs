using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NLI_MinerScript : 敵方士兵控制器
{
    //個人屬性
    public string id;
    public float hp = 15;
    public float moveSpeed = 4.5f;
    public SpriteRenderer spr;
    public Animator animator;
    //礦物相關屬性
    public Transform mineralTransform;
    public Vector3 mineralPosition;
    public Collider[] minerals;
    public Resource mineralScript;
    public bool conveyBack;
    public float miningAmount = 5;
    public float miningDistance = 3;
    public float miningFrequence = 1.5f;
    public float miningCD = 0;
    public float mineralCarry = 0;
    public float maxMineralCarry = 35;
    public float mineralDetectScope = 5;
    //友方屬性
    public 敵方士兵控制器 allyController;
    public Transform nexus;
    public float conveyDistance = 0.5f;
    public Transform retreatSpot;
    private void Start()
    {
        mineralCarry = 0;
        miningDistance = 2;
        conveyBack = false;
        nexus = GameObject.Find("NLI_Nexus").transform;
        allyController = GameObject.Find("敵方兵種控制器").GetComponent<敵方士兵控制器>();
        retreatSpot = GameObject.Find("NLI_retreatSpot").transform;
        spr = transform.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (miningCD > 0) miningCD -= Time.deltaTime;
        switch (status)
        {
            case Status.attack:
                if (!mineralTransform)
                {
                    if (conveyBack)
                    {
                        ConveyBack(nexus);
                    }
                    else
                    {
                        Forward();
                        DetectMineral(mineralDetectScope);
                    }
                }
                else
                {
                    Face(mineralTransform);
                    Mining(mineralTransform);
                }
                break;
            case Status.defend:
                if (!mineralTransform)
                {
                    if (conveyBack)
                    {
                        ConveyBack(nexus);
                    }
                    else
                    {
                        Forward();
                        DetectMineral(mineralDetectScope);
                    }
                }
                else
                {
                    Face(mineralTransform);
                    Mining(mineralTransform);
                }
                break;
            case Status.retreat:
                animator.SetBool("mining", false);
                Face(retreatSpot);
                MoveTo(retreatSpot.position);
                break;
        }
    }
    public void UnderAttack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            allyController.DyingSoldier();
            Destroy(gameObject);
        }
    }
    void Forward()
    {
        spr.flipX = false;
        transform.position -= new Vector3(moveSpeed * Time.deltaTime,0,0);
    }
    void MoveTo(Vector3 destination)
    {
        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }
    }
    void Face(Transform target)
    {
        if (transform.position.x >= target.position.x) spr.flipX = false;
        else spr.flipX = true;
    }
    void Mining(Transform target)
    {
        float mineralDistance = Vector3.Distance(transform.position, target.position);

        if (mineralCarry >= maxMineralCarry)
        {
            animator.SetBool("mining", false);

            conveyBack = true;
            //LeaveMineral();
            return;
        }
        if (transform.position != mineralPosition)
        {
            animator.SetBool("mining", false);

            MoveTo(mineralPosition);
        }
        else
        {
            animator.SetBool("mining", true);
            animator.SetInteger("carry", (int)(mineralCarry));

            if (miningCD <= 0)
            {
                target.GetComponent<Resource>().UnderCollect((int)miningAmount);
                mineralCarry += miningAmount;
                miningCD += miningFrequence;
            }
        }
    }
    public void ConveyBack(Transform target)
    {
        Face(target);
        float _nexusDistance = Vector3.Distance(transform.position,target.position);
        if (_nexusDistance - 5 > conveyDistance)
        {
            MoveTo(target.position);
        }
        else
        {
            allyController.money += mineralCarry;
            mineralCarry = 0;
            conveyBack = false;
        }
    }
    //public void LeaveMineral()
    //{
    //    mineralScript.RemoveOccupiedList(this.gameObject);
    //}
    void DetectMineral(float detectScope)
    {
        minerals = Physics.OverlapSphere(transform.position, detectScope, LayerMask.GetMask("mineral"));
        if (minerals.Length < 0) return;

        float _minDistance = float.MaxValue;
        foreach (Collider _mineral in minerals)
        {
            if (_mineral.tag == "occupied") continue;
            float _mineralDistance = Vector3.Distance(_mineral.transform.position, transform.position);
            if (_mineralDistance < _minDistance)
            {
                _minDistance = _mineralDistance;
                mineralTransform = _mineral.transform;
                mineralScript = _mineral.GetComponent<Resource>();
                //mineralScript.AddOccupiedList(this.gameObject);
            }
        }
    }
}