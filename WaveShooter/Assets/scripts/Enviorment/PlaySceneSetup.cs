using UnityEngine;

public class PlaySceneSetup : MonoBehaviour
{
    public GameObject[] turretPrefab;
    public WeaponPlot[] targetPlots;

    private void Start()
    {
        for (int i = 0; i < targetPlots.Length; i++)
        {
            int turretIndex = GameControl.Instance.selectedTurretsPerPlot[i];

            if (turretIndex == -1)
            {
                continue;
            }

            if (targetPlots[i] == null)
            {
                continue;
            }

            if (turretIndex < 0 || turretIndex >= turretPrefab.Length || turretPrefab[turretIndex] == null)
            { 
                continue;
            }

            targetPlots[i].BuildWeapon(turretPrefab[turretIndex]);
        }
    }
}
