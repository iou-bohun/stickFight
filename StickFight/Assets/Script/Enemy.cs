using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void GetDmg(float dmg)
    {
        anim.SetTrigger("hit");
        hp = hp - dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
