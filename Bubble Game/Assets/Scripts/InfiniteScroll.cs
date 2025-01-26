using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private Bounds _bounds;
    [SerializeField] private Vector2 _velocity;

    private void Awake()
    {
        _velocity.x *= Random.Range(0.6666f, 1.5f);
    }

    private void Update()
    {
        transform.position += (Vector3)_velocity * Time.deltaTime;

        if (transform.position.x < _bounds.min.x)
            transform.position = new(_bounds.max.x, transform.position.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);
    }
}
