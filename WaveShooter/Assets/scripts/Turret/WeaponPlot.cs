using UnityEngine;

public class WeaponPlot : MonoBehaviour
{
    public GameObject currentWeapon;
    public int plotIndex;

    public void BuildWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(
            weaponPrefab,
            transform.position,
            transform.rotation,
            transform
            );

        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        if (plotIndex < 0 || plotIndex >= GameControl.Instance.selectedTurretsPerPlot.Length)
        {
            return;
        }
        GameControl.Instance.selectedTurretsPerPlot[plotIndex] = GameControl.Instance.selectedTurretIndex;
   


    }

    private void OnMouseDown()
    {
        if (ShopManager.selectedWeapon != null)
        {
            BuildWeapon(ShopManager.selectedWeapon);
        } 
    }

}
