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

        // 스폰 파일 불러오기
        // 스폰 파일은 오로지 몬스터 스폰위치에 관련된 정보만 포함해야 한다.

        // 이유 : 굳이 정해져있는 플레이어 스폰위치를 스폰파일에 추가해서
        // if문으로 체크하게 하고 싶지 않다.
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
                //todo: 랜덤하게 생성하도록 바꿔야 한다.
                //todo: 몬스터 종류 랜덤, 몬스터 위치 랜덤
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

    
    // 플레이어 스폰은 일정한 장소에서만 되도록 기획 -> 스폰파일에 포함X
    
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
