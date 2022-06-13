using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public string _nextSceneName;

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        _nextSceneName = GetSceneName(type);
        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));

    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
