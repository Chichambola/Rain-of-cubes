using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField, Range(1,5)] private int  _delay;
    
    private Coroutine _coroutine;
    
    public event Action<Cube> Released;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _coroutine = StartCoroutine(Spawn());
    }

    protected override void ActionOnGet(Cube cube)
    {
        float spawnAreaMinX = SpawnArea.bounds.min.x;
        float spawnAreaMaxX = SpawnArea.bounds.max.x;

        float spawnAreaMinZ = SpawnArea.bounds.min.z;
        float spawnAreaMaxZ = SpawnArea.bounds.max.z;

        float cubePositionX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float cubePositionY = SpawnArea.bounds.min.y;
        float cubePositionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        cube.gameObject.transform.position = new Vector3(cubePositionX, cubePositionY, cubePositionZ);

        cube.gameObject.SetActive(true);

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
        while(PoolCapacity < MaxPoolCapacity)
        {
            GetObject();

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
}
