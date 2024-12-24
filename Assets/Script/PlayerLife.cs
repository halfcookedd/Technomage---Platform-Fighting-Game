using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxLives = 3; // Jumlah nyawa maksimal
    [SerializeField] private float fallDeathYThreshold = -10f; // Batas Y untuk kematian jatuh
    [SerializeField] private TMP_Text winText; // Referensi ke UI Text untuk menampilkan pemenang
    [SerializeField] private Button restartButton; // Referensi ke tombol Restart
    [SerializeField] private Button mainMenuButton; // Referensi ke tombol Main Menu
    [SerializeField] private GameObject bgDark; // Referensi ke tombol Main Menu
    [SerializeField] private GameObject otherPlayer; // Referensi ke pemain lain



    private int currentLives; // Nyawa saat ini
    private Vector3 respawnPoint; // Titik respawn
    private bool isGameOver = false; // Status permainan
    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentLives = maxLives;
        respawnPoint = transform.position;

        // Pastikan UI diatur ke kondisi awal
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        bgDark.gameObject.SetActive(false);


        // Tambahkan listener ke tombol
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (isGameOver) return;

        if (transform.position.y < fallDeathYThreshold)
        {
            TakeDamage();
            audioManager.PlaySFX(audioManager.deathSound);
        }
    }

    public void TakeDamage()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        Debug.Log(rb.name + " Remaining Lives: " + currentLives);
    }

    private void GameOver()
    {
        isGameOver = true;

        // Menampilkan background gelap
        bgDark.gameObject.SetActive(true);

        // Menampilkan UI Game Over
        string winningPlayer = otherPlayer.name; // Pemain lain adalah pemenang
        winText.text = winningPlayer + " Wins!";
        winText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        Debug.Log("Game Over! " + winningPlayer + " Wins!");
    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Ganti dengan nama scene main menu Anda
    }
}


// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class PlayerLife : MonoBehaviour
// {
//     [SerializeField] private int maxLives = 3; // Jumlah nyawa maksimal
//     [SerializeField] private float fallDeathYThreshold = -10f; // Batas Y untuk kematian jatuh

//     private int currentLives; // Nyawa saat ini
//     private Vector3 respawnPoint; // Titik respawn
//     AudioManager audioManager;

//     void Awake()
//     {
//         audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//     }

//     void Start()
//     {
//         // Inisialisasi nyawa saat memulai
//         currentLives = maxLives;
//         // Set titik respawn awal ke posisi awal pemain
//         respawnPoint = transform.position;
//     }

//     void Update()
//     {
//         // Periksa apakah pemain jatuh di bawah batas Y
//         if (transform.position.y < fallDeathYThreshold)
//         {
//             TakeDamage();
//             audioManager.PlaySFX(audioManager.deathSound);
//         }
//     }

//     public void TakeDamage()
//     {
//         // Kurangi nyawa
//         currentLives--;

//         // Periksa apakah pemain masih memiliki nyawa
//         if (currentLives <= 0)
//         {
//             // Panggil metode Game Over
//             GameOver();
//         }
//         else
//         {
//             // Respawn pemain ke titik awal
//             Respawn();
//         }
//     }

//     private void Respawn()
//     {
//         // Reset posisi pemain ke titik respawn
//         transform.position = respawnPoint;

//         // Reset fisik pemain
//         Rigidbody2D rb = GetComponent<Rigidbody2D>();
//         if (rb != null)
//         {
//             rb.linearVelocity = Vector2.zero;
//         }

//         // Log untuk debugging
//         Debug.Log(rb.name + " Remaining Lives: " + currentLives);
//     }

//     private void GameOver()
//     {
//         // Log untuk debugging
//         Debug.Log("Game Over! No more lives left.");

//         // Reload scene saat game over
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     // Metode untuk mengatur ulang nyawa (bisa dipanggil dari luar)
//     public void ResetLives()
//     {
//         currentLives = maxLives;
//     }

//     // Metode untuk mendapatkan jumlah nyawa saat ini
//     public int GetCurrentLives()
//     {
//         return currentLives;
//     }
// }