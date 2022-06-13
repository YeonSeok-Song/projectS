using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    void Start()
    {
        Canvas cv = transform.gameObject.GetComponent<Canvas>();
        cv.renderMode = RenderMode.ScreenSpaceCamera;
        cv.worldCamera = Camera.main;
    }

}
