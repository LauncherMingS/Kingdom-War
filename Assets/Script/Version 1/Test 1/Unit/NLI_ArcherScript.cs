using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLI_ArcherScript : 敵方士兵控制器
{
    //個人屬性
    public int id;
    public float hp = 10;
    public float atk = 3;
    public float atkFrequence = 2.5f;
    public float atkCD = 0;
    public float attackScope = 9.5f;//攻擊距離
    public float moveSpeed = 4.5f;//移動速度
    public GameObject Arrow;
    //public float zoomMagnitude;
    public Vector3 right = Vector3.right;
    public SpriteRenderer spr;
    public Animator animator;
    //敵人屬性
    public float attackDetectscope = 13;
    public float defendDetectScope = 11;
    public Transform NLI_enemyTransform;
    public Collider[] detectEnemies;
    //public Vector3 atkNexusPos;
    //友方參考屬性
    public Vector3 defendSpot;//站點
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
            case Status.attack://向前並尋找敵人攻擊
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
            case Status.defend://走向站點、列隊並隨時警戒，當敵人靠近，進行攻擊
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
            case Status.retreat://向後方撤退
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
            GameObject.Find("我方兵種控制器").GetComponentInParent<我方士兵控制器>().DiedUnit();
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
                    target.GetComponent<敵方士兵>().UnderAttack(atk);
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

        float _minDistance = float.MaxValue;//設定與敵人最近的距離做為鎖定目標參照
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
