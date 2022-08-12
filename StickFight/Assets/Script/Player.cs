using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public float curPos;
    public float nextPos;
    public int attackStatus;
    public float curAttack1Delay;
    public float curAttack2Delay;
    public float curAttack3Delay;
    public float maxAttackDelay;
    public float maxAttack2Delay;
    public float maxAttack3Delay;
    
    Rigidbody2D rigidbody;
    Animator anim;

    public bool isAttack;
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
        curAttack1Delay += Time.deltaTime;
        curAttack2Delay += Time.deltaTime;
        curAttack3Delay += Time.deltaTime;
        Move();
        Attack();
        ComboAttack();
        CcomboAttack();
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
    public void Attack()
    {
        if (curAttack1Delay < maxAttackDelay||!(attackStatus ==1)) return;
        if ((Input.GetKeyDown(KeyCode.Z)))
            PlayAttack(0);
        attackStatus = 1;
    }
    public void ComboAttack()
    {
        if (curAttack2Delay < maxAttack2Delay) return;
       if(anim.GetCurrentAnimatorStateInfo(0).IsName("Blend")
            &&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.7f
            &&Input.GetKeyDown(KeyCode.Z))
        {
            PlayAttack(1);
            curAttack2Delay = 0;
        }
    }
    public void CcomboAttack()
    {
        if (curAttack3Delay < maxAttack3Delay) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Blend")
             && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f
             && Input.GetKeyDown(KeyCode.Z))
        {
            PlayAttack(2);
            curAttack3Delay = 0;
        }
    }

    public void PlayAttack(float num)
    {
        anim.SetFloat("Blend", num);
        anim.SetTrigger("Atk");
        curAttack1Delay = 0;
    }

    
}
    
