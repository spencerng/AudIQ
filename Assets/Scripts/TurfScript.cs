using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurfScript : MonoBehaviour
{

    Rigidbody2D turfRb;
    public GameObject turf, player;

    private void Start()
    {
        turfRb = gameObject.GetComponent<Rigidbody2D>();
        
    }


    void Update()
    {
        turfRb.velocity = new Vector2(-50, turfRb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eraser")
        {
            Destroy(gameObject);
            Debug.Log("Turf destroyed");
            
        }
    }
}
