using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager
using System.Collections; // Import for Coroutine

public class BeforeStage : MonoBehaviour
{
    void Start()
    {
        // Start the coroutine
        StartCoroutine(ChangeSceneAfterDelay(1)); // 1 second delay
    }

    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the new scene
        SceneManager.LoadScene("MapScene"); // Replace with your scene name
    }
}