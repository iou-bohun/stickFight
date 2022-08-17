using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float distance;
    public float dmg;
    public float curAttackDelay;
    public float maxAttackDelay;
    bool attack = true;
    public GameObject player;
    Animator anim;

    public Transform pos;
    public Vector2 boxSize;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        AwakeThis(distance);
        Attack();
        curAttackDelay += Time.deltaTime;
        AttackReturn();
    }

    void AwakeThis(float dis)
    {
        if (dis < 3f)
        {
            anim.SetBool("Awake", true);
        }
        else anim.SetBool("Awake", false);
    }

    void Attack()
    {
        if (attack == false) return;
        if(distance < 2.2f && curAttackDelay>maxAttackDelay)
        {
            curAttackDelay = 0;
            attack = false;
            anim.SetTrigger("Attack");Debug.Log("Trigger");
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player" )
                {
                    Debug.Log("Check");
                    Player2 player = collider.GetComponent<Player2>();
                    player.GetDmg(dmg);
                    
                }
            }
            
        }
    }
    void AttackReturn()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TEnemyAttack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.99f)
        {
            attack = true;
            Debug.Log(attack);
        }
    }

    public void GetDmg(float dmg)
    {
        hp = hp - dmg;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
