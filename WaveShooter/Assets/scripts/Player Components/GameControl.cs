using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public float Turret;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // if (control == null)
        {

            DontDestroyOnLoad(gameObject);
            //control = this;
            // }
            //  else if(control != this)
            {
                // Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
