using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _numberOfColliders;

    private Collider[] _hitColliders;

    private void Awake()
    {
        _hitColliders = new Collider[_numberOfColliders];
    }

    public void CreateExplosion(Bomb bomb)
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
