using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static GameObject selectedWeapon;

    public void SelectedWeapon(GameObject weaponPrefab)
    {
        selectedWeapon = weaponPrefab;
    }
}
