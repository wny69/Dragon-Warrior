using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float wallJumpXForce = 8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Animator animator;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown = 1f;
    private float moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput = 1f;

        if (wallJumpCooldown > 0.2f)
        {
            body.linearVelocity = new Vector2(moveInput * speed, body.linearVelocity.y);

            if (OnWall() && !IsGrounded())
                body.gravityScale = 0;
            else
                body.gravityScale = 1;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Jump();
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        animator.SetBool("run", moveInput != 0);
        animator.SetBool("grounded", IsGrounded());
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump");
        }
        else if (OnWall() && !IsGrounded())
        {
            float wallDirection = -Mathf.Sign(transform.localScale.x);
            body.linearVelocity = new Vector2(wallDirection * wallJumpXForce, jumpForce);
            transform.localScale = new Vector3(wallDirection, 1, 1);
            wallJumpCooldown = 0;
            animator.SetTrigger("jump");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return moveInput == 0 && IsGrounded() && !OnWall();
    }
}