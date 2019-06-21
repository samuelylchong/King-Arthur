using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;


    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] groundPoints;
    [SerializeField] private float groundRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool airControl;

    private bool attack;
    private bool slide;
    private bool facingRight;
    private bool isGrounded;
    private bool jump;


    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValues();
    }

    private void HandleMovement(float horizontal)
    {
        //if the player is not attacking, then the player is allowed to move.
        if(!myAnimator.GetBool("slide") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Knight_Attack") && (isGrounded || airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Knight_Attack"))
        {
            myRigidbody.velocity = new Vector2(0, 0);
        }

        //jump
        if(isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetBool("land", false);
            myAnimator.SetTrigger("jump");

        }

        //slide
        if (slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Slide"))
        {
            myAnimator.SetBool("slide", true);
        }
        else if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Slide"))
        {
            myAnimator.SetBool("slide", false);
        }


        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if(attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Knight_Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            slide = true;
        }
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            //to flip the charater, the scale either has to be -1 or +1;

            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;

            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        attack = false;
        slide = false;
        jump = false;
    }

    private bool IsGrounded()
    {
        if(myRigidbody.velocity.y <= 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", true);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if(!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}

