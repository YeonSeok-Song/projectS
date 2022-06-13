using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // 플레이어와 길이로 체크하는 이벤트
    // 플레이어가 자기 자신위에 있으면 시작하는 이벤트
    protected GameObject _player;
    protected GameScene _scene;

    protected virtual void Init()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _scene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    protected virtual void UpdateController()
    {

    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    // 사물이 이벤트 오브젝트 안에 있을 때
    protected virtual void OnTriggerEnter2D(Collider2D other) { }

    // 사물이 이벤트 오브젝트와의 거리가 가까울때 
    protected virtual void OnRangeEnter() { }

    protected virtual void OnDrawGizmos() { }

    public bool IsRangedIn(GameObject go)
    {
        float dist = (go.transform.position - transform.position).magnitude;

        if (dist <= Define.EVENT_RANGE)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
