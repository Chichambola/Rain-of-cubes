using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Coroutine _coroutine;
    private Cube _cube;

    public event Action<Bomb> Released;

    private void OnEnable()
    {
        _cubeSpawner.Released += Spawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.Released -= Spawn;
    }

    private void Spawn(Cube cube)
    {
        _cube = cube;

        GetObject();
    }

    protected override void ActionOnGet(Bomb bomb)
    {
        base.ActionOnGet(bomb);

        bomb.transform.position = _cube.transform.position;

        bomb.OldEnough += Release;
    }

    protected override void ActionOnRelease(Bomb bomb)
    {
        base.ActionOnRelease(bomb);

        Released?.Invoke(bomb);

        bomb.OldEnough -= Release;
    }
}
