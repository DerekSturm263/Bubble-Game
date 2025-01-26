using UnityEngine;

public class PlayerInteractionSettings : MonoBehaviour
{
    [System.Flags]
    public enum PlayerInteract
    {
        DestroyThis = 1 << 0,
        InteractThis = 1 << 1,
        DestroyPlayer = 1 << 2
    }

    [SerializeField] private PlayerInteract _interaction;
    public PlayerInteract Interaction => _interaction;
}
