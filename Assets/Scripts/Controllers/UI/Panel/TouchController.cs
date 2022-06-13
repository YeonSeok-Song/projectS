using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    public GameObject _joyStick;

    [SerializeField]
    public GameObject _moveStick;

    public float Width { get; private set; }
    public float height { get; private set; }

    public float CenterX { get; private set; }
    public float CenterY { get; private set; }

    public float PointX { get; private set; }
    public float PointY { get; private set; }

    GameObject _player;
    RectTransform _rt;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rt = transform.gameObject.GetComponent<RectTransform>();

        float padInX = (-1 * (_rt.rect.xMin));
        CenterX = (padInX + (padInX + _rt.rect.width)) / 2;
        CenterY = (_rt.rect.height) / 2;
    }
    Vector3 GetPoint(float x, float y)
    {
        return new Vector3(x - CenterX, y + _rt.rect.yMin, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 PointCenter = GetPoint(eventData.position.x, eventData.position.y);

        _joyStick.gameObject.SetActive(true);
        _moveStick.gameObject.SetActive(true);
        _joyStick.transform.localPosition = PointCenter;
        _moveStick.transform.localPosition = PointCenter;
        

        PointX = PointCenter.x;
        PointY = PointCenter.y;

        //Debug.Log($"clicked : {PointX}, {PointY}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform background = _joyStick.GetComponent<RectTransform>();
        Image stick = _moveStick.GetComponent<Image>();

        Vector3 pointer = (new Vector3(PointX, PointY, 0) - GetPoint(eventData.position.x, eventData.position.y));

        
        pointer.x = pointer.x / background.sizeDelta.x;
        pointer.y = pointer.y / background.sizeDelta.y;

        pointer = pointer.magnitude > 1 ? pointer.normalized : pointer;

        _moveStick.transform.localPosition = new Vector3(PointX - (pointer.x * background.rect.width / 3), PointY - (pointer.y * background.rect.height / 3), 0);

        // Debug.Log(pointer);

        // 더치패트의 pivot이 중앙으로 잡아서 방향이 전부 반대
        // 방향 백터의 왼쪽이 + 오른쪽이 -, 위쪽 - 아래쪽 +
        // 캐릭터 이동 시 방향 백터에 -1을 곱해줘야 정상적으로 움직임.
        _player.GetComponent<PlayerController>().GetMoveInput(pointer);
    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Touchend : " + eventData);
        _joyStick.transform.localPosition = GetPoint(CenterX - (_rt.rect.width / 2), CenterY);
        _moveStick.transform.localPosition = GetPoint(CenterX - (_rt.rect.width / 2), CenterY);
        _joyStick.gameObject.SetActive(false);
        _moveStick.gameObject.SetActive(false);
        PointX = 0.0f;
        PointY = 0.0f;

        _player.GetComponent<PlayerController>().GetMoveInput(Vector3.zero);
    }
}
