using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy //Inheritance from Enemy.cs
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    //private Rigidbody2D rb; //Rigidbody is for controlling speed and moving

    private bool facingLeft = true;

    protected override void Start() //Using and overriding the Start() from Enemy
    {
        base.Start(); // base is whatever you inherit from the parent
        coll = GetComponent<Collider2D>();
        //rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Transition from jump to fall
        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //Transition from fall to idle
        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    private void Move() 
    {
        if(facingLeft)
        {
            //Test to see if the frog is beyond leftCap
            if(transform.position.x > leftCap)
            {
                //Make sure sprite is facing right location
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //If touching the ground
                if(coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            //Test to see if we are beyond leftCap
            if(transform.position.x < rightCap)
            {
                //Make sure sprite is facing right location
                if(transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //If touching the ground
                if(coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            } 
        }
    }


}
