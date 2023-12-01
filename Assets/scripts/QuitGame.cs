using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Exit Play mode in the Unity Editor
        #else
            Application.Quit(); // Quit the application in a built executable
        #endif
    }
}
