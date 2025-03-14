using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class �ڤ�h�L : �ڤ�h�L���
{
    //�ӤH�ݩ�
    public string id;
    public float hp = 10;
    public float atk = 3;
    public float atkFrequence = 2.5f;
    public float atkCD = 0;
    public float attackScope = 2.5f;//�����Z��
    public float moveSpeed = 4.5f;//���ʳt��
    //public float zoomMagnitude;
    public Vector3 right = Vector3.right;
    public SpriteRenderer spr;
    public Animator animator;
    //�ĤH�ݩ�
    public float attackDetectscope = 5;
    public float defendDetectScope = 3;
    public Transform NLI_enemyTransform;
    public Collider[] detectEnemies;
    //public Vector3 atkNexusPos;
    //�ͤ�Ѧ��ݩ�
    public Vector3 defendSpot;//���I
    public Vector3 retreatSpot = new Vector3(-32,0,2);

    //AudioSource audioSource;
    //public AudioClip swordFightSound;
    void Start()
    {
        spr = transform.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        //zoomMagnitude = 0.01f;
        /*detectSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        detectSphere.transform.parent = transform;
        detectSphere.transform.position = transform.position;*/
        //prePositon = transform.position;
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
        //print(Vector3.Distance(transform.position, prePositon));
        //if (transform.position.z > prePositon.z) zoomIn();
        //else if (transform.position.z < prePositon.z) zoomOut();
        /*if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.z >= 4) return;
            transform.position += new Vector3(0,0,1*unit);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.z <= 0) return;
            transform.position -= new Vector3(0, 0, 1*unit);
        }*/
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
        animator.SetBool("move",true);
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
    void GoFight(Transform target,float missingScope)
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance > attackScope)
        {
            MoveTo(target.position);
        }
        else
        {
            //audioSource.clip = swordFightSound;
            //audioSource.Play();

            animator.SetBool("atk", true);
            animator.SetBool("idle", false);
            animator.SetBool("move", false);
            if (atkCD <= 0)
            {
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
            //target = null;//�ǤJ���Ѽƥu��Ū�����\��A�S���мg���\��
        }
    }
    void Detect(Vector3 soldierPosition, float detectScope)
    {
        detectEnemies = Physics.OverlapSphere(soldierPosition, detectScope, LayerMask.GetMask("NLI"));
        if (detectEnemies.Length < 0) return;

        float _minDistance = float.MaxValue;//�]�w�P�ĤH�̪񪺶Z��������w�ؼаѷ�
        foreach(Collider enemyCld in detectEnemies)
        {
            float _enemyDistance = Vector3.Distance(enemyCld.transform.position, soldierPosition);
            if (_enemyDistance < _minDistance)
            {
                _minDistance = _enemyDistance;
                NLI_enemyTransform = enemyCld.transform;
                if (NLI_enemyTransform.gameObject.tag == "nexus" || NLI_enemyTransform.gameObject.tag == "tent")
                {
                    attackScope = 5.5f;
                    //NLI_enemyTransform.GetComponent<Nexus>().Addin(gameObject);
                    //atkNexusPos = NLI_enemyTransform.GetComponent<Nexus>().AllocatePos();
                }
                else
                {
                    attackScope = 2;
                }
            }
        }
    }
    /*void Target(Vector3 soldierPosition,float targetScope)
    {
        //GameObject[] _findEnemy = GameObject.FindGameObjectsWithTag("enemy");//��M���W�Ҧ��ĤH����
        if (NLI_soldiersList.Count > 0)//�O�_������ĤH
        {
            float _minDistance = float.MaxValue;//�]�w�P�ĤH�̪񪺶Z��������w�ؼаѷ�
            foreach (GameObject enemyObj in NLI_soldiersList)
            {
                float _distance = Vector3.Distance(enemyObj.transform.position, soldierPosition);
                //���o�P�C�ӼĤH���󪺶Z��
                if (_distance <= targetScope)//�p�G�b�����d��(detectScope)��
                {
                    if (_distance < _minDistance)//conclusion:���o�̪񪺼ĤH��m
                    {
                        _minDistance = _distance;
                        enemyTransform = enemyObj.transform;
                    }
                }
            }
        }
    }*/
    /*void ZoomIn()
    {
        prePositon = transform.position;
        transform.localScale -= new Vector3(zoomMagnitude * unit, zoomMagnitude * unit, zoomMagnitude * unit);
    }
    void ZoomOut()
    {
        prePositon = transform.position;
        transform.localScale += new Vector3(zoomMagnitude * unit, zoomMagnitude * unit, zoomMagnitude * unit);
    }*/
    /*public void OrderAttack()
    {
        status = Status.attack;
    }
    public void OrderDefend()
    {
        status = Status.defend;
    }
    public void OrderRetreat()
    {
        status = Status.retreat;
    }*/
}