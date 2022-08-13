using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public float curPos;
    public float nextPos;

    public float curAttackDelay;
    public float maxAttackDealy;
    public int attackStatus =1;
    public bool attack3;
   
    
    Rigidbody2D rigidbody;
    Animator anim;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Attack();
       attackCombo();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            attackStatus = 1;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            attack3 = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            attack3 = false;
        }
        curAttackDelay += Time.deltaTime;
    }


    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
        anim.SetInteger("Status", Mathf.Abs((int)h));
        
        if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
    void Attack()
    {
        if (curAttackDelay < maxAttackDealy) return;
        if (Input.GetKeyDown(KeyCode.Z)&&attackStatus == 1&&attack3 == false)
        {
            PlayAttack(attackStatus++);
            curAttackDelay = 0;
        }
    }
    void attackCombo()
    {
        if (Input.GetKeyDown(KeyCode.Z) && attackStatus == 2
           && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
           && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7)
        {
            PlayAttack(attackStatus++);
            curAttackDelay = 0;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
            && Input.GetKeyDown(KeyCode.Z) && attack3 == true)
        {
            PlayAttack(3);
            curAttackDelay = 0;
        }
    }
    void PlayAttack(int attackNum)
    {
        anim.SetTrigger("Atk" + attackNum);
    }
    
}
    
