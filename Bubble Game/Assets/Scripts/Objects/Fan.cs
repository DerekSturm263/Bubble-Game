using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class Fan : MonoBehaviour, IOnToggle
{
    private ParticleSystem _particles;
    private BoxCollider2D _col;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _redirectSpeed;

    [SerializeField] private bool _isOn;

    private List<Rigidbody2D> _rbs = new();

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _col = GetComponent<BoxCollider2D>();

        ParticleSystem.ShapeModule shape = _particles.shape;

        shape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
        shape.scale = new(_col.size.x / 2, _col.size.y / 2, 1);
        shape.position = _col.offset + new Vector2(0, _col.size.y / -2);

        if (!_isOn)
            _particles.Stop();
        else
            _particles.Play();
    }

    private void Update()
    {
        if (!_isOn)
            return;

        foreach (Rigidbody2D rb in _rbs)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, _direction, Time.deltaTime * _redirectSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _rbs.Add(collision.attachedRigidbody);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _rbs.Remove(collision.attachedRigidbody);
    }

    [ContextMenu("Toggle On")]
    public void ToggleOn()
    {
        _isOn = !_isOn;

        if (!_isOn)
            _particles.Stop();
        else
            _particles.Play();
    }

    [ContextMenu("Toggle Off")]
    public void ToggleOff()
    {
        _isOn = !_isOn;

        if (!_isOn)
            _particles.Stop();
        else
            _particles.Play();
    }
}
