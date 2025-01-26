using UnityEngine;

using static BubbleInteractionSettings;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class Bubble : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private float _despawnTime;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        Destroy(gameObject, _despawnTime);
    }

    private void Update()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            t.position = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleInteractionSettings popSettings))
        {
            if (popSettings.Interaction.HasFlag(BubbleInteraction.TrapThis) && transform.childCount == 0)
            {
                collision.transform.SetParent(transform, false);
                collision.transform.localPosition = Vector3.zero;

                collision.rigidbody.bodyType = RigidbodyType2D.Static;
                collision.collider.enabled = false;

                transform.localScale = new(1.15f, 1.15f);
                return;
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.BounceThis) && collision.rigidbody.linearVelocityY < 0)
            {
                collision.rigidbody.linearVelocityY *= -3;
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.InteractThis) && collision.gameObject.TryGetComponent(out IOnInteract onInteract))
            {
                onInteract.Interact(null);
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.PopBubble))
            {
                _anim.SetTrigger("Pop");

                Destroy(gameObject, 0.25f);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = true;
        }

        foreach (Throwable throwable in GetComponentsInChildren<Throwable>())
        {
            throwable.SetIsThrown(true);
        }

        transform.DetachChildren();
    }
}
