using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    private void OnEnable()
    {
        _bombSpawner.Released += CreateExplosion;
    }

    private void OnDisable()
    {
        _bombSpawner.Released -= CreateExplosion;
    }

    private void CreateExplosion(Bomb bomb)
    {
        Collider[] hits = Physics.OverlapSphere(bomb.transform.position, _explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, bomb.transform.position, _explosionRadius);
            }
        }
    }
}
