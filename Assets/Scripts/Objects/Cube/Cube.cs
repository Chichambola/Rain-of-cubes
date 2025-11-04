using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : Object
{
    [SerializeField] private Color _originalColor;

    private bool _isCollisionOccured = false;
    private MeshRenderer _meshRenderer;

    public event Action<Cube> OldEnough;

    protected override void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        Rigidbody = GetComponent<Rigidbody>();
        Lifespan = Random.Range(MinLifespan, MaxLifespan);
    }

    protected override void OnEnable()
    {
        _meshRenderer.material.color = _originalColor;

        Coroutine = StartCoroutine(Aging());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollisionOccured == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            _meshRenderer.material.color = Random.ColorHSV();

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
        _meshRenderer.material.color = _originalColor;
    }
}
