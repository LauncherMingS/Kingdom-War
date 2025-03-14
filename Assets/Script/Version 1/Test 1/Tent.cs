using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : MonoBehaviour
{
    public float hp = 750;
    public float maxhp = 750;
    public float percentage;
    public float time = 0;
    public bool countStart = false;

    public SpriteRenderer spr;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (countStart)
        {
            time += Time.deltaTime;
            if (time > 5)
            {
                Destroy(gameObject);
                countStart = false;
                time = 0;
            }
        }
        percentage = hp / maxhp;
        if (hp <= 0) spr.sprite = Resources.Load<Sprite>("Tent/tent7");
        else if (percentage < 0.1f) spr.sprite = Resources.Load<Sprite>("Tent/tent6");
        else if (percentage < 0.2f) spr.sprite = Resources.Load<Sprite>("Tent/tent5");
        else if (percentage < 0.4f) spr.sprite = Resources.Load<Sprite>("Tent/tent4");
        else if (percentage < 0.6f) spr.sprite = Resources.Load<Sprite>("Tent/tent3");
        else if (percentage < 0.8f) spr.sprite = Resources.Load<Sprite>("Tent/tent2");

    }
    public void UnderAttack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            countStart = true;
            hp = 0;
        }
    }
}
