using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIManager
{
    // ���⼭ ������ ���°��� ���޹����� �����ؼ�
    // ���Ͱ� ������ �ִ� �ൿ �� � �ൿ�� �ϸ� �Ǵ��� ���̵�? ������
    // �Ѱ��ش�. �װ� ������ ���ʹ� �� ���̵� ���� �´� �ൿ�� �ϰ� ��

    // todo: �ʿ��Ѱ�? �� ���ؼ� �ٽ� �����غ���
    MonsterTree MT;
    BossTree BT;

    public void Init()
    {
        MT = new MonsterTree();
        BT = new BossTree();
    }

    public void MonsterAct(GameObject go)
    {
        MT.CalcTree(go, go.GetComponent<MonsterController>().StateDict);
    }

    public void BossAct(GameObject go)
    {
        
    }

}
