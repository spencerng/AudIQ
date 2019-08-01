using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchDiscriminationGameScript : MonoBehaviour
{
    Rigidbody2D characterRb;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool onGround;
    public GameObject turf;

    int frameCount;

    void Start()
    {
        characterRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        groundCheckRadius = 0.2f;
        frameCount = -100;
        
        
    }

    void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");


        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            characterRb.velocity = new Vector2(characterRb.velocity.x, 300);
        }

        frameCount++;

        Debug.Log(frameCount);

        if (frameCount >= 300)
        {
            frameCount = 0;
           
            GameObject newPlatform = Instantiate(turf, new Vector2(1200, 250), characterRb.transform.rotation);
            newPlatform.AddComponent<TurfScript>();
            newPlatform = Instantiate(turf, new Vector2(1200, -250), characterRb.transform.rotation);
            newPlatform.AddComponent<TurfScript>();
        }

        
    }


}
