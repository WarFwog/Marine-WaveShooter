using UnityEngine;
using UnityEngine.UI;

public class WeaponCrosshair : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Your player ship")]
    public Transform player;

    [Tooltip("The UI Image that will show the crosshair")]
    public Image uiCrosshairImage;

    [Header("Crosshair Settings")]
    public float defaultRadius = 4f;
    public float currentRadius = 4f;

    [System.Serializable]
    public class WeaponCrosshairData
    {
        public string weaponName;
        public Sprite crosshairSprite;
        public float radius = 5f;
        public Color color = Color.white;
    }

    public WeaponCrosshairData[] weaponCrosshairs;

    private Camera mainCam;
    private string currentWeaponName = "Default";

    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;

        if (uiCrosshairImage == null)
            Debug.LogError("UI Crosshair Image is not assigned!");

        UpdateCrosshairForWeapon("Default");
    }

    void Update()
    {
        if (player == null || uiCrosshairImage == null) return;

        // Get mouse position in world space
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = player.position.z;

        // Direction from player to mouse
        Vector3 direction = mouseWorldPos - player.position;
        float distance = direction.magnitude;

        // Clamp to current weapon radius
        if (distance > currentRadius)
        {
            direction = direction.normalized * currentRadius;
        }

        // Final world position for the crosshair
        Vector3 worldCrosshairPos = player.position + direction;

        // === Convert world position to screen position for UI ===
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldCrosshairPos);

        // Move the UI crosshair
        uiCrosshairImage.rectTransform.position = screenPos;

        // Optional: make UI crosshair always point "up" (remove if you want rotation)
        uiCrosshairImage.rectTransform.rotation = Quaternion.identity;
    }

    // Call this when switching weapons
    public void UpdateCrosshairForWeapon(string weaponName)
    {
        currentWeaponName = weaponName;

        foreach (var data in weaponCrosshairs)
        {
            if (data.weaponName.ToLower() == weaponName.ToLower())
            {
                if (uiCrosshairImage != null)
                {
                    uiCrosshairImage.sprite = data.crosshairSprite;
                    uiCrosshairImage.color = data.color;
                }
                currentRadius = data.radius;
                return;
            }
        }

        // Fallback
        currentRadius = defaultRadius;
        Debug.LogWarning($"No crosshair data for weapon: {weaponName}");
    }

    // Useful for shooting / aiming
    public Vector2 GetAimDirection()
    {
        if (player == null) return Vector2.right;
        Vector3 worldPos = GetWorldCrosshairPosition();
        return (worldPos - player.position).normalized;
    }

    public Vector3 GetWorldCrosshairPosition()
    {
        // Returns the current clamped world position (good for bullet spawning)
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = player.position.z;

        Vector3 dir = mouseWorld - player.position;
        if (dir.magnitude > currentRadius)
            dir = dir.normalized * currentRadius;

        return player.position + dir;
    }

    void OnDisable()
    {
        Cursor.visible = true;
    }
}
