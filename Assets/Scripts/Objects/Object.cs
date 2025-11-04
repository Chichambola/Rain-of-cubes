using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class Object : MonoBehaviour
{
    protected int MaxLifespan = 5;
    protected int MinLifespan = 2;
    protected int CurrentLife = 0;
    protected int Lifespan = 0;
    protected int AgingDelay = 1;

    protected Rigidbody Rigidbody;
    protected Coroutine Coroutine;

    protected bool IsDead => CurrentLife == Lifespan;

    protected abstract void Awake();

    protected abstract void OnEnable();

    protected abstract IEnumerator Aging();

    public abstract void ResetCharacteristics();
}
