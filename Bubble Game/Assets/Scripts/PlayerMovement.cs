using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(SpriteRenderer), typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer _sprt;
    private Rigidbody2D _rb;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private Vector2 _boxcastOffset;
    [SerializeField] private Vector2 _boxcastSize;
    [SerializeField] private LayerMask _boxcastLayer;

    private bool _isGrounded;

    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private int _maxBubbleCount;
    [SerializeField] private Vector2 _bubbleOffset;
    [SerializeField] private float _bubbleSpeed;
    private int _currentBubbleCount;

    private void Awake()
    {
        _sprt = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.BoxCast(transform.position + (Vector3)_boxcastOffset, _boxcastSize, 0, Vector2.zero, 0, _boxcastLayer);
    
        if (_isGrounded)
        {
            _currentBubbleCount = _maxBubbleCount;
        }

        _sprt.color = _currentBubbleCount > 0 ? Color.blue : Color.red;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)_boxcastOffset, _boxcastSize);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        float movement = ctx.ReadValue<Vector2>().x;

        _rb.linearVelocityX = movement * _movementSpeed;

        if (movement < 0)
            _sprt.flipX = true;
        else if (movement > 0)
            _sprt.flipX = false;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (!_isGrounded)
            return;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void Bubble(InputAction.CallbackContext ctx)
    {
        if (_currentBubbleCount == 0)
            return;

        --_currentBubbleCount;

        Vector2 bubbleOffset = _bubbleOffset;
        if (_sprt.flipX)
            bubbleOffset.x *= -1;
        
        GameObject bubble = Instantiate(_bubblePrefab, transform.position + (Vector3)bubbleOffset, Quaternion.identity);
        bubble.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(_bubbleSpeed * (_sprt.flipX ? -1 : 1), 0);
    }
}
