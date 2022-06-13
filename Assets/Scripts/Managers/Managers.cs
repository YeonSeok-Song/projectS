using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _Instance;
    static Managers Instance { get { Init();  return _Instance; } }


    #region Core
    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ScenesManager _scene = new ScenesManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ScenesManager Scene { get { return Instance._scene; } }


    #endregion

    #region Contents
    MapManager _map = new MapManager();
    UIManager _ui = new UIManager();
    SpawnManager _spawn = new SpawnManager();
    CameraManager _camera = new CameraManager();
    InventoryManager _inventory = new InventoryManager();
    AIManager _ai = new AIManager();
    DropManager _drop = new DropManager();

    public static MapManager Map { get { return Instance._map; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SpawnManager Spawn { get { return Instance._spawn; } }
    public static CameraManager CameraM { get { return Instance._camera; } }
    public static InventoryManager Inventory { get { return Instance._inventory; } }
    public static AIManager AI { get { return Instance._ai; } }
    public static DropManager Drop { get { return Instance._drop; } }


    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        //_network.Update();
        //UI.RotateScreen();
        UI.setScreen();
    }

    static void Init()
    {
        if (_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            _Instance = go.GetComponent<Managers>();

            //_Instance._camera.init();
            _Instance._data.Init();
            _Instance._ui.Init();
            //_Instance._spawn.Init();
            //_Instance._network.Init();
            _Instance._pool.Init();
            //_Instance._sound.Init();
            _Instance._ai.Init();
        }
    }

    public static void Clear()
    {
        //Sound.Clear();
        //Scene.Clear();
        //UI.Clear();
        //Pool.Clear();
    }

}
