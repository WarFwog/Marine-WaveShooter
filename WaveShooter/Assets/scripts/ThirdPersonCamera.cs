using UnityEngine;

public class GunshipCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                    // Your gunship

    [Header("Camera Settings")]
    public float distance = 12f;
    public float heightOffset = 3f;
    public float rotationSpeed = 80f;           // How fast camera turns when mouse at edge

    [Header("Edge Settings")]
    public float edgeSize = 50f;                // Pixels from edge before camera starts moving
    public float deadZone = 100f;               // Center area where mouse does nothing

    [Header("Crosshair")]
    public Texture2D crosshairTexture;
    public int crosshairSize = 32;

    private float yaw = 0f;
    private float pitch = 15f;

    void Start()
    {
        Cursor.visible = true;           // Mouse cursor is visible
        Cursor.lockState = CursorLockMode.Confined;  // Keep mouse inside game window

        if (target == null)
            Debug.LogError("GunshipCamera: Assign your gunship to the Target field!");
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 mousePos = Input.mousePosition;

        // Calculate how close mouse is to edges
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float horizontal = 0f;
        float vertical = 0f;

        // Left / Right edge
        if (mousePos.x < edgeSize)
            horizontal = -1f;
        else if (mousePos.x > screenWidth - edgeSize)
            horizontal = 1f;

        // Bottom / Top edge
        if (mousePos.y < edgeSize)
            vertical = -1f;
        else if (mousePos.y > screenHeight - edgeSize)
            vertical = 1f;

        // Apply rotation when mouse is at edges
        yaw   += horizontal * rotationSpeed * Time.deltaTime;
        pitch -= vertical * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        // Position camera behind gunship with rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, heightOffset, -distance);

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 2f);
    }

    void OnGUI()
    {
        if (crosshairTexture == null) return;

        Vector2 center = new Vector2(
            Input.mousePosition.x,
            Screen.height - Input.mousePosition.y   // GUI uses top-left as origin
        );

        float half = crosshairSize / 2f;
        GUI.DrawTexture(new Rect(center.x - half, center.y - half, crosshairSize, crosshairSize), crosshairTexture);
    }
}

