using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneController : SingletonBehaviour<SceneController>
{
    private SceneLoadSettings _last;
    private bool _isTransitioning;

    private GameObject _transitionCanvas;
    private GameObject _transitionInstance;

    public void Load(SceneLoadSettings settings)
    {
        if (_isTransitioning)
            return;

        if (settings.Transition)
        {
            /*foreach (InputEvent inputEvent in FindFirstObjectByType<InputEvent>())
            {
                inputEvent.enabled = false;
            }*/

            EventSystem.current.enabled = false;

            StartCoroutine(LoadWithTransition(settings));
        }
        else
        {
            LoadNoTransition(settings);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLast(SceneLoadSettings fallback) => Load(_last ? fallback : _last);

    private void StartTransition(SceneLoadSettings settings)
    {
        if (!_transitionCanvas)
            _transitionCanvas = GameObject.FindGameObjectWithTag("Transition Canvas");

        _transitionInstance = Instantiate(settings.Transition, _transitionCanvas.transform);
        _isTransitioning = true;
    }

    private IEnumerator LoadWithTransition(SceneLoadSettings settings)
    {
        StartTransition(settings);

        AsyncOperation operation = SceneManager.LoadSceneAsync(settings.Scene.Name, settings.LoadParameters);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => operation.progress >= 0.9f);

        operation.allowSceneActivation = true;

        StopTransition(settings);
    }

    private void StopTransition(SceneLoadSettings settings)
    {
        if (_transitionInstance)
            _transitionInstance.GetComponent<Animator>().SetTrigger("Finished");

        _isTransitioning = false;
    }

    private void LoadNoTransition(SceneLoadSettings settings)
    {
        SceneManager.LoadScene(settings.Scene.Name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
