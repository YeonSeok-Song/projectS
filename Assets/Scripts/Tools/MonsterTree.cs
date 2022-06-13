using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    
    Node Parent;
    Dictionary<Define.State, bool> executeCondition = new Dictionary<Define.State, bool>();
    Action<GameObject> action = null;    

    public List<Node> childs = new List<Node>();
    public Node(Node p, Dictionary<Define.State, bool> ex, Action<GameObject> ac = null)
    {
        this.Parent = p;
        this.executeCondition = ex;
        this.action = ac;
    }

    public void AddChild(Node node)
    {
        childs.Add(node);
    }

    public void execute(GameObject go)
    {
        action.Invoke(go);
    }

    public bool check(Dictionary<Define.State, bool> state)
    {
        // todo: 계산하는 로직 구현
        foreach (Define.State s in executeCondition.Keys)
        {
            if (executeCondition[s] == state[s])
            {
                return true;
            }
        }

        
        return false;
    }
}

public class MonsterTree
{
    public Node root = new Node(null, null, null);

    public MonsterTree()
    {

        #region Hostile

        #region 1계층
        root.AddChild(new Node(root, new Dictionary<Define.State, bool> {
            { Define.State.isHostile, true },
        }));
        #endregion

        #region 2계층
        root.childs[0].AddChild(new Node(root.childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isIdle, true },
        }, (go) =>
        {
            go.GetComponent<MonsterController>().StartPatrol();
        }
        ));
        root.childs[0].AddChild(new Node(root.childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isMove, true },
        }));
        root.childs[0].AddChild(new Node(root.childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isAttack, true },
        }));
        root.childs[0].AddChild(new Node(root.childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isDead, true },
        }));

        #endregion

        #region 3계층
        root.childs[0].childs[0].AddChild(new Node(root.childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, true },
        }, (go) => {
            go.GetComponent<MonsterController>().IdleToMove();
        }));
        root.childs[0].childs[0].AddChild(new Node(root.childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, false },
        }, (go) => {
            //Debug.Log("StartPatrol");
            go.GetComponent<MonsterController>().StartPatrol();
        }));

        root.childs[0].childs[1].AddChild(new Node(root.childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, true },
        }));
        root.childs[0].childs[1].AddChild(new Node(root.childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, false },
        }, (go) => {
            //Debug.Log("EndPatrol");
            go.GetComponent<MonsterController>().EndPatrol();
        }));

        root.childs[0].childs[2].AddChild(new Node(root.childs[0].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, true },
        }, (go) => {
            // 공격타입에 따라 발사
            go.GetComponent<MonsterController>().MonsterAttack(5);
            return;
        }));
        root.childs[0].childs[2].AddChild(new Node(root.childs[0].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, false },
        }, (go) => {

            // 그냥 평타
            return;
        }));

        root.childs[0].childs[3].AddChild(new Node(root.childs[0].childs[3], new Dictionary<Define.State, bool> {
            { Define.State.isDeadNormal, true },
        }, (go) => {
            go.GetComponent<MonsterController>().Dead();
            return;
        }));
        root.childs[0].childs[3].AddChild(new Node(root.childs[0].childs[3], new Dictionary<Define.State, bool> {
            { Define.State.isDeadExplode, true },
        }, (go) => {
            go.GetComponent<MonsterController>().ExplosionDead();
            return;
        }));
        root.childs[0].childs[3].AddChild(new Node(root.childs[0].childs[3], new Dictionary<Define.State, bool> {
            { Define.State.isDeadRevive, true },
        }, (go) => {
            go.GetComponent<MonsterController>().ReviveDead();
            return;
        }));


        #endregion

        #region 4계층
        root.childs[0].childs[0].childs[0].AddChild(new Node(root.childs[0].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isRangedIn, true },
        }, (go) => {
            go.GetComponent<MonsterController>().IdleToAttack();
        }));

        root.childs[0].childs[0].childs[0].AddChild(new Node(root.childs[0].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isRangedIn, false },
        }, (go) => {
            go.GetComponent<MonsterController>().IdleToMove();
        }));


        root.childs[0].childs[1].childs[0].AddChild(new Node(root.childs[0].childs[1].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isRangedIn, true },
        }, (go) => {
            go.GetComponent<MonsterController>().StateDict[Define.State.isMove] = false;
            go.GetComponent<MonsterController>().StateDict[Define.State.isAttack] = true;
        }));

        root.childs[0].childs[1].childs[0].AddChild(new Node(root.childs[0].childs[1].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isRangedIn, false },
        }, (go) =>
        {
            go.GetComponent<MonsterController>().MoveTarget();
            
        }));
        #endregion

        #endregion


        #region none hostile.

        #region 1계층
        root.AddChild(new Node(root, new Dictionary<Define.State, bool> {
            { Define.State.isHostile, false },
        }));

        #endregion

        #region 2계층

        root.childs[1].AddChild(new Node(root.childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isIdle, true },
        }));
        root.childs[1].AddChild(new Node(root.childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isMove, true },
        }));
        root.childs[1].AddChild(new Node(root.childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isAttack, true },
        }));
        root.childs[1].AddChild(new Node(root.childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDead, true },
        }, (go) => {

            // 죽음 타입에 따라 실행
            return;
        }));
        #endregion

        #region 3계층

        root.childs[1].childs[0].AddChild(new Node(root.childs[1].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, true },
        }));
        root.childs[1].childs[0].AddChild(new Node(root.childs[1].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, false },
        }));

        root.childs[1].childs[1].AddChild(new Node(root.childs[1].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, true },
        }));
        root.childs[1].childs[1].AddChild(new Node(root.childs[1].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isDetected, false },
        }, (go) => {
            // 아무것도 안함.
            return;

        }));

        root.childs[1].childs[2].AddChild(new Node(root.childs[1].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, true },
        }, (go) => {

            // 공격타입에 따라 발사
            return;
        }));
        root.childs[1].childs[2].AddChild(new Node(root.childs[1].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, false },
        }, (go) => {

            // 그냥 평타
            return;
        }));

        #endregion


        #region 4계층
        root.childs[1].childs[0].childs[0].AddChild(new Node(root.childs[1].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, true },
        },(go) => {
            // 움직인다.
            return;
        } ));
        root.childs[1].childs[0].childs[0].AddChild(new Node(root.childs[1].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, false },
        }, (go) => {
            // 바라본다
            return;
        }));

        root.childs[1].childs[0].childs[1].AddChild(new Node(root.childs[1].childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, true },
        }, (go) => {
            // 움직인다.
            return;
        }));
        root.childs[1].childs[0].childs[1].AddChild(new Node(root.childs[1].childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, false },
        }, (go) => {
            // 아무것도 하지 않는다.
            return;
        }));

        #endregion


        #endregion
 
    }

    public void CalcTree(GameObject go, Dictionary<Define.State, bool> state)
    {
        Node correctNode = null;

        // 포문으로 트리를 돌면서 계산.
        foreach (Node sib in root.childs)
        {
            // 조건이 맞으면 자식쪽을 한번씩 체크
            // 자식중에 조건에 맞는게 없으면 부모한테 간다.
            // 조건을 검사할때는 있는것만 검사한다.
            // 나머지는 상관없음
            // 맞으면 그거 실행.

            correctNode = FindCorrectNode(sib, state);
            if (correctNode != null)
            {
                break;
            }

            // 조건이 안맞으면 형제로 넘김
        }
        // 여기 커랜트 노드가 null이 된다.
        correctNode.execute(go);
    }

    public Node FindCorrectNode(Node cn, Dictionary<Define.State, bool> state)
    {
        if (cn.check(state) == false)
        {
            return null;  
        }

        //맞는데 자식이 있다?
        // 확인
        if (cn.childs.Count > 0)
        {
            foreach (Node i in cn.childs)
            {
                if (FindCorrectNode(i, state) == null)
                {
                    continue;
                }
                else
                {
                    return FindCorrectNode(i, state);
                }
            }
        }
        else
        {
            return cn;
        }

        return null;
    }
    
}

