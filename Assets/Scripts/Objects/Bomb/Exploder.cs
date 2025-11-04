using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private Bomb _bomb;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _numberOfColliders;

    private Collider[] _hitColliders;

    private void Awake()
    {
        _hitColliders = new Collider[_numberOfColliders];
    }

    private void OnEnable()
    {
        _bomb.OldEnough += CreateExplosion;
    }

    private void OnDisable()
    {
        _bomb.OldEnough -= CreateExplosion;
    }

    private void CreateExplosion(Bomb bomb)
    {
        int hits = Physics.OverlapSphereNonAlloc(bomb.transform.position, _explosionRadius, _hitColliders);

        for (int i = 0; i < hits; i++)
        {
            if (_hitColliders[i].TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, bomb.transform.position, _explosionRadius);
            }
        }
    }
}
