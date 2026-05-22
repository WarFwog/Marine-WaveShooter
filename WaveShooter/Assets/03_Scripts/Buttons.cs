using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
     public void OnClickGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);



    }

    public void Quitgame()
    {
        Application.Quit();
    }
}