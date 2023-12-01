using UnityEngine;
using TMPro;

public class RandomTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] textOptions;

    private void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro object reference not set in the inspector!");
            return;
        }

        if (textOptions.Length == 0)
        {
            Debug.LogError("No text options provided in the inspector!");
            return;
        }

        // Randomly select text from the options
        string randomText = textOptions[Random.Range(0, textOptions.Length)];

        // Set the selected text to the TextMeshPro object
        textMeshPro.text = randomText;
    }
}
