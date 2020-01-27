using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim; //Enemy's children (this case Frog) can have access to 'protected' functions
    
    protected virtual void Start() //Children can use this function and override it when 'virtual'
    {
        anim = GetComponent<Animator>();
    }

    //Death parameter is set as a Trigger in Frog Animator view
    //Has to be public to PlayerController can use this function
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
    }

    //Destroy the whole Frog object
    //Added at the end of Enemy_Death animation in the Frog's Animation tab
    private void Death()
    {
        Destroy(this.gameObject);
    }

}
