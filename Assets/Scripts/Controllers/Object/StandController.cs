using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandController : ObjectController
{
    protected override void Init()
    {

    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void OnRangeEnter()
    {
        base.OnRangeEnter();

        Debug.Log("�����ϱ� Ŭ�� �� ���� ����.");

    }

}
