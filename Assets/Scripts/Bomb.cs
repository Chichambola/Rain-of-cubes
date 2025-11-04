using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class Bomb : Object<Bomb>
{
    public override event Action<Bomb> OldEnough;
    
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
        MeshRenderer.material.color = BaseColor;
    }
}
