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

    void Start()
    {
        characterRb = GameObject.Find("player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");

        characterRb.velocity = new Vector2(3, characterRb.velocity.y);
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
}
