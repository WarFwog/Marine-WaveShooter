using UnityEngine;

public class ShipCollector : MonoBehaviour
{
   
    public GameObject[] turretPrefab;
  

    public void ChooseShip(int ShipIndex)
    {
        GameControl.Instance.SetShip(ShipIndex);
        
    }

    public void ChooseTurret(int TurretIndex)
    { 
        GameControl.Instance.SetTurret(TurretIndex);
        ShopManager.selectedWeapon = turretPrefab[TurretIndex];
    }


}
