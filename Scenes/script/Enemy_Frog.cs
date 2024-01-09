using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{

    private Rigidbody2D rb;

    public Transform leftpoint,rightpoint;

    private float leftfx, rightx;

    private Animator Anim;

    private Collider2D Coll;

    public LayerMask Ground;

    public float Speed,Jumpforce;

    public bool Faceleft = true;
    
     protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftfx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    
    void Update()
    {
        // Movement();
        SwitchAnim();
    }

    void Movement()
    {
        if (Faceleft)
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(-Speed,Jumpforce);
            }
            if (transform.position.x < leftfx)
            {
                transform.localScale = new Vector3(-1, 1, 1); 
                Faceleft = false;
            }
        }
        else
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(Speed,Jumpforce);
            }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1,1,1);
                Faceleft = true;
            }
        }

    }

    void SwitchAnim()
    {
        if (Anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                Anim.SetBool("Jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        if (Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))
        {
            Anim.SetBool("falling", false);
        }
    }
}
