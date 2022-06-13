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
        // todo: ����ϴ� ���� ����
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

        #region 1����
        root.AddChild(new Node(root, new Dictionary<Define.State, bool> {
            { Define.State.isHostile, true },
        }));
        #endregion

        #region 2����
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

        #region 3����
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
            // ����Ÿ�Կ� ���� �߻�
            go.GetComponent<MonsterController>().MonsterAttack(5);
            return;
        }));
        root.childs[0].childs[2].AddChild(new Node(root.childs[0].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, false },
        }, (go) => {

            // �׳� ��Ÿ
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

        #region 4����
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

        #region 1����
        root.AddChild(new Node(root, new Dictionary<Define.State, bool> {
            { Define.State.isHostile, false },
        }));

        #endregion

        #region 2����

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

            // ���� Ÿ�Կ� ���� ����
            return;
        }));
        #endregion

        #region 3����

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
            // �ƹ��͵� ����.
            return;

        }));

        root.childs[1].childs[2].AddChild(new Node(root.childs[1].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, true },
        }, (go) => {

            // ����Ÿ�Կ� ���� �߻�
            return;
        }));
        root.childs[1].childs[2].AddChild(new Node(root.childs[1].childs[2], new Dictionary<Define.State, bool> {
            { Define.State.isRangedAttack, false },
        }, (go) => {

            // �׳� ��Ÿ
            return;
        }));

        #endregion


        #region 4����
        root.childs[1].childs[0].childs[0].AddChild(new Node(root.childs[1].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, true },
        },(go) => {
            // �����δ�.
            return;
        } ));
        root.childs[1].childs[0].childs[0].AddChild(new Node(root.childs[1].childs[0].childs[0], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, false },
        }, (go) => {
            // �ٶ󺻴�
            return;
        }));

        root.childs[1].childs[0].childs[1].AddChild(new Node(root.childs[1].childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, true },
        }, (go) => {
            // �����δ�.
            return;
        }));
        root.childs[1].childs[0].childs[1].AddChild(new Node(root.childs[1].childs[0].childs[1], new Dictionary<Define.State, bool> {
            { Define.State.isAttacked, false },
        }, (go) => {
            // �ƹ��͵� ���� �ʴ´�.
            return;
        }));

        #endregion


        #endregion
 
    }

    public void CalcTree(GameObject go, Dictionary<Define.State, bool> state)
    {
        Node correctNode = null;

        // �������� Ʈ���� ���鼭 ���.
        foreach (Node sib in root.childs)
        {
            // ������ ������ �ڽ����� �ѹ��� üũ
            // �ڽ��߿� ���ǿ� �´°� ������ �θ����� ����.
            // ������ �˻��Ҷ��� �ִ°͸� �˻��Ѵ�.
            // �������� �������
            // ������ �װ� ����.

            correctNode = FindCorrectNode(sib, state);
            if (correctNode != null)
            {
                break;
            }

            // ������ �ȸ����� ������ �ѱ�
        }
        // ���� Ŀ��Ʈ ��尡 null�� �ȴ�.
        correctNode.execute(go);
    }

    public Node FindCorrectNode(Node cn, Dictionary<Define.State, bool> state)
    {
        if (cn.check(state) == false)
        {
            return null;  
        }

        //�´µ� �ڽ��� �ִ�?
        // Ȯ��
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
//    // �÷��̾ �ٶ󺻴�.
//}

//else if (isIdle == true && isHostile == false && isDetected == false && isAttacked == false)
//{
//    // Idle
//}




// ���� Ư���� ���� �������� ����.

// Ư�� �ȿ����� �ൿ �������� ����.


//if (isHostile == true && isPossiveMove == true && isDetected == false)
//{
//    // ��Ʈ��
//}

//else if (isHostile == true && isPossiveMove == true && isDetected == true)
//{
//    // �÷��̾ �Ѿư���.
//}



//else if (isMoving == true && isIdle == true && isHostile == false && isPossiveMove == true && isDetected == true && isAttacked == true)
//{
//    // �÷��̾ �Ѿư���.
//}



//else if (isRanged == true && isDetected == true && isPossiveMove == true && isRangeIn == true)
//{
//    // �����̸鼭 ���� (�Ÿ� ����) -> �Ÿ��� �����ϴ� �Դ� �����ϱ�
//}

//else if (isRanged == true && isDetected == true && isPossiveMove == false && isRangeIn == true)
//{
//    // ���� ����
//}

//else if (isPossiveMove == false && isPossiveAttack == false )
//{
//    // ����
//}

//else if (isRanged == false && isDetected == true && isPossiveMove == true && isRangeIn == false)
//{
//    // ������ ������ ����.
//}

//else if (isPossiveMove == false)
//{
//    // �ӹ�
//}

//else if (isRange == false && isDetected == true && isPossiveMove == true && isRangeIn == true)
//{
//    // ���� ������.
//}