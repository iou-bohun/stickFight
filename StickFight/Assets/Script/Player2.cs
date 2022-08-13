using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int speed;
    public float curPos;
    public float nextPos;
    public float jumpPower;
    int jumpCount;
    bool ground;

    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    public float curAttackDelay;
    public float maxAttackDealy;
    public int attackStatus = 1;
    public bool attack3;

    public float dmg;


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
        Jump();
        ComboCheck();
        IsFalling();
    
        m_timeSinceAttack += Time.deltaTime;

    }
    
    public void Jump()
    {
        if (jumpCount >= 2) return;
        if (Input.GetKeyDown("space"))
        {
            anim.SetTrigger("Jump");
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    public void IsFalling()
    {
        anim.SetFloat("InAir", rigidbody.velocity.y);
        anim.SetBool("IsGround", ground);
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
        if (Input.GetKeyDown(KeyCode.Z) && m_timeSinceAttack > 0.3f)
        {
            m_currentAttack++;
            if (m_currentAttack > 3)
                m_currentAttack = 1;
            if (m_timeSinceAttack > 0.8f)
                m_currentAttack = 1;
            anim.SetTrigger("Atk" + m_currentAttack);
            m_timeSinceAttack = 0.0f;
        }
        
    }
    void ComboCheck()
    {
        if(Input.GetKeyDown(KeyCode.Z)
            &&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
            &&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.85f)
        {
            Debug.Log("dd");
            Debug.Log(dmg);
        }
        if(Input.GetKeyDown(KeyCode.Z)
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            Debug.Log("dss");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            ground = true;
            jumpCount = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            ground = false;
        }
    }
}

