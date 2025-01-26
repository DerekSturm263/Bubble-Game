using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Activator : MonoBehaviour
{
    private SpriteRenderer _rndr;

    [SerializeField] private GameObject[] _targets;

    [SerializeField] private Sprite _onState;
    [SerializeField] private Sprite _offState;

    private void Awake()
    {
        _rndr = GetComponent<SpriteRenderer>();
    }

    public void ToggleAllOn()
    {
        foreach (GameObject target in _targets)
        {
            foreach (IOnToggle toggle in target.GetComponents<IOnToggle>())
            {
                toggle.ToggleOn();
            }
        }

        _rndr.sprite = _onState;
    }

    public void ToggleAllOff()
    {
        foreach (GameObject target in _targets)
        {
            foreach (IOnToggle toggle in target.GetComponents<IOnToggle>())
            {
                toggle.ToggleOff();
            }
        }

        _rndr.sprite = _offState;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _targets.Length; ++i)
        {
            Gizmos.DrawLine(transform.position, _targets[i].transform.position);
        }
    }
}
