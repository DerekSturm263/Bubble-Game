using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : Activator
{
    [SerializeField] private float _activationTime;

    [ContextMenu("Press")]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToggleAllOn();

        Invoke(nameof(ToggleAllOff), _activationTime);
    }
}