//else if (isHostile == false  && isDetected == true && isAttacked == false)
//{
//    // 플레이어를 바라본다.
//}

//else if (isIdle == true && isHostile == false && isDetected == false && isAttacked == false)
//{
//    // Idle
//}




// 먼저 특성에 대한 이프문을 돈다.

// 특성 안에서의 행동 이프문을 돈다.


//if (isHostile == true && isPossiveMove == true && isDetected == false)
//{
//    // 패트롤
//}

//else if (isHostile == true && isPossiveMove == true && isDetected == true)
//{
//    // 플레이어를 쫓아간다.
//}



//else if (isMoving == true && isIdle == true && isHostile == false && isPossiveMove == true && isDetected == true && isAttacked == true)
//{
//    // 플레이어를 쫓아간다.
//}



//else if (isRanged == true && isDetected == true && isPossiveMove == true && isRangeIn == true)
//{
//    // 움직이면서 곰격 (거리 유지) -> 거리는 유지하대 왔다 갔다하기
//}

//else if (isRanged == true && isDetected == true && isPossiveMove == false && isRangeIn == true)
//{
//    // 서서 공격
//}

//else if (isPossiveMove == false && isPossiveAttack == false )
//{
//    // 스턴
//}

//else if (isRanged == false && isDetected == true && isPossiveMove == true && isRangeIn == false)
//{
//    // 때리러 가까이 간다.
//}

//else if (isPossiveMove == false)
//{
//    // 속박
//}

//else if (isRange == false && isDetected == true && isPossiveMove == true && isRangeIn == true)
//{
//    // 서서 때린다.
//}