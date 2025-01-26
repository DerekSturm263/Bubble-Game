using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : Activator
{
    [ContextMenu("Toggle On")]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ToggleAllOn();
    }
    
    [ContextMenu("Toggle Off")]
    private void OnCollisionExit2D(Collision2D collision)
    {
        ToggleAllOff();
    }
}
