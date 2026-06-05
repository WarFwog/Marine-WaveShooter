using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public int selectedShipIndex;
    public int selectedTurretIndex;
    public int[] selectedTurretsPerPlot = new int[6];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < selectedTurretsPerPlot.Length; i++) {
            selectedTurretsPerPlot[i] = -1;
        }
    }

    public void SetShip(int index)
    {
        selectedShipIndex = index;
    }
    public void SetTurret(int index)
    {
        selectedTurretIndex = index;
    }
}
