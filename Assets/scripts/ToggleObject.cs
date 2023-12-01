using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject objectToToggle; // Reference to the game object you want to toggle

    private bool isObjectVisible = false; // Flag to track the visibility of the object

    public void ToggleVisibility()
    {
        isObjectVisible = !isObjectVisible; // Toggle the flag

        objectToToggle.SetActive(isObjectVisible); // Set the object's visibility based on the flag
    }
}
