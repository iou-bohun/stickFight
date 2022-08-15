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
    bool roll;

    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    public float curAttackDelay;
    public float maxAttackDealy;
    public int attackStatus = 1;
    public bool attack2;
    public bool attack3;

    public Transform pos;
    public Vector2 boxSize;

    public float firstDmg = 10;
    public float secDmg = 20;
    public float thridDmg = 30;
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
        ComboCheck();
        Attack();
        Jump();
        IsFalling();


        m_timeSinceAttack += Time.deltaTime;
    }

   

    public void Move()
    {
        //#.���ݽ� Ű���� �̵� ���� //////////////////////////////
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) && ground == true) return;
        /////////////////////////////////////////////////////////////

        if (roll == false) speed = 4;
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, 0, 0) * speed * Time.deltaTime;
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
        Rolling();
    }

    //#.�÷��̾� ����
    public void Jump()
    {
        if (jumpCount >= 2) return;
        if (Input.GetKeyDown("space"))
        {
            anim.SetTrigger("Jump");
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            roll = false;
            anim.SetBool("IsRoll", roll);
        }
    }

    //#.�÷��̾� �߶� ����
    public void IsFalling()
    {
        anim.SetFloat("InAir", rigidbody.velocity.y);
        anim.SetBool("IsGround", ground);
    }

    //#.�÷��̾� ����
    void Attack()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Z) && m_timeSinceAttack > 0.3f)
        {
            AttackMove(h);
            AttackCheck();
            anim.SetBool("IsRoll", false);
            anim.SetTrigger("Atk" + m_currentAttack);
            Debug.Log(dmg);
            m_timeSinceAttack = 0.0f;
            roll = false;

            //#. ���ݿ� �ణ�� �����ð� �Ҵ�
            StartCoroutine(EnemyAttack());
        }
    }


    //#.�� ���� 
    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(0.2f);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetDmg(dmg);
            }
        }
    }
    //#.�÷��̾� ���� ���� ���� �ִϸ��̼� 0.85�ۼ�Ʈ ����  zŰ Ŭ���� ���� 
    void ComboCheck()
    {
        if (Input.GetKeyDown(KeyCode.Z)
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            attack2 = true;
        }
        if (Input.GetKeyDown(KeyCode.Z)
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
        {
            attack3 = true;
        }
    }



    //#.������ ��� ������ �ִ��� üũ
    void AttackCheck()
    {
        m_currentAttack++;
        if (m_currentAttack > 3)
        {
            m_currentAttack = 1;
            attack2 = false;
            attack3 = false;
        }
        if (m_timeSinceAttack > 0.8f)
        {
            m_currentAttack = 1;
            attack2 = false;
            attack3 = false;
        }
        //#. ����Ʈ���� 
        if (m_currentAttack == 2 && attack2 == true)
        {
            dmg = secDmg;
        }
        else if (m_currentAttack == 3 && attack3 == true)
        {
            dmg = thridDmg;
        }
        else if (m_currentAttack == 1)
        {
            dmg = firstDmg;
        }
    }

    //#.���ݽ� �̵�
    void AttackMove(float h)
    {
        if (ground == true && h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody.AddForce(Vector2.right * 150f);
        }
        else if (ground == true && h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rigidbody.AddForce(Vector2.left * 150);
        }
    }

    //#.�÷��̾� ������ 
    void Rolling()
    {
        if (!(rigidbody.velocity.y == 0)) return;
        if(Input.GetKeyDown(KeyCode.X))
        {
            roll = true;
            anim.SetTrigger("IsRoll");
            speed = 7;
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Roll")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            roll = false;
            anim.SetBool("IsRoll", roll);
        }
    }

    //#.�÷��̾ ���� �ִ��� ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground = true;
            jumpCount = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground = false;
        }
    }


    //#,���� ���� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}

