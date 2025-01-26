using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Switch : Activator, IOnInteract
{
    private bool _currentState;

    [ContextMenu("Toggle")]
    public void Interact(PlayerMovement player)
    {
        _currentState = !_currentState;

        if (_currentState)
            ToggleAllOn();
        else
            ToggleAllOff();
    }
}
