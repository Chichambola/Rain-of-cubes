using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : PoolableObject
{
    private bool _isCollisionOccured = false;

    public event Action<Cube> OldEnough;

    protected override void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        Rigidbody = GetComponent<Rigidbody>();
        Lifespan = Random.Range(MinLifespan, MaxLifespan);
    }

    protected override void OnEnable()
    {
        Renderer.material.color = OriginalColor;

        Coroutine = StartCoroutine(Aging());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollisionOccured == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            Renderer.material.color = Random.ColorHSV();

            StartCoroutine(Aging());

            _isCollisionOccured = true;
        }
    }

    protected override IEnumerator Aging()
    {
        var wait = new WaitForSeconds(AgingDelay);

        while (CurrentLife != Lifespan)
        {
            CurrentLife++;

            yield return wait;
        }

        if (IsDead)
            OldEnough?.Invoke(this);
    }

    public override void ResetCharacteristics()
    {
        CurrentLife = 0;
        _isCollisionOccured = false;
        Renderer.material.color =  OriginalColor;
    }
}
