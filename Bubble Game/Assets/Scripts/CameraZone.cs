using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[ExecuteAlways]
public class CameraZone : MonoBehaviour
{
    private static CameraZone _current;
    public static CameraZone Current => _current;

    private BoxCollider2D _col;

    [SerializeField] private Vector2 _respawnPoint;
    public Vector2 RespawnPoint => _respawnPoint;

    private CinemachineConfiner2D _confiner;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
        _confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _confiner.BoundingShape2D = _col;
            _current = this;

            SaveState.SaveAll();
        }
    }

    [ContextMenu("Teleport Here")]
    public void Teleport()
    {
        FindFirstObjectByType<PlayerMovement>().transform.position = _respawnPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)_col.offset, _col.size);
        Gizmos.DrawCube(_respawnPoint, Vector3.one);
    }
}
