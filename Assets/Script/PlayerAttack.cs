using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Transform shootingPoint; // Titik dari mana peluru akan ditembakkan
    public GameObject projectilePrefab; // Prefab peluru
    public GameObject meelePrefab; // Prefab Meele

    public float shootCooldown = 0.5f; // Waktu cooldown antara tembakan
    private float lastShootTime; // Waktu terakhir peluru ditembakkan
    private Animator anim; // Referensi ke Animator

    void Start()
    {
        anim = GetComponent<Animator>(); // Inisialisasi Animator
        lastShootTime = -shootCooldown; // Inisialisasi agar bisa langsung menembak
    }

    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= lastShootTime + shootCooldown)
        {
            // Jalankan animasi serangan jarak dekat
            anim.SetTrigger("isMelee");

            // Tembakkan proyektil
            GameObject projectile = Instantiate(meelePrefab, shootingPoint.position, shootingPoint.rotation);

            // Set waktu terakhir tembakan
            lastShootTime = Time.time;
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
}
