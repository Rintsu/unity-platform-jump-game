using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim; //Enemy's children (Frog and Eagle) can have access to 'protected' functions
    protected Rigidbody2D rb;
    protected AudioSource death;
    
    protected virtual void Start() //Children can use this function and override it when 'virtual'
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    //Death parameter is set as a Trigger in Frog Animator view
    //Has to be public to PlayerController can use this function
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.velocity = Vector2.zero; // same as: new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
    }

    //Destroy the whole Frog object
    //Added at the end of Enemy_Death animation in the Frog's Animation tab
    private void Death()
    {
        Destroy(this.gameObject);
    }

}
