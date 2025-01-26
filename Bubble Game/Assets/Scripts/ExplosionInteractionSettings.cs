using UnityEngine;

public class ExplosionInteractionSettings : MonoBehaviour
{
    [System.Flags]
    public enum ExplosionInteract
    {
        DestroyThis = 1 << 0,
        InteractThis = 1 << 1,
        DestroyTilesThis = 1 << 2
    }

    [SerializeField] private ExplosionInteract _interaction;
    public ExplosionInteract Interaction => _interaction;
}
