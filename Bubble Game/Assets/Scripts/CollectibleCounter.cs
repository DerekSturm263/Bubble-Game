using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleCounter : MonoBehaviour
{
    [SerializeField] private GameObject[] _collectibles;

    [SerializeField] private string _format;
    [SerializeField] private UnityEvent<string> _onCollect;
    [SerializeField] private bool _onlyShowWhenStarted;

    [SerializeField] private UnityEvent<string> _onCollectAll;
    private bool _hasCollected;

    private void Update()
    {
        int count = _collectibles.Count(item => item == null);
        string message = !_onlyShowWhenStarted || count > 0 ? string.Format(_format, count, _collectibles.Length) : "";

        _onCollect.Invoke(message);

        if (!_hasCollected && count == _collectibles.Length)
        {
            _onCollectAll.Invoke(message);
            _hasCollected = true;
        }
    }
}
