using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceTextController : MonoBehaviour
{
    GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = _player.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
    }
}
