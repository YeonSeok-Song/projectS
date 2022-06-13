using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : UIBaseController
{
    protected override void Init()
    {
        base.Init();
        _ui = transform;
        if (_ui == null)
        {
            Debug.Log("not find Inven");
        }
        _ui.gameObject.SetActive(false);
    }

    public override void UIToggle()
    {
        base.UIToggle();
    }
}
