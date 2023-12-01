using UnityEngine;
using UnityEngine.UI;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public Button button;

    private void Start()
    {
        // Find the main camera and use its transform as the spawn point
        if (spawnPoint == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                spawnPoint = mainCamera.transform;
            }
            else
            {
                Debug.LogError("Spawn point not set and no main camera found!");
            }
        }

        // Assign a method to the button's onClick event
        button.onClick.AddListener(SpawnPrefabAtSpawnPoint);
    }

    private void SpawnPrefabAtSpawnPoint()
    {
        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position + spawnPoint.forward, spawnPoint.rotation);
        }
    }
}

