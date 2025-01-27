using UnityEngine;
using System.Collections.Generic;

public class SaveState : MonoBehaviour
{
    private static List<SaveState> _all = new();

    private bool _activeState;
    private Vector2 _position;

    private void Awake()
    {
        _all.Add(this);
    }

    public void Save()
    {
        _activeState = gameObject.activeSelf;
        _position = transform.position;
    }

    public static void SaveAll()
    {
        foreach (SaveState item in _all)
        {
            item.Save();
        }
    }

    public void Load()
    {
        gameObject.SetActive(_activeState);
        transform.position = _position;
    }

    public static void LoadAll()
    {
        foreach (SaveState item in _all)
        {
            item.Load();
        }
    }
}
