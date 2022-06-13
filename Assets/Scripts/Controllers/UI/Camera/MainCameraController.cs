using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : CameraController
{
    GameObject _myPlayer;
    Vector3 cameraPosition = new Vector3(0, 0, -10);
    private float _cameraSpeed = 5;
    
    protected override void SetSize()
    {
        base.SetSize();
        if (Camera.main.gameObject == null)
        {
            return;
        }
        
        base.Vertical = Camera.main.orthographicSize;
        base.Horizontal = Camera.main.orthographicSize * Screen.width / Screen.height;

    }

    protected override void Init()
    {
        base.Init();
        _myPlayer = GameObject.FindGameObjectWithTag("Player");
        SetSize();
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        if (_myPlayer == null) return;

        FocusObjectByPos(DontOverMap(_myPlayer.transform.position), true);
    }

    protected void FocusObjectByPos(Vector3 pos, bool slerp)
    {
        if (slerp)
        {
            transform.position = Vector3.Slerp(transform.position, pos + cameraPosition, _cameraSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = cameraPosition + pos;
        }
        
    }
}
