using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class GameScene : BaseScene
{
    public GameObject _player;

    //UI_GameScene _sceneUI;
    public int CurrentMapId { get; private set; } = 3;
    public int PlayerType { get; private set; } = 1;

    protected override void Init()
    {
        base.Init();
        // ��� �� �κп� ������ ������ ���� ��Ŷ �ڵ鷯�� ���� ����������
        // ������ Ŭ�󿡼� ���� (���߿� ����)

        SceneType = Define.Scene.Game;

        Clear();

        Managers.Map.CreateMap(CurrentMapId);
        Managers.Spawn.Spawning(PlayerType);

        Managers.Resource.Instantiate("Camera/Main");
        Managers.Resource.Instantiate("UI/CameraUI");

        
        #region
        // ����� ȭ�� ũ��
        //Screen.SetResolution(640, 480, false);

        //Managers.Resource.Instantiate("Creature/Player");
        //_sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        // ���� �ڵ�ȭ
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject monster = Managers.Resource.Instantiate("Creature/Monster");
        //    monster.name = $"Monster_{i + 1}";

        //    // todo : �浹ó��
        //    Vector3Int pos = new Vector3Int()
        //    {
        //        x = Random.Range(-20, 20),
        //        y = Random.Range(-10, 10)
        //    };

        //    MonsterController mc = monster.GetComponent<MonsterController>();
        //    mc.CellPos = pos;

        //    Managers.Object.Add(monster);
        //}

        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        //gameObject.GetOrAddComponent<CursorController>();

        //GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        //Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        ////Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        //GameObject go = new GameObject { name = "SpawningPool" };
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(2);
        #endregion
    }

    public void ChangeMap(int id)
    {
        AllMonsterDistroy();
        AllBulletDistroy();

        Managers.Map.CreateMap(id);
        Managers.Spawn.Spawning(PlayerType);

        CurrentMapId = id;
    }

    public void AllMonsterDistroy()
    {
        GameObject[] _monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject go in _monsters)
        {
            Managers.Resource.Destroy(go);
            
        }
    }

    public void AllBulletDistroy()
    {
        GameObject[] _projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject go in _projectiles)
        {
            Managers.Resource.Destroy(go);

        }
    }

    public void DamagedCreature(GameObject target, GameObject Attacker, int damage = 1)
    {

    }

    public override void Clear()
    {
    }
}
