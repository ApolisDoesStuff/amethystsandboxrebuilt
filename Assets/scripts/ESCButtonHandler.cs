using UnityEngine;

public class ESCButtonHandler : MonoBehaviour
{
  public GameObject panel; // The UI panel to enable

  void Update()
  {
      if (Input.GetKey(KeyCode.Escape))
      {
          if (panel.activeSelf)
          {
              panel.SetActive(false);
              Cursor.lockState = CursorLockMode.Locked;
              Cursor.visible = false;
          }
          else
          {
              panel.SetActive(true);
              Cursor.lockState = CursorLockMode.None;
              Cursor.visible = true;
          }
      }
  }
}
