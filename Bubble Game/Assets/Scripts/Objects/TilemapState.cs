using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct TilemapState
{
    [SerializeField] private LayerMask _layer;
    public readonly LayerMask Layer => _layer;

    [SerializeField] private TileBase _tile;
    public readonly TileBase Tile => _tile;
}
