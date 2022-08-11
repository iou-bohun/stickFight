using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow)) ;
    }
}
