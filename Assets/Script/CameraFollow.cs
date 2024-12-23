using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Zoom Settings")]
    [SerializeField] private float baseZoom = 1f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 4f;
    [SerializeField] private float zoomThreshold = 10f;
    [SerializeField] private float maxPlayerDistance = 20f;
    [SerializeField] private float zoomSmoothSpeed = 2f;

    [Header("Movement Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;
    
    [Header("Camera Bounds")]
    [SerializeField] private float minX = -10f; // Batas kiri
    [SerializeField] private float maxX = 10f;  // Batas kanan
    [SerializeField] private float minY = -5f;  // Batas bawah
    [SerializeField] private float maxY = 5f;   // Batas atas

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = baseZoom;
        UpdateCameraBounds();
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        UpdateCameraBounds();
        MoveCamera();
        AdjustZoom();
    }

    void UpdateCameraBounds()
    {
        // Update camera dimensions
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void MoveCamera()
    {
        Vector3 centerPoint = (player1.position + player2.position) / 2f;
        Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);

        // Batasi posisi kamera dengan memperhitungkan ukuran viewport kamera
        newPosition.x = Mathf.Clamp(newPosition.x, minX + halfWidth, maxX - halfWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minY + halfHeight, maxY - halfHeight);

        // Gerakkan kamera dengan smooth
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
    }

    void AdjustZoom()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        float targetZoom;

        if (distance < zoomThreshold)
        {
            targetZoom = baseZoom;
        }
        else
        {
            float clampedDistance = Mathf.Min(distance, maxPlayerDistance);
            float zoomFactor = (clampedDistance - zoomThreshold) / (maxPlayerDistance - zoomThreshold);
            targetZoom = Mathf.Lerp(baseZoom, maxZoom, zoomFactor);
        }

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothSpeed);
        
        // Update bounds setelah zoom berubah
        UpdateCameraBounds();
    }

    // Method untuk mengatur batas kamera dari script lain
    public void SetCameraBounds(float leftBound, float rightBound, float bottomBound, float topBound)
    {
        minX = leftBound;
        maxX = rightBound;
        minY = bottomBound;
        maxY = topBound;
    }

    // Method untuk visualisasi batas kamera di editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gambar rectangle untuk menunjukkan batas kamera
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(minX, minY, 0));
    }
}