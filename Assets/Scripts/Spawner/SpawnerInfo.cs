using UnityEngine;

public class SpawnerInfo : MonoBehaviour
{
    public string ObjectName { get; private set; }
    public int SpawnedObjects { get; private set; }
    public int CreatedObjects { get; private set; }
    public int ActiveObjects { get; private set; }

    public void SetStartValues(string objectName, int spawnedObjects, int createdObjects, int activeObjects)
    {
        ObjectName = objectName;
        SpawnedObjects = spawnedObjects;
        CreatedObjects = createdObjects;
        ActiveObjects = activeObjects;
    }

    public void InscreaseSpawnedObjectCount()
    {
        SpawnedObjects++;
    }

    public void InscreaseCreatedObjectCount()
    {
        CreatedObjects++;
    }

    public void SetActiveObjectsCount(int count)
    {
        ActiveObjects = count;
    }
}
