using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 knockbackDirection;
    private float knockbackForce;
    private float knockbackDuration = 0.5f;
    private float knockbackTimer;
    private AnimationCurve knockbackCurve;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    }

    void Update()
    {

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;

            // Hitung waktu relatif untuk kurva akselerasi
            float t = 1f - (knockbackTimer / knockbackDuration);
            float force = knockbackForce * knockbackCurve.Evaluate(t);

            // Terapkan knockback dengan mengatur velocity langsung
            rb.linearVelocity = new Vector2(knockbackDirection.x * force, knockbackDirection.y * force);
        }

    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (rb != null)
        {
            knockbackDirection = direction.normalized;
            knockbackForce = force;
            knockbackDuration = duration;
            knockbackTimer = knockbackDuration;

            Debug.Log($"Applying Knockback - Direction: {knockbackDirection}, Force: {knockbackForce}, Duration: {knockbackDuration}");
        }
    }

    public void SetMovement(Vector2 movement)
    {
        if (knockbackTimer <= 0) // Hanya terapkan gerakan jika knockback sudah selesai
        {
            rb.linearVelocity = new Vector2(movement.x * rb.linearVelocity.magnitude, rb.linearVelocity.y);
        }
    }
}