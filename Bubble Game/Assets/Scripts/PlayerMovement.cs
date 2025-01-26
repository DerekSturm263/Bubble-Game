using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using static PlayerInteractionSettings;

[RequireComponent(typeof(PlayerInput), typeof(SpriteRenderer), typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public partial class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer _rndr;
    private Rigidbody2D _rb;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private BoxcastSettings _groundedCast;
    [SerializeField] private BoxcastSettings _interactCast;

    private bool _isGrounded;

    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _maxBubbleCount;
    [SerializeField] private Vector2 _bubbleOffset;
    [SerializeField] private float _bubbleSpeed;

    private int _currentBubbleCount;
    public void SetCurrentBubbleCount(int currentBubbleCount) => _currentBubbleCount = currentBubbleCount;

    [SerializeField] private Vector2 _holdingOffset;
    public Vector2 HoldingOffset
    {
        get
        {
            Vector2 offset = new(_holdingOffset.x * (_rndr.flipX ? -1 : 1), _holdingOffset.y);
            return offset;
        }
    }

    private GameObject _currentlyHeld;
    public GameObject CurrentlyHeld => _currentlyHeld;

    [SerializeField] private Vector2 _throwForce;

    private float _moveAmount;

    private void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isGrounded = _groundedCast.GetHit(transform, false);
    
        if (_isGrounded)
        {
            _currentBubbleCount = _maxBubbleCount;
        }

        _rndr.color = _currentBubbleCount > 0 ? Color.blue : Color.red;

        if (_currentlyHeld)
        {
            _currentlyHeld.transform.localPosition = HoldingOffset;
        }

        _rb.linearVelocityX = _moveAmount * _movementSpeed;

        if (_moveAmount < 0)
            _rndr.flipX = true;
        else if (_moveAmount > 0)
            _rndr.flipX = false;
    }

    private void OnDrawGizmos()
    {
        _groundedCast.Draw(transform, _rndr ? _rndr.flipX : false);
        _interactCast.Draw(transform, _rndr ? _rndr.flipX : false);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _moveAmount = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (!_isGrounded || ctx.ReadValue<float>() == 0)
            return;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void Bubble(InputAction.CallbackContext ctx)
    {
        if (_currentBubbleCount == 0 || ctx.ReadValue<float>() == 0)
            return;

        --_currentBubbleCount;

        Vector2 bubbleOffset = _bubbleOffset;
        if (_rndr.flipX)
            bubbleOffset.x *= -1;
        
        GameObject bubble = Instantiate(_bubblePrefab, transform.position + (Vector3)bubbleOffset, Quaternion.identity);
        bubble.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(_bubbleSpeed * (_rndr.flipX ? -1 : 1), 0);
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0)
            return;

        if (_currentlyHeld && _currentlyHeld.TryGetComponent(out IOnInteract onInteract))
        {
            onInteract.Interact(this);
        }
        else
        {
            RaycastHit2D hit = _interactCast.GetHit(transform, _rndr.flipX);
            
            if (hit && hit.transform.TryGetComponent(out IOnInteract onInteract2))
            {
                onInteract2.Interact(this);
            }
        }
    }

    public void Grab(GameObject gameObject)
    {
        gameObject.transform.SetParent(transform, false);

        if (gameObject.TryGetComponent(out Collider2D col))
            col.enabled = false;

        if (gameObject.TryGetComponent(out Rigidbody2D rb))
            rb.bodyType = RigidbodyType2D.Static;

        _currentlyHeld = gameObject;
    }

    public void Drop(GameObject gameObject)
    {
        gameObject.transform.SetParent(null, true);

        if (gameObject.TryGetComponent(out Collider2D col))
            col.enabled = true;

        if (gameObject.TryGetComponent(out Rigidbody2D rb))
            rb.bodyType = RigidbodyType2D.Dynamic;

        _currentlyHeld = null;
    }

    public void Throw(GameObject gameObject)
    {
        Drop(gameObject);

        if (gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 throwForce = new(_throwForce.x * (_rndr.flipX ? -1 : 1), _throwForce.y);

            rb.linearVelocity = throwForce;
        }

        if (gameObject.TryGetComponent(out BoxCollider2D col))
        {
            col.enabled = false;
            StartCoroutine(ResetCollider(col));
        }
    }

    private IEnumerator ResetCollider(BoxCollider2D col)
    {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractionSettings playerSettings))
        {
            if (playerSettings.Interaction.HasFlag(PlayerInteract.DestroyPlayer))
            {
                Die();
            }
            
            if (playerSettings.Interaction.HasFlag(PlayerInteract.DestroyThis))
            {
                Destroy(collision.gameObject);
            }

            if (playerSettings.Interaction.HasFlag(PlayerInteract.InteractThis) && collision.gameObject.TryGetComponent(out IOnInteract onInteract))
            {
                onInteract.Interact(this);
            }
        }
    }

    public void Die()
    {
        transform.position = CameraZone.Current.RespawnPoint;
    }
}
