using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float atkDamage;
    public float speed;
    public string enemy;
    private void Start()
    {
        Destroy(gameObject, 3);
    }
    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer(enemy))) {
            if (other.gameObject.CompareTag("nexus"))
            {
                other.gameObject.GetComponent<Nexus>().UnderAttack(atkDamage);
                Destroy(gameObject);
            }
            else
            {
                other.gameObject.GetComponent<UnitManager>().UnderAttack(atkDamage);
                Destroy(gameObject);
            }
        }
    }
    //public Transform target;
    //public float speed = 10;
    //public float minDistance = 0.5f;
    //public float distanceToTarget;
    //public bool moveFlag = true;
    //void Start()
    //{
    //    distanceToTarget = Vector3.Distance(transform.position, target.position);
    //    StartCoroutine(Parabola());
    //}
    //IEnumerator Parabola()
    //{
    //    while (moveFlag)
    //    {
    //        Vector3 targetPos = target.position;
    //        /* Vector2 direction = targetPos - transform.position;
    //         float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //         transform.rotation = Quaternion.AngleAxis(angle2 , Vector3.right);*/
    //        transform.LookAt(target,Vector3.forward);
    //        float angle = Mathf.Min(1, Vector3.Distance(transform.position, targetPos) / distanceToTarget) * 4;
    //        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, Mathf.Clamp(-angle, -42, 42));
    //        float currentDistance = Vector3.Distance(transform.position, target.position);
    //        if (currentDistance < minDistance)
    //        {
    //            moveFlag = false;
    //        }
    //        transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDistance));
    //        yield return null;
    //    }
    //    if (!moveFlag)
    //    {
    //        transform.position = target.position;
    //        StopCoroutine(Parabola());
    //        GameObject.Destroy(this);
    //    }
    //}
}
