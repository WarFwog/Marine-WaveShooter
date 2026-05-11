using UnityEngine;
using UnityEngine.InputSystem;

public class GunshipRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform firePoint;   // Optional: where bullets spawn from

    private InputAction tapAction;

    void Awake()
    {
        tapAction = new InputAction("Tap", binding: "<Pointer>/press");
        tapAction.Enable();
        tapAction.performed += OnTap;
    }

    void OnDestroy()
    {
        tapAction.performed -= OnTap;
        tapAction.Disable();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        if (GunshipCamera.Instance == null) return;

        if (GunshipCamera.Instance.GetMouseWorldPosition(out Vector3 hitPoint, targetLayer))
        {
            Debug.Log("Hit: " + hitPoint);

            // Example: Look at target or shoot
            if (firePoint != null)
            {
                firePoint.LookAt(hitPoint);
                // Spawn bullet / laser here
            }
        }
    }
}
