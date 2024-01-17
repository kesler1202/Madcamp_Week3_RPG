using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class SceneChanger : MonoBehaviour
{
    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Load 'MapScene'
            SceneManager.LoadScene("SceneBetwScene");
        }
    }
}