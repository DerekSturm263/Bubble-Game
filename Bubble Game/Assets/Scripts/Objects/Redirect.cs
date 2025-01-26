using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Redirect : MonoBehaviour
{
    private BoxCollider2D _col;

    [SerializeField] private Vector2 _newDirection;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            collision.transform.position = transform.position + (Vector3)(_newDirection.normalized * _col.size);

            rb.linearVelocity = _newDirection;
        }
    }
}
