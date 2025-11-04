using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Object
{
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity = 5;
    [SerializeField] private T _objectPrefab;

    private ObjectPool<T> _pool;
    private SpawnerInfo _spawnerInfo;

    public event Action<SpawnerInfo> ValuesChanged;

    private void OnValidate()
    {
        if (PoolCapacity > MaxPoolCapacity)
            PoolCapacity = MaxPoolCapacity - 1;
    }

    protected void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateObject,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease,
            actionOnDestroy: @object => Destroy(@object.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: MaxPoolCapacity);

        _spawnerInfo = new SpawnerInfo(_objectPrefab.name, 0, 0, 0);

        ValuesChanged?.Invoke(_spawnerInfo);
    }

    private T CreateObject()
    {
        _spawnerInfo.InscreaseCreatedObjectCount();

        ValuesChanged?.Invoke(_spawnerInfo);

        return Instantiate(_objectPrefab);
    }

    protected void GetObject()
    {
        _pool.Get();
    }

    protected virtual void ActionOnGet(T @object)
    {
        @object.gameObject.SetActive(true);

        _spawnerInfo.SetActiveObjectsCount(_pool.CountActive);

        _spawnerInfo.InscreaseSpawnedObjectCount();

        ValuesChanged?.Invoke(_spawnerInfo);
    }

    protected void Release(T @object) 
    {
        if (@object.gameObject.activeSelf)
        {
            _pool.Release(@object);
        }
    }

    protected virtual void ActionOnRelease(T @object)
    {
        @object.gameObject.SetActive(false);

        @object.ResetCharacteristics();

        _spawnerInfo.SetActiveObjectsCount(_pool.CountActive);
        ValuesChanged?.Invoke(_spawnerInfo);
    }
}
