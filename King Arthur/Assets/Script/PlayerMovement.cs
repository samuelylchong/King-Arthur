using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    float horizontalMove = 0f;
    public float runSpeed = 30f;
    bool jump = false;
    bool crouch = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left = -1, right = 1
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        //check jump button
        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        //check crouch button
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }


        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        //Debug.Log(Input.GetButtonDown("Crouch"));

    }

    public void onLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        //move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
