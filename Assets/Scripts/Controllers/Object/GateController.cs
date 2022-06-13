using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : ObjectController
{
    int _moveMapId = 3;
    

    protected override void UpdateController()
    {
        base.UpdateController();
        OnRangeEnter();
    }

    protected override void Init()
    {
        base.Init();
        if (transform.gameObject.name.Contains("_"))
        {
            _moveMapId = Int32.Parse(transform.gameObject.name.Split('_')[2]);
        }

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.gameObject.CompareTag("Player"))
        {
            _scene.ChangeMap(_moveMapId);
        }
    }

    protected override void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, Define.EVENT_RADIUS);
    }

}
