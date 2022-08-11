using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public float curPos;
    public float nextPos;
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
        Move();
        Attack();
    }

    public void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Status",  Mathf.Abs((int)h));
        }
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
        if (isAttack) return;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("attack");
            isAttack = true;
        }
        Invoke("AttackTime", 0.1f);
    }
    public void AttackTime()
    {
        isAttack = false;
    }
}
