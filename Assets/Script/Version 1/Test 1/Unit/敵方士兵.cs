using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class �Ĥ�h�L : �Ĥ�h�L���
{
    //�ӤH�ݩ�
    public int id;
    public float hp = 10;
    public float atk = 3;
    public float atkFrequence = 2.5f;
    public float atkCD = 0;
    public float attackScope = 2.5f;
    public float moveSpeed = 4.5f;
    //public float zoomMagnitude;
    public Vector3 left = Vector3.left;
    public SpriteRenderer spr;
    public Animator animator;

    public float attackDetectscope = 5;
    public float defendDetectScope = 3;
    public Transform SYWS_enemyTransform;
    public Collider[] detectEnemies;
    //�ͤ�Ѧ��ݩ�
    public Vector3 defendSpot;//���I
    public Vector3 retreatSpot = new Vector3(32,0,2);
    void Start()
    {
        spr = transform.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        status = Status.defend;
        //TestTarget();
    }
    void Update()
    {
        if (atkCD > 0) atkCD -= Time.deltaTime;
        switch (status)
        {
            case Status.attack://�V�e�ôM��ĤH����
                if (!SYWS_enemyTransform)
                {
                    Forward();
                    //Target(transform.position,attackDetectscope);
                    Detect(transform.position, attackDetectscope);
                }
                else
                {
                    Face(SYWS_enemyTransform.position);
                    GoFight(SYWS_enemyTransform, attackDetectscope);
                }
                break;
            case Status.defend://���V���I�B�C�����H��ĵ�١A��ĤH�a��A�i�����
                if (!SYWS_enemyTransform)
                {
                    Face(defendSpot);
                    //Target(defendSpot,defendDetectScope);
                    Detect(defendSpot, defendDetectScope);
                    MoveTo(defendSpot);
                }
                else
                {
                    Face(SYWS_enemyTransform.position);
                    GoFight(SYWS_enemyTransform, defendDetectScope);
                }
                break;
            case Status.retreat://�V���M�h
                Face(retreatSpot);
                MoveTo(retreatSpot);
                break;
        }
        //Move();
        //TestForward();
    }
    public void UnderAttack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameObject.Find("�Ĥ�L�ر��").GetComponentInParent<�Ĥ�h�L���>().DyingSoldier();
            Destroy(gameObject);
        }
    }
    void Forward()
    {
        animator.SetBool("move", true);
        animator.SetBool("idle", false);
        animator.SetBool("atk", false);
        transform.Translate(moveSpeed * Time.deltaTime * left);
    }
    void Face(Vector3 targetPosition)
    {
        if (transform.position.x >= targetPosition.x) spr.flipX = false;
        else spr.flipX = true;
    }
    void MoveTo(Vector3 destination)
    {
        if (transform.position != destination)
        {
            animator.SetBool("move", true);
            animator.SetBool("idle", false);
            animator.SetBool("atk", false);
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("move", false);
            animator.SetBool("atk", false);
        }
    }
    void GoFight(Transform target, float missingScope)
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance > attackScope)
        {
            MoveTo(target.position);
        }
        else
        {
            if (atkCD <= 0)
            {
                if (target.tag == "infantry")
                {
                    target.GetComponent<�ڤ�h�L>().UnderAttack(atk);
                }
                else if (target.tag == "miner")
                {
                    target.GetComponent<SYWS_MinerScript>().UnderAttack(atk);
                }
                else if (target.tag == "nexus")
                {
                    target.GetComponent<Nexus>().UnderAttack(atk);
                }
                else if (target.tag == "tent")
                {
                    target.GetComponent<Tent>().UnderAttack(atk);
                }
                animator.SetBool("atk", true);
                animator.SetBool("idle", false);
                animator.SetBool("move", false);
                atkCD += atkFrequence;
            }
        }
        if (targetDistance > missingScope)
        {
            SYWS_enemyTransform = null;
            //target = null;//�ǤJ���Ѽƥu��Ū�����\��A�S���мg���\��
        }
    }
    void Detect(Vector3 soldierPosition, float targetScope)
    {
        detectEnemies = Physics.OverlapSphere(soldierPosition, targetScope, LayerMask.GetMask("SYWS"));
        if (detectEnemies.Length < 0) return;

        float _minDistance = float.MaxValue;//�]�w�P�ĤH�̪񪺶Z��������w�ؼаѷ�
        foreach (Collider enemyCld in detectEnemies)
        {
            float _enemyDistance = Vector3.Distance(enemyCld.transform.position, soldierPosition);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                SYWS_enemyTransform = enemyCld.transform;
                if (SYWS_enemyTransform.gameObject.tag == "nexus")
                {
                    attackScope = 5.5f;
                }
                else
                {
                    attackScope = 2;
                }
            }
        }
    }
    /*void Move()
    {
        //transform.Translate(speed * unit * Vector3.left);
    }
    void TestForward()//can delete
    {
        if (targetObject)
        {
            targetDistance = Vector3.Distance(transform.position, targetObject.transform.position);
            if (targetDistance < attackScope)
            {
                //TestAtk();
            }
        }
    }
    void TestAtk()
    {
        if (atkCD <= Time.time)
        {
            targetObject.GetComponent<�ڤ�h�L>().UnderAttack(4);
            atkCD = Time.time + atkFrequence;
        }
    }
    void TestTarget()//can delete
    {
        GameObject target = GameObject.FindWithTag("ally");
        if (target != null)
        {
            targetObject = target;
        }
    }*/
}