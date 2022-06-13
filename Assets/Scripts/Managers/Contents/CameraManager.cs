using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    public void CreateCamera(string cName)
    {
        GameObject go = Managers.Resource.Instantiate($"Camera/{cName}");

        if (go != null)
        {
            go.name = $"{cName}";
        }
    }

    public void SwitchCamera(string cName, bool on)
    {
        GameObject co = GameObject.Find(cName);
        if (co == null) return;

        if (on == true)
        {
            Camera.main.enabled = false;
            Camera coCamera = co.GetComponent<Camera>();
            coCamera.enabled = true;
        }
        else
        {
            Camera.main.enabled = true;
            Camera coCamera = co.GetComponent<Camera>();
            coCamera.enabled = false;
        }
    }

    public GameObject[] FindCamera()
    {
        return GameObject.FindGameObjectsWithTag("Camera");
    }

    
}
