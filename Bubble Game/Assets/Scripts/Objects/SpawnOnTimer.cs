using UnityEngine;

public class SpawnOnTimer : MonoBehaviour, IOnToggle
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _frequency;

    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private bool _isOn;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), _frequency, _frequency);
    }

    private void Spawn()
    {
        if (!_isOn)
            return;

        GameObject spawned = Instantiate(_prefab, transform.position + (Vector3)_offset, Quaternion.identity);
        
        if (spawned.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = _velocity;
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
