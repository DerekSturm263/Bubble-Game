using UnityEngine;

using static BubbleInteractionSettings;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Bubble : MonoBehaviour
{
    [SerializeField] private float _despawnTime;

    private void Awake()
    {
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
            if (popSettings.Interaction.HasFlag(BubbleInteraction.PopOnTouch))
            {
                Destroy(gameObject);
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.BounceOnTouch) && collision.rigidbody.linearVelocityY < 0)
            {
                collision.rigidbody.linearVelocityY *= -3;
            }

            if (popSettings.Interaction.HasFlag(BubbleInteraction.TrapOnTouch))
            {
                collision.transform.SetParent(transform, false);
                collision.transform.localPosition = Vector3.zero;

                collision.rigidbody.bodyType = RigidbodyType2D.Static;
                collision.collider.enabled = false;

                transform.localScale = new(1.15f, 1.15f);
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

        transform.DetachChildren();
    }
}
