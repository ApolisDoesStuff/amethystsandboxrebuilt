using UnityEngine;

public class GraphicsSettingsController : MonoBehaviour
{
    public void SetGraphicsToLow()
    {
        QualitySettings.SetQualityLevel(0); // 0 corresponds to the "Low" quality level
    }

    public void SetGraphicsToHigh()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1); // Set to the highest available quality level
    }
}
