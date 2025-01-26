using UnityEngine;

public class BubbleInteractionSettings : MonoBehaviour
{
    [System.Flags]
    public enum BubbleInteraction
    {
        PopBubble = 1 << 0,
        BounceThis = 1 << 1,
        TrapThis = 1 << 2,
        InteractThis = 1 << 3
    }

    [SerializeField] private BubbleInteraction _interaction;
    public BubbleInteraction Interaction => _interaction;
}
