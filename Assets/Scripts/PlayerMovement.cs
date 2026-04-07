using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float wallSlideSpeed = 1f;
    //Настройки прыжка 
    [SerializeField] private float wallJumpForceX = 8f;
    [SerializeField] private float wallJumpForceY = 10f;
    [SerializeField] private float wallJumpControlLockTime = 0.2f;
    private float wallJumpTimer;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private float wallJumpCooldown;
    private float horizontalInput;
    private int jumpCount;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Движение
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Поворот
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(4, 4, 4);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-4, 4, 4);

        // Сброс прыжков
        if (isGrounded() && body.linearVelocity.y <= 0)
        {
            jumpCount = 0;
        }

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Анимации
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());
        animator.SetFloat("yVelocity", body.linearVelocity.y);

        // Прыжок от стены
        if (wallJumpCooldown > 0.2f)
        {
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                //скольжение на стене
                body.linearVelocity = new Vector2(0, -wallSlideSpeed);
            }
            else
                body.gravityScale = 7;
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        // Прыжок с земли
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            animator.SetTrigger("jump");
            jumpCount = 1;
        }
        // Прыжок от стены
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (onWall() && !isGrounded())
            {
                float direction = -Mathf.Sign(transform.localScale.x);

                body.linearVelocity = new Vector2(direction * wallJumpForceX, wallJumpForceY);

                wallJumpTimer = wallJumpControlLockTime;
                wallJumpCooldown = 0;

                animator.SetTrigger("jump");
            }

            wallJumpCooldown = 0;
        }
        // Двойной прыжок
        else if (jumpCount < maxJumps)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            animator.SetTrigger("doubleJump");
            jumpCount++;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        return raycastHit.collider != null;
    }
}