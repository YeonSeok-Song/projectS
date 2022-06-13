using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIManager
{
    // 여기서 몬스터의 상태값을 전달받으면 연산해서
    // 몬스터가 가지고 있는 행동 중 어떤 행동을 하면 되는지 아이디? 값으로
    // 넘겨준다. 그걸 받으면 몬스터는 그 아이디 값에 맞는 행동을 하게 됨

    // todo: 필요한가? 에 대해선 다시 생각해보자
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
