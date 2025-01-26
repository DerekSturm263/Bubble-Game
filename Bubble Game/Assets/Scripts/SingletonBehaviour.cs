using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;
    public static T Instance => s_instance;

    public virtual void Initialize()
    {
        s_instance = this as T;

        Application.quitting += Shutdown;
    }

    public virtual void Shutdown()
    {
        Application.quitting -= Shutdown;

        s_instance = null;
    }
}
