using System;
using UnityEngine;

[Serializable]
public struct BoxcastSettings
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _size;
    [SerializeField] private LayerMask _layer;

    public readonly RaycastHit2D GetHit(Transform parent, bool flipXOffset)
    {
        Vector2 offset = new(_offset.x * (flipXOffset ? -1 : 1), _offset.y);

        return Physics2D.BoxCast(parent.position + (Vector3)offset, _size, 0, Vector2.zero, 0, _layer);
    }

    public readonly void Draw(Transform parent, bool flipXOffset)
    {
        Vector2 offset = new(_offset.x * (flipXOffset ? -1 : 1), _offset.y);

        Gizmos.DrawWireCube(parent.position + (Vector3)offset, _size);
    }
}
