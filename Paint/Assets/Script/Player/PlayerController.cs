using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;

    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float gorundCheckRadius = .2f;
    public bool facingRight = true;
    private bool isDead = false;

    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, gorundCheckRadius, groundLayer);
        Jump();
    }

    void PlayerMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
