using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Scene Load Settings", menuName = "Custom/Scene Load Settings")]
public class SceneLoadSettings : ScriptableObject
{
    [SerializeField] private SceneField _scene;
    public SceneField Scene => _scene;

    [SerializeField] private GameObject _transition;
    public GameObject Transition => _transition;

    [SerializeField] private LoadSceneParameters _loadParameters;
    public LoadSceneParameters LoadParameters => _loadParameters;
}
