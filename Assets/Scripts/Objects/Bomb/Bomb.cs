using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb : PoolableObject
{
    [SerializeField] private Exploder _exploder;

    private Coroutine _opacityCoroutine;

    public event Action<Bomb> OldEnough;

    protected override void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<MeshRenderer>();
        Lifespan = Random.Range(MinLifespan, MaxLifespan);
        OriginalColor = Renderer.material.color;
    }

    protected override void OnEnable()
    {
        Coroutine = StartCoroutine(Aging());

        _opacityCoroutine = StartCoroutine(ChangingOpacity());
    }

    public override void ResetCharacteristics()
    {
        CurrentLife = 0;
        Renderer.material.color = OriginalColor;
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

        _exploder.CreateExplosion(this);
    }

    private IEnumerator ChangingOpacity()
    {
        Color color = Renderer.material.color;

        int fullTransperancy = 0;

        while (Renderer.material.color.a != fullTransperancy)
        {
            color.a = Mathf.Lerp(color.a, fullTransperancy, Time.deltaTime / Lifespan);

            Renderer.material.color = color;

            yield return null;
        }
    }
}
