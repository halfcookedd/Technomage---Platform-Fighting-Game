using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Transform shootingPoint; // Titik dari mana peluru akan ditembakkan
    public GameObject projectilePrefab; // Prefab peluru
    public float shootCooldown = 0.5f; // Waktu cooldown antara tembakan
    private float lastShootTime; // Waktu terakhir peluru ditembakkan

    private Animator anim; // Referensi ke Animator

    [SerializeField] private float meleeAttackRange = 1f; // Jarak serangan jarak dekat
    [SerializeField] private LayerMask enemyLayer; // Layer untuk musuh

    void Start()
    {
        anim = GetComponent<Animator>(); // Inisialisasi Animator
        lastShootTime = -shootCooldown; // Inisialisasi agar bisa langsung menembak
    }

    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Jalankan animasi serangan jarak dekat
            anim.SetTrigger("isMelee");

            // Deteksi musuh dalam jangkauan serangan
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shootingPoint.position, meleeAttackRange, enemyLayer);

            // Beri damage ke musuh
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit enemy: " + enemy.name);
                // Tambahkan logika untuk memberi damage ke musuh
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= lastShootTime + shootCooldown)
        {
            // Jalankan animasi serangan jarak jauh
            anim.SetTrigger("isRange");

            // Tembakkan proyektil
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

            // Set waktu terakhir tembakan
            lastShootTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Gambar lingkaran untuk menunjukkan jangkauan serangan jarak dekat (untuk debugging)
        if (shootingPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(shootingPoint.position, meleeAttackRange);
        }
    }
}
