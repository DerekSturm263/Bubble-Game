using UnityEngine;

public class ConveyorBelt : MonoBehaviour, IOnToggle
{
    [SerializeField] private float _speed;

    private int _direction;

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    [ContextMenu("Toggle On")]
    public void ToggleOn()
    {
        _direction *= -1;
    }

    [ContextMenu("Toggle Off")]
    public void ToggleOff()
    {
        _direction *= -1;
    }
}
