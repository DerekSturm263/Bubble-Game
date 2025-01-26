using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pulse : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Color _color;
    [SerializeField] private float _speed;

    private Light2D _light;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        _light.color = new(_color.r, _color.g, _color.b, _curve.Evaluate(Mathf.Repeat(Time.time * _speed, 1)));
    }
}
