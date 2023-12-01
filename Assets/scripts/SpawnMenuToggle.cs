using UnityEngine;

public class SpawnMenuToggle : MonoBehaviour
{
    public GameObject objectToToggle; // Reference to the game object you want to toggle

    private bool isObjectVisible = false; // Flag to track the visibility of the object

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleObjectVisibility();
        }
    }

    private void ToggleObjectVisibility()
    {
        isObjectVisible = !isObjectVisible; // Toggle the flag

        objectToToggle.SetActive(isObjectVisible); // Set the object's visibility based on the flag

        if (isObjectVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
