using UnityEngine;

using static BubbleInteractionSettings;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class Bubble : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;

    [SerializeField] private float _despawnTime;
    [SerializeField] private float _bounceForce;

    [SerializeField] private AudioClip _pop;

    private RigidbodyType2D _oldType;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, _despawnTime);
    }

    private void Update()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            t.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleInteractionSettings popSettings))
        {
            if (popSettings.Interaction.HasFlag(BubbleInteraction.TrapThis) && transform.childCount == 0)
            {
                transform.localScale = new(1.2f, 1.2f);

                collision.transform.SetParent(transform, false);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localScale = new(1 / 1.2f, 1 / 1.2f);

                _oldType = collision.attachedRigidbody.bodyType;
                collision.attachedRigidbody.bodyType = RigidbodyType2D.Static;
                collision.enabled = false;

                return;
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.BounceThis) && collision.attachedRigidbody.linearVelocityY < 0)
            {
                collision.attachedRigidbody.linearVelocityY = _bounceForce;
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.InteractThis) && collision.gameObject.TryGetComponent(out IOnInteract onInteract))
            {
                onInteract.Interact(null);
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.PopBubble))
            {
                _anim.SetTrigger("Pop");
                _rb.linearVelocity = Vector2.zero;
                AudioSource.PlayClipAtPoint(_pop, transform.position);

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
            col.isTrigger = false;
        }

        foreach (Throwable throwable in GetComponentsInChildren<Throwable>())
        {
            throwable.SetIsThrown(true);
        }

        transform.DetachChildren();
    }
}
