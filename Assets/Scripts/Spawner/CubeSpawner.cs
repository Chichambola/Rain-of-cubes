using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField, Range(1, 5)] private int _delay;
    [SerializeField] private Collider _spawnArea;

    private Coroutine _coroutine;

    public event Action<Cube> Released;

    private void Start()
    {
        _coroutine = StartCoroutine(Spawn());
    }

    protected override void ActionOnGet(Cube cube)
    {
        float spawnAreaMinX = _spawnArea.bounds.min.x;
        float spawnAreaMaxX = _spawnArea.bounds.max.x;

        float spawnAreaMinZ = _spawnArea.bounds.min.z;
        float spawnAreaMaxZ = _spawnArea.bounds.max.z;

        float cubePositionX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float cubePositionY = _spawnArea.bounds.min.y;
        float cubePositionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        cube.gameObject.transform.position = new Vector3(cubePositionX, cubePositionY, cubePositionZ);

        base.ActionOnGet(cube);

        cube.OldEnough += Release;
    }

    protected override void ActionOnRelease(Cube cube)
    {
        Released?.Invoke(cube);

        base.ActionOnRelease(cube);

        cube.OldEnough -= Release;
    }

    private IEnumerator Spawn()
    {
        while (PoolCapacity < MaxPoolCapacity)
        {
            GetObject();

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
}
