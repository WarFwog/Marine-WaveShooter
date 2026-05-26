using UnityEngine;
using UnityEngine.InputSystem;

public class GunshipCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                    // Your gunship

    [Header("Camera Settings")]
    public float distance = 15f;
    public float heightOffset = 4f;
    public float rotationSpeed = 90f;

    [Header("Edge Panning")]
    public float edgeSize = 60f;                // How close to edge before panning

    [Header("Crosshair")]
    public Texture2D crosshairTexture;
    public int crosshairSize = 36;

    private float yaw = 0f;
    private float pitch = 20f;

    // For easy access from other scripts
    public static GunshipCamera Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if (target == null)
            Debug.LogWarning("GunshipCamera: Target not assigned!");
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        float horizontal = 0f;
        float vertical = 0f;

        // Edge detection
        if (mousePos.x < edgeSize) horizontal = -1f;
        else if (mousePos.x > Screen.width - edgeSize) horizontal = 1f;

        if (mousePos.y < edgeSize) vertical = -1f;
        else if (mousePos.y > Screen.height - edgeSize) vertical = 1f;

        // Rotate camera when mouse is at edge
        yaw += horizontal * rotationSpeed * Time.deltaTime;
        pitch -= vertical * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -25f, 65f);

        // Position camera
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPos = target.position + rotation * new Vector3(0, heightOffset, -distance);

        transform.position = Vector3.Lerp(transform.position, desiredPos, 8f * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 2f);
    }

    // Helper method for other scripts to get mouse world position
    public bool GetMouseWorldPosition(out Vector3 worldPos, LayerMask layerMask = default)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f, layerMask))
        {
            worldPos = hit.point;
            return true;
        }

        worldPos = ray.GetPoint(50f); // fallback
        return false;
    }

    void OnGUI()
    {
        if (crosshairTexture == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 guiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);

        float half = crosshairSize / 2f;
        GUI.DrawTexture(new Rect(guiPos.x - half, guiPos.y - half, crosshairSize, crosshairSize), crosshairTexture);
    }
}

