using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    // 자료구조
    // 루트 풀 -> 각 오브젝트를 대표하는 오브젝트이름_루트풀 -> 만든 오브젝트를 자기 이름의 루트 풀에 집어넣기
    #region Pool 
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 8)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return Utils.GetOrAddComponent<Poolable>(go);
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
            {
                return;
            }

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }
            poolable.gameObject.SetActive(true);

            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
                // 아래의 돈디스트로이 해제용 코드
            }
            poolable.transform.parent = parent;
            
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion
    // 코드 접기

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
            // 한번이라도 DontDestroyOnLoad로 본인 또는 자식으로 설정되면 계속 유지된다.
            // 여기서는 풀링 오브젝트의 자식으로 들어가서 설정된다.
            // 해제하려면 이 설정이 걸리지 않은 어떤 오브젝트로 한번이라도 이동하면 된다.
            // pool클래스의 pop에서 currentscene의 자식으로 설정하도록 해보자

        }
    }

    public void CreatePool(GameObject original, int count = 1)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);

    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }
        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }
        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
}
