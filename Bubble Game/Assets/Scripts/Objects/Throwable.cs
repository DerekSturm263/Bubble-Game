using UnityEngine;

public class Throwable : MonoBehaviour, IOnInteract
{
    private bool _isThrown;
    public bool IsThrown => _isThrown;
    public void SetIsThrown(bool isThrown) => _isThrown = isThrown;

    public void Interact(PlayerMovement player)
    {
        if (!player)
            return;

        if (player.CurrentlyHeld == gameObject)
        {
            player.DropOrThrow(gameObject);
            _isThrown = true;
        }
        else
        {
            player.Grab(gameObject);
        }
    }
}
