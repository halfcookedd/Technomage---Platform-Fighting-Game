using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    public float speed = 50f; // Kecepatan peluru
    public float lifetime = 2f; // Waktu hidup peluru sebelum dihancurkan
    public float knockbackForce = 8f; // Kekuatan knockback
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Set kecepatan peluru
        Destroy(gameObject, lifetime); // Hancurkan peluru setelah waktu tertentu
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        //abaikan objek dengan tag "OneWayPlatform"
        if (collision.CompareTag("OneWayPlatform"))
        {
            return;
        }

        PlayerController hitPlayer = collision.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            hitPlayer.TakeDamage(knockbackDirection, knockbackForce, 0.5f);
        }

        Destroy(gameObject);
    }
}
