using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb : Object
{
    [SerializeField] private MeshRenderer _renderer;

    private Coroutine _opacityCoroutine;
    private Color _originalColor;

    public event Action<Bomb> OldEnough;

    protected override void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Lifespan = Random.Range(MinLifespan, MaxLifespan);
        _originalColor = _renderer.material.color;
    }

    protected override void OnEnable()
    {
        Coroutine = StartCoroutine(Aging());

        _opacityCoroutine = StartCoroutine(ChangingOpacity());
    }

    public override void ResetCharacteristics()
    {
        CurrentLife = 0;
        _renderer.material.color = _originalColor;
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

    private IEnumerator ChangingOpacity()
    {
        Color color = _renderer.material.color;

        int fullTransperancy = 0;

        while (_renderer.material.color.a != fullTransperancy)
        {
            color.a = Mathf.Lerp(color.a, fullTransperancy, Time.deltaTime / Lifespan);

            _renderer.material.color = color;

            yield return null;
        }
    }
}
