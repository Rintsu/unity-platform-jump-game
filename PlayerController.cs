using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //Finite State Machines FSM
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;

    //Inspector variables (shown in the player inspector)
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f; //Just starting values, but can be modified in Inspector
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0; //At the beginning player has 0 cherries collected
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        //Updates the state
        //Sets animation based on Enumerator state on line 10
        anim.SetInteger("state", (int)state);
    }

    //Unity knows that if you have 'Is Trigger' ticked it will run onTriggerEnter function
    //We have ticked cherry's sprite 'Is Trigger' -> the cherries will be collected on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries++;
            cherryText.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            //Get the enemy object from Enemy.cs
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(state == State.falling)
            {
                //JumpedOn is defined in Frog.cs which sets Trigger "Death"
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is on the right, player is damaged and moved to left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is on the left, player is damaged and moved to right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        
        //Moving left
        if(hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //Facing left
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving right
        else if(hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //Facing right
            transform.localScale = new Vector2(1, 1);
        }
        //Jumping only when player is touching layer
        //Edit -> Project Settings -> Input -> Jump -> Name (hence "Jump")
        if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        //2f is generating a small slide when stop running
        else if(Mathf.Abs(rb.velocity.x) > 1f)
        {
            //Moving
            state = State.running;
        }
        else 
        {
            state = State.idle;
        }
    }
}
