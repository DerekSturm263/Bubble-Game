using UnityEngine;

public class SpawnOnTimer : MonoBehaviour, IOnToggle
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _timerOffset;
    [SerializeField] private float _frequency;

    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private bool _isOn;

    [SerializeField] private int _maxSpawned;

    private int _currentSpawned;
    public int IncrementSpawned() => ++_currentSpawned;
    public int DecrementSpawned() => --_currentSpawned;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), _timerOffset, _frequency);
    }

    private void Spawn()
    {
        if (!_isOn || _currentSpawned >= _maxSpawned)
            return;

        GameObject spawned = Instantiate(_prefab, transform.position + (Vector3)_offset, Quaternion.identity);
        
        SpawnerTag tag = spawned.AddComponent<SpawnerTag>();
        tag.SetSpawner(this);
        
        if (spawned.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = _velocity;
        }
    }

    [ContextMenu("Toggle On")]
    public void ToggleOn()
    {
        _isOn = !_isOn;
    }

    [ContextMenu("Toggle Off")]
    public void ToggleOff()
    {
        _isOn = !_isOn;
    }
}
