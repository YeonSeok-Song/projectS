using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBaseController : MonoBehaviour
{
    protected Transform _ui;
    //public bool Pause { get; private set; } = false;
    public bool Status { get; protected set; } = false;

    protected virtual void Init()
    {
    }

    protected virtual void UpdateController()
    {

    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    public virtual void UIToggle()
    {
        if (Status == true)
        {
            Time.timeScale = 1;
            _ui.gameObject.SetActive(false);
            Status = false;
            
            //Pause = false;
        }
        else
        {
            Time.timeScale = 0;
            _ui.gameObject.SetActive(true);
            Status = true;
            
            //Pause = true;

        }
    }
}
