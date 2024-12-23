using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    // private bool isGrounded = false;
    private Animator anim;
    private Vector2 moveInput;
    private bool doubleJump;
    private GameObject currentOneWayPlatform;
    private Knockback knockback;
    private bool facingRight = true; // Untuk melacak arah pemain
    private bool isKnockbacked = false;
    private float knockbackTimer = 0f;
    private float originalGravityScale;

    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpingPower = 20f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Inisialisasi Animator
        originalGravityScale = rb.gravityScale;
    }

    private void Start()
    {
        knockback = GetComponent<Knockback>();
    }

    private void Update()
    {
        if (!isKnockbacked)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

            // Kontrol animasi berjalan
            anim.SetBool("isWalking", moveInput.x != 0);
        }

        // Kontrol animasi melompat dan jatuh
        if (!IsGrounded() && rb.linearVelocity.y > 0)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }
        else if (!IsGrounded() && rb.linearVelocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        // Memanggil Flip jika arah gerakan berubah
        if ((moveInput.x > 0 && !facingRight) || (moveInput.x < 0 && facingRight))
        {
            Flip();
        }

        // Perbarui status knockback
        if (isKnockbacked)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockbacked = false;
                rb.gravityScale = originalGravityScale; // Kembalikan gravity scale ke normal
                anim.SetBool("isWalking", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Menyimpan input arah (A/D atau Panah Kiri/Kanan)
        moveInput = context.ReadValue<Vector2>();

        // Jika sedang tidak dalam knockback, terapkan input ke pergerakan
        if (!isKnockbacked)
        {
            knockback.SetMovement(moveInput);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Reset doubleJump saat di ground
        if (IsGrounded() && !context.performed)
        {
            doubleJump = false;
        }

        if (!isKnockbacked && context.performed)
        {
            if (IsGrounded() || doubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!isKnockbacked)
        {
            if (context.performed)
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(DisableCollision());
                }
            }
        }
        if (!IsGrounded())
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
            isKnockbacked = false; // Reset status isKnockbacked menjadi false
        }
        // Abaikan tabrakan antara Player 1 dan Player 2
        if ((gameObject.CompareTag("Player 1") && collision.gameObject.CompareTag("Player 2")) ||
            (gameObject.CompareTag("Player 2") && collision.gameObject.CompareTag("Player 1")))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return; // Hindari logika tambahan jika diperlukan
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
            isKnockbacked = true; // Set status knockback
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        // Membalik arah pemain dengan rotasi
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void TakeDamage(Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        if (knockback != null)
        {
            isKnockbacked = true;
            knockbackTimer = knockbackDuration;
            rb.gravityScale = 3f; // Reset gravity scale saat terkena knockback
            knockback.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }
}