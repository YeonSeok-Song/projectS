using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Data;

public class SpawnManager
{

    public struct SpawnPos
    {
        public float x;
        public float y;
        public int type;

        public SpawnPos(float x, float y, int type) : this()
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }

    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }

    List<SpawnPos> _spawn = new List<SpawnPos>();

    public void LoadSpawn(int mapId)
    {
        this.Clear();

        string mapName = Managers.Data.MapDict[mapId].name;

        // ���� ���� �ҷ�����
        // ���� ������ ������ ���� ������ġ�� ���õ� ������ �����ؾ� �Ѵ�.

        // ���� : ���� �������ִ� �÷��̾� ������ġ�� �������Ͽ� �߰��ؼ�
        // if������ üũ�ϰ� �ϰ� ���� �ʴ�.
        TextAsset spTxt = Managers.Resource.Load<TextAsset>($"Map/{mapName}_Spawn");

        if (spTxt == null)
        {
            return;
        }

        StringReader reader = new StringReader(spTxt.text);

        MaxX = int.Parse(reader.ReadLine());
        MinX = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());

        int xCount = MaxX - MinX + 1;
        int yCount = MaxY - MinY + 1;

        for (int y = 0; y < yCount; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < xCount; x++)
            {
                int type = line[x] - '0';
                if (type > 0)
                {
                    float posX = x + MinX;
                    float posY = MaxY - y;
                    _spawn.Add(new SpawnPos(posX, posY, type));
                }
            }
        }
    }
    public void Spawning(int playerType)
    {
        int inGameId = 0;
        if (_spawn == null)
        {
            return;
        }
        foreach (SpawnPos pos in _spawn)
        {
            if (pos.type == 1)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                if (!go)
                {
                    PlayerSpawn(pos, playerType);
                }
                else
                {
                    PlayerReposition(go, pos);
                }
            }
            else if (pos.type == 2)
            {
                //todo: �����ϰ� �����ϵ��� �ٲ�� �Ѵ�.
                //todo: ���� ���� ����, ���� ��ġ ����
                MonsterSpawn(pos, inGameId);
                inGameId++;
            }
        }
    }

    public void PlayerReposition(GameObject go, SpawnPos pos)
    {
        if (go == null)
        {
            return;
        }
        go.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    protected void MonsterSpawn(SpawnPos pos, int inGameId)
    {
        Monster data = Managers.Data.MonsterDict[pos.type];
        GameObject go = Managers.Resource.Instantiate($"Creature/Monsters/{data.name}");
        if (go == null)
        {
            return;
        }
        
        go.name = $"{data.name}_{pos.type}_{inGameId}";
        go.transform.position = new Vector3(pos.x, pos.y, 0);
        
    }

    
    // �÷��̾� ������ ������ ��ҿ����� �ǵ��� ��ȹ -> �������Ͽ� ����X
    
    protected void PlayerSpawn(SpawnPos pos, int playerType)
    {
        Player data = Managers.Data.PlayerDict[playerType];
        GameObject go = Managers.Resource.Instantiate($"Creature/{data.name}");
        if (go == null)
        {
            return;
        }
        go.name = $"{data.name}_{playerType}";
        go.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    public void Clear()
    {
        _spawn.Clear();
    }
}
