using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : Object<Cube>
{
    private bool _isCollisionOccured = false;

    public override event Action<Cube> OldEnough;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollisionOccured == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            MeshRenderer.material.color = Random.ColorHSV();

            StartCoroutine(Aging());
            StopCoroutine(Aging());

            _isCollisionOccured = true;
        }
    }

    protected override IEnumerator Aging()
    {
        var wait = new WaitForSeconds(AgingDelay);

        while (CurrentLife < Lifespan)
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
        MeshRenderer.material.color = BaseColor;
    }
}
