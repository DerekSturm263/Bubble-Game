using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Refill : MonoBehaviour
{
    private SpriteRenderer _rndr;

    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;

    [SerializeField] private float _reactivateTime;

    private bool _isActive = true;

    private void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive)
            return;

        if (collision.TryGetComponent(out PlayerMovement player))
        {
            player.SetCurrentBubbleCount(1);
            _rndr.sprite = _inactiveSprite;

            Invoke(nameof(Reactivate), _reactivateTime);
        }
    }

    private void Reactivate()
    {
        _isActive = true;
        _rndr.sprite = _activeSprite;
    }
}
