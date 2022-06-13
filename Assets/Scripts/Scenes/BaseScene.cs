using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    protected virtual void Update() { }

    protected virtual void Start() { }

    public abstract void Clear();
}
