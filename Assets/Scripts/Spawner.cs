using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private Collider _spawnArea;
    [SerializeField, Range(1,5)] private int  _delay;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _maxPoolCapacity = 5;

    private ObjectPool<Cube> _pool;

    private void OnValidate()
    {
        if (_poolCapacity > _maxPoolCapacity)
            _poolCapacity = _maxPoolCapacity -1;
    }

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRealese(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxPoolCapacity);
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        _cube.OldEnough += ActionOnRealese;

        float spawnAreaMinX = _spawnArea.bounds.min.x;
        float spawnAreaMaxX = _spawnArea.bounds.max.x;

        float spawnAreaMinZ = _spawnArea.bounds.min.z;
        float spawnAreaMaxZ = _spawnArea.bounds.max.z;

        float cubePositionX = Random.Range(spawnAreaMinX, spawnAreaMaxX); 
        float cubePositionY = _spawnArea.bounds.min.y;
        float cubePositionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        cube.gameObject.transform.position = new Vector3(cubePositionX, cubePositionY, cubePositionZ);

        cube.gameObject.SetActive(true);
    }

    private IEnumerator Spawn()
    {
        while(_poolCapacity < _maxPoolCapacity)
        {
            GetCube();

            yield return new WaitForSecondsRealtime(_delay);
        }
    }

    private void ActionOnRealese(Cube cube)
    {
        _pool.Release(cube);

        _cube.OldEnough -= ActionOnRealese;

        cube.gameObject.SetActive(false);
    }
}
