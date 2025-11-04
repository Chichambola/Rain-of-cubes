using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;


public abstract class Object<T>: MonoBehaviour
{
    [SerializeField] protected Color BaseColor;
    
    private int _maxLifespan = 5;
    private int _minLifespan = 2;
    protected int CurrentLife;
    protected int Lifespan;
    protected int AgingDelay = 1;

    protected MeshRenderer MeshRenderer;
    protected Rigidbody Rigidbody;
    protected Coroutine Coroutine;
    
    public virtual event Action<T> OldEnough;
    protected bool IsDead => CurrentLife == Lifespan;
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MeshRenderer = GetComponent<MeshRenderer>();
        Lifespan = Random.Range(_minLifespan, _maxLifespan);
    }

    private void Start()
    {
        Coroutine = StartCoroutine(Aging());
    }

    protected abstract IEnumerator Aging();

    public abstract void ResetCharacteristics();
}
