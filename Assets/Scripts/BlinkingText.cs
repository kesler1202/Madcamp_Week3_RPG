using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class BlinkingText : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Adjust the interval between blinks in seconds.
    private TextMeshProUGUI textComponent; // Change to TextMeshProUGUI
    private float timer;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>(); // Get the TextMeshProUGUI component
        timer = blinkInterval;
    }

    void Update()
    {
        // Update the timer.
        timer -= Time.deltaTime;

        // Toggle text visibility when the timer reaches zero.
        if (timer <= 0f)
        {
            textComponent.enabled = !textComponent.enabled; // Toggle visibility
            timer = blinkInterval;
        }
    }
}