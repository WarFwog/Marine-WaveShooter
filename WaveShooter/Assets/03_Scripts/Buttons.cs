using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
     public void OnClickGame()
    {
        SceneManager.LoadScene(1);
        // SceneManager.LoadScene("LobbyScreen");



    }

    public void Quitgame()
    {
        Application.Quit();
    }
}