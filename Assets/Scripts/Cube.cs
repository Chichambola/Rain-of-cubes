using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private int _maxLifespan = 5;
    private int _minLifespan = 2;
    private int _currentLife;
    private int _lifespan;
    private bool _isCollisonOccured = false;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigiRigidbody;

    public event Action<Cube> OldEnough;
    public bool IsDead => _currentLife == _lifespan;

    private void Awake()
    {
        _rigiRigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _lifespan = Random.Range(_minLifespan, _maxLifespan);
    }

    private void OnCollisionEnter()
    {
        OldEnough?.Invoke(this);

        if (_isCollisonOccured == false)
        {
            _meshRenderer.material.color = Random.ColorHSV();

            StartCoroutine(Aging());
            StopCoroutine(Aging());

            _isCollisonOccured = true;
        }
    }

    private IEnumerator Aging()
    {
        int delay = 1;

        while (_currentLife < _lifespan)
        {
            _currentLife++;

            Debug.Log(_currentLife);

            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
