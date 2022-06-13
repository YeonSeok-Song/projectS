using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour
{
    GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        _player.GetComponent<PlayerController>().Attack();
    }

}
