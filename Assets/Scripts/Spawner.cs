using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public abstract class Spawner<T> : MonoBehaviour where T : Object<T>
{
    [SerializeField] private T _objectPrefab;
    [SerializeField] protected Collider SpawnArea;
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity = 5;

    private ObjectPool<T> _pool;
    private SpawnerInfo _spawnerInfo;

    private void OnValidate()
    {
        if (PoolCapacity > MaxPoolCapacity)
            PoolCapacity = MaxPoolCapacity -1;
    }

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateObject,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease,
            actionOnDestroy:  @object => Destroy(@object.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: MaxPoolCapacity);
    }

    private T CreateObject()
    {
        Debug.Log("Spawning object");
        
        return Instantiate(_objectPrefab);
    }
    
    protected void GetObject()
    {
        _pool.Get();
    }

    protected virtual void ActionOnGet(T @object)
    {
    }

    protected virtual void Release(T @object)
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
    }
}
