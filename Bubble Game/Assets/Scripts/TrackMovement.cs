using UnityEngine;

public class TrackMovement : MonoBehaviour, IOnToggle
{
    public enum InterpolationType
    {
        Linear,
        Spherical
    }

    public enum EndType
    {
        WrapAround,
        TurnAround,
        LoopAround
    }

    [SerializeField] private Vector3[] _positions;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _closenessCheck;
    [SerializeField] private EndType _end;
    [SerializeField] private bool _isOn;

    [SerializeField] private InterpolationType _interpolation;

    private float _timeAtIndex;
    private int _direction = 1;

    private int _currentIndex;
    private Vector3 Current => _positions[_currentIndex % _positions.Length];

    private int _nextIndex = 1;
    private Vector3 Next => _positions[_nextIndex % _positions.Length];

    void Update()
    {
        if (!_isOn)
            return;

        if (_interpolation == InterpolationType.Linear)
            transform.position = Vector3.Lerp(Current, Next, _timeAtIndex);
        else if (_interpolation == InterpolationType.Spherical)
            transform.position = Vector3.Slerp(Current, Next, _timeAtIndex);

        float speedMultiplier = Vector3.Distance(Next, Current);
        _timeAtIndex += (Time.deltaTime * _movementSpeed) / speedMultiplier;

        if (Vector3.Distance(transform.position, Next) < _closenessCheck)
        {
            _currentIndex += _direction;
            _timeAtIndex = 0;

            _nextIndex = _currentIndex + _direction;

            // At the end of the path...
            if (_currentIndex >= _positions.Length - 1 || _currentIndex <= 0)
            {
                if (_end == EndType.WrapAround)
                {
                    _currentIndex = 0;
                    _nextIndex = _currentIndex + _direction;
                }
                else if (_end == EndType.TurnAround)
                {
                    _direction *= -1;
                    _nextIndex = _currentIndex + _direction;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb) && rb.bodyType != RigidbodyType2D.Static)
            collision.transform.SetParent(transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb) && rb.bodyType != RigidbodyType2D.Static)
            collision.transform.SetParent(null, true);
    }

    public void ToggleOn()
    {
        _direction *= -1;
        _nextIndex = _currentIndex + _direction;
    }

    public void ToggleOff()
    {
        _direction *= -1;
        _nextIndex = _currentIndex + _direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLineList(_positions);
    }
}
