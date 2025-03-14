using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLI_ArcherScript : �Ĥ�h�L���
{
    //�ӤH�ݩ�
    public int id;
    public float hp = 10;
    public float atk = 3;
    public float atkFrequence = 2.5f;
    public float atkCD = 0;
    public float attackScope = 9.5f;//�����Z��
    public float moveSpeed = 4.5f;//���ʳt��
    public GameObject Arrow;
    //public float zoomMagnitude;
    public Vector3 right = Vector3.right;
    public SpriteRenderer spr;
    public Animator animator;
    //�ĤH�ݩ�
    public float attackDetectscope = 13;
    public float defendDetectScope = 11;
    public Transform NLI_enemyTransform;
    public Collider[] detectEnemies;
    //public Vector3 atkNexusPos;
    //�ͤ�Ѧ��ݩ�
    public Vector3 defendSpot;//���I
    public Vector3 retreatSpot = new Vector3(-32, 0, 2);
    void Start()
    {
        spr = transform.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (atkCD > 0) atkCD -= Time.deltaTime;
        if (NLI_enemyTransform && NLI_enemyTransform.gameObject.layer == LayerMask.NameToLayer("retreat")) NLI_enemyTransform = null;

        switch (status)
        {
            case Status.attack://�V�e�ôM��ĤH����
                if (!NLI_enemyTransform)
                {
                    Forward();
                    //Target(transform.position,attackDetectscope);
                    Detect(transform.position, attackDetectscope);
                }
                else
                {
                    Face(NLI_enemyTransform.position);
                    GoFight(NLI_enemyTransform, attackDetectscope);
                }
                break;
            case Status.defend://���V���I�B�C�����H��ĵ�١A��ĤH�a��A�i�����
                if (!NLI_enemyTransform)
                {
                    Face(defendSpot);
                    //Target(defendSpot,defendDetectScope);
                    Detect(defendSpot, defendDetectScope);
                    MoveTo(defendSpot);
                }
                else
                {
                    Face(NLI_enemyTransform.position);
                    GoFight(NLI_enemyTransform, defendDetectScope);
                }
                break;
            case Status.retreat://�V���M�h
                Face(retreatSpot);
                MoveTo(retreatSpot);
                break;
        }
    }
    public void UnderAttack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameObject.Find("�ڤ�L�ر��").GetComponentInParent<�ڤ�h�L���>().DiedUnit();
            Destroy(gameObject);
        }
    }
    void Forward()
    {
        animator.SetBool("move", true);
        animator.SetBool("idle", false);
        animator.SetBool("atk", false);
        spr.flipX = true;
        transform.Translate(moveSpeed * Time.deltaTime * right);
    }
    void Face(Vector3 targetPosition)
    {
        if (transform.position.x <= targetPosition.x) spr.flipX = true;
        else spr.flipX = false;
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
            animator.SetBool("atk", true);
            animator.SetBool("idle", false);
            animator.SetBool("move", false);
            if (atkCD <= 0)
            {
                Instantiate(Arrow,transform);
                //Arrow.GetComponent<Arrow>().target = target;
                if (target.tag == "infantry")
                {
                    target.GetComponent<�Ĥ�h�L>().UnderAttack(atk);
                }
                else if (target.tag == "miner")
                {
                    target.GetComponent<NLI_MinerScript>().UnderAttack(atk);
                }
                else if (target.tag == "nexus")
                {
                    target.GetComponent<Nexus>().UnderAttack(atk);
                }
                else if (target.tag == "tent")
                {
                    target.GetComponent<Tent>().UnderAttack(atk);
                }
                atkCD += atkFrequence;
            }
        }
        if (targetDistance > missingScope)
        {
            NLI_enemyTransform = null;
        }
    }
    void Detect(Vector3 soldierPosition, float detectScope)
    {
        detectEnemies = Physics.OverlapSphere(soldierPosition, detectScope, LayerMask.GetMask("NLI"));
        if (detectEnemies.Length < 0) return;

        float _minDistance = float.MaxValue;//�]�w�P�ĤH�̪񪺶Z��������w�ؼаѷ�
        foreach (Collider enemyCld in detectEnemies)
        {
            float _enemyDistance = Vector3.Distance(enemyCld.transform.position, soldierPosition);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                NLI_enemyTransform = enemyCld.transform;
                if (NLI_enemyTransform.gameObject.tag == "nexus")
                {
                    attackScope = 5.5f;
                }
                else
                {
                    attackScope = 15;
                }
            }
        }
    }
}
