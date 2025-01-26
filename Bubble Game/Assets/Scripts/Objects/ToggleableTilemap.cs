using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public partial class ToggleableTilemap : MonoBehaviour, IOnToggle
{
    private Tilemap _tilemap;

    [SerializeField] private TilemapState _state1;
    [SerializeField] private TilemapState _state2;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        ToggleState1();
    }

    [ContextMenu("Toggle On")]
    public void ToggleOn()
    {
        ToggleState2();
    }

    [ContextMenu("Toggle Off")]
    public void ToggleOff()
    {
        ToggleState1();
    }

    public void ToggleState1()
    {
        gameObject.layer = (int)Mathf.Log(_state1.Layer, 2);
        UpdateAllTiles(_state1.Tile);
    }

    public void ToggleState2()
    {
        gameObject.layer = (int)Mathf.Log(_state2.Layer, 2);
        UpdateAllTiles(_state2.Tile);
    }

    public void UpdateAllTiles(TileBase newTile)
    {
        for (int y = _tilemap.cellBounds.min.y; y < _tilemap.cellBounds.max.y; ++y)
        {
            for (int x = _tilemap.cellBounds.min.x; x < _tilemap.cellBounds.max.x; ++x)
            {
                Vector3Int position = new(x, y);

                if (!_tilemap.HasTile(position))
                    continue;

                _tilemap.SetTile(position, newTile);
                _tilemap.RefreshTile(position);
            }
        }
    }
}
