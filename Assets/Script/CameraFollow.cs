using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player1; // Referensi ke Player 1
    [SerializeField] private Transform player2; // Referensi ke Player 2
    [SerializeField] private float minZoom = 5f; // Zoom minimum
    [SerializeField] private float maxZoom = 15f; // Zoom maksimum
    [SerializeField] private float zoomLimiter = 10f; // Pengatur kecepatan zoom
    [SerializeField] private float smoothSpeed = 0.125f; // Kecepatan smoothing

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Ambil komponen Camera
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        MoveCamera();
        AdjustZoom();
    }

    void MoveCamera()
    {
        // Posisi tengah antara kedua pemain
        Vector3 centerPoint = (player1.position + player2.position) / 2f;

        // Posisi target kamera
        Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);

        // Gerakkan kamera secara halus ke posisi target
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
    }

    void AdjustZoom()
    {
        // Hitung jarak antara kedua pemain
        float distance = Vector3.Distance(player1.position, player2.position);

        // Hitung ukuran kamera (zoom level)
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);

        // Atur zoom secara halus
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
}