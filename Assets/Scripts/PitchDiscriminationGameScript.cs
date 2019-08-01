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

    void Start()
    {
        characterRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        groundCheckRadius = 0.2f;
        Instantiate(turf, new Vector2(-1, -3), gameObject.transform.rotation);
    }

    void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");


        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            characterRb.velocity = new Vector2(characterRb.velocity.x, 300);
        }
    }


}
