using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // �÷��̾�� ���̷� üũ�ϴ� �̺�Ʈ
    // �÷��̾ �ڱ� �ڽ����� ������ �����ϴ� �̺�Ʈ
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

    // �繰�� �̺�Ʈ ������Ʈ �ȿ� ���� ��
    protected virtual void OnTriggerEnter2D(Collider2D other) { }

    // �繰�� �̺�Ʈ ������Ʈ���� �Ÿ��� ����ﶧ 
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
