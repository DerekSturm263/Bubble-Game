using UnityEngine;
using UnityEngine.Tilemaps;

using static ExplosionInteractionSettings;

[RequireComponent(typeof(Throwable))]
public class Explodeable : MonoBehaviour, IOnToggle
{
    private Throwable _throwable;

    [SerializeField] private int _radius;
    [SerializeField] private GameObject _explosionParticles;
    [SerializeField] private GameObject _crumblingParticles;

    private void Awake()
    {
        _throwable = GetComponent<Throwable>();
    }

    public void ToggleOn()
    {
        Explode();
    }

    public void ToggleOff() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_throwable.IsThrown)
        {
            Explode();
        }
    }

    private void OnDestroy()
    {
        Explode();
    }

    public void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent(out ExplosionInteractionSettings bombSettings))
            {
                if (bombSettings.Interaction.HasFlag(ExplosionInteract.InteractThis) && hit.gameObject.TryGetComponent(out IOnInteract onInteract))
                {
                    onInteract.Interact(null);
                }

                if (bombSettings.Interaction.HasFlag(ExplosionInteract.DestroyTilesThis) && hit.gameObject.TryGetComponent(out Tilemap tilemap))
                {
                    int radius = _radius + _radius - 1;

                    Vector3Int[] positions = new Vector3Int[radius * radius];
                    TileBase[] tiles = new TileBase[positions.Length];

                    for (int i = 0; i < positions.Length; ++i)
                    {
                        (int x, int y) = (i % radius, i / radius);

                        Vector3 worldPos = new Vector3(x + (_radius / 2 - _radius), y + _radius / 2 - _radius) + transform.position;

                        positions[i] = tilemap.WorldToCell(worldPos);
                        tiles[i] = null;

                        if (tilemap.GetTile(positions[i]))
                            Instantiate(_crumblingParticles, worldPos, Quaternion.identity);
                    }

                    tilemap.SetTiles(positions, tiles);
                }

                if (bombSettings.Interaction.HasFlag(ExplosionInteract.DestroyThis))
                {
                    Destroy(hit.gameObject);
                }
            }
        }

        Instantiate(_explosionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
