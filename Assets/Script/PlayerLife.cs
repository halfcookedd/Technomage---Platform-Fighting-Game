using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxLives = 3; // Jumlah nyawa maksimal
    [SerializeField] private float fallDeathYThreshold = -10f; // Batas Y untuk kematian jatuh

    private int currentLives; // Nyawa saat ini
    private Vector3 respawnPoint; // Titik respawn

    void Start()
    {
        // Inisialisasi nyawa saat memulai
        currentLives = maxLives;
        // Set titik respawn awal ke posisi awal pemain
        respawnPoint = transform.position;
    }

    void Update()
    {
        // Periksa apakah pemain jatuh di bawah batas Y
        if (transform.position.y < fallDeathYThreshold)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        // Kurangi nyawa
        currentLives--;

        // Periksa apakah pemain masih memiliki nyawa
        if (currentLives <= 0)
        {
            // Panggil metode Game Over
            GameOver();
        }
        else
        {
            // Respawn pemain ke titik awal
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset posisi pemain ke titik respawn
        transform.position = respawnPoint;

        // Reset fisik pemain
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Log untuk debugging
        Debug.Log(rb.name + " Remaining Lives: " + currentLives);
    }

    private void GameOver()
    {
        // Log untuk debugging
        Debug.Log("Game Over! No more lives left.");

        // Reload scene saat game over
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Metode untuk mengatur ulang nyawa (bisa dipanggil dari luar)
    public void ResetLives()
    {
        currentLives = maxLives;
    }

    // Metode untuk mendapatkan jumlah nyawa saat ini
    public int GetCurrentLives()
    {
        return currentLives;
    }
}