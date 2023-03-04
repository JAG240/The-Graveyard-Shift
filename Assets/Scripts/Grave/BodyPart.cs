using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.name == "Player")
        {
            PlayerInventory inv = collision.gameObject.GetComponent<PlayerInventory>();
            if(inv.AddToHand(transform.name))
                Destroy(gameObject);
        }
    }
}
