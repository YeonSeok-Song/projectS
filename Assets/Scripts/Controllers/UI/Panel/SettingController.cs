using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : UIBaseController
{
    protected override void Init()
    {
        base.Init();
        _ui = transform;
        if (_ui == null)
        {
            Debug.Log("not find Setting");
        }
        _ui.gameObject.SetActive(false);
    }

    public void SettingToggle()
    {
        base.UIToggle();
    }
}
