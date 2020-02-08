using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHelper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //When the player collides with GravityHelper box collider
            //it will untick the PlayerController script -> disabled
            //This solves the problem where player could climb the obstacles after falling
            //Not anymore!
            collision.gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
}
