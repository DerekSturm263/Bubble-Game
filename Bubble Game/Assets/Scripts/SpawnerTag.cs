using UnityEngine;

public class SpawnerTag : MonoBehaviour
{
    private SpawnOnTimer _spawner;
    public void SetSpawner(SpawnOnTimer spawner) => _spawner = spawner;

    private void Start()
    {
        _spawner.IncrementSpawned();
    }

    private void OnDestroy()
    {
        _spawner.DecrementSpawned();
    }
}
