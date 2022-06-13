using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager
{
    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }
    public int XLength { get; private set; }
    public int YLength { get; private set; }

    bool[,] _collision;

    public Grid CurrentGrid { get; private set; }

    public void LoadMap(GameObject go)
    {
        CurrentGrid = go.GetComponent<Grid>();

        // 충돌 관련 파일 불러오기
        TextAsset coTxt = Managers.Resource.Load<TextAsset>($"Map/{go.name}");
        StringReader reader = new StringReader(coTxt.text);

        MaxX = int.Parse(reader.ReadLine());
        MinX = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());

        XLength = MaxX - MinX + 1;
        YLength = MaxY - MinY + 1;

        _collision = new bool[YLength, XLength];

        for (int y = 0; y < YLength; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < XLength; x++)
            {
                _collision[y, x] = (line[x] == '1' ? true : false);
            }
        }
    }

    public void DestroyMap()
    {
        GameObject map = GameObject.Find("map");
        
        if (map != null)
        {
            Managers.Resource.Destroy(map);
            CurrentGrid = null;
        }
    }

    public void CreateMap(int mapId)
    {
        DestroyMap();
        string mapName = Managers.Data.MapDict[mapId].name;
        GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");
        LoadMap(go);
        
        go.name = "map";
        Managers.Spawn.LoadSpawn(mapId);
    }

    public bool CanGo (Vector3 pos)
    {
        // https://tenlie10.tistory.com/141
        // 2D 충돌 로직

        if (pos.x < MinX || pos.x > MaxX)
        {
            return false;
        }

        if (pos.y < MinY || pos.y > MaxY)
        {
            return false;
        }

        List<Vector2> edges = new List<Vector2>();

        // 월드맵 기준 0,0에서 오른쪽은 -1 인데
        // 맵 에디터의 맵텍스트 생성시 방향을 반대로 가정해 만들어서
        // 이런 수식이 생김. - 해줘야 한다. (원래 월드맵 기준 x + MinX임)
        float x = pos.x - MinX - 0.5f;
        float y = MaxY - pos.y;

        // 캐릭터 바닥 (4각형)
        edges.Add(new Vector2(x - 0.25f, y + 0.9f));
        edges.Add(new Vector2(x + 0.25f, y + 0.9f));
        edges.Add(new Vector2(x - 0.25f, y + 1f));
        edges.Add(new Vector2(x + 0.25f, y + 1f));

        foreach (Vector2 edge in edges)
        {
            if (_collision[(int)Math.Round(edge.y), (int)Math.Round(edge.x)] == true)
            {
                //Debug.Log($"Collision!! {edge}");
                return false;
            }

            #region

            // velo.x > 1 => 오른쪽 velo.x < 0 => 왼쪽
            // velo.y > 1 => 위 velo.y < 0 => 아래

            // 위
            //if (velo.x == 0 && velo.y > 0)
            //{
            //    if (_collision[(int)Math.Truncate(edge.y), (int)Math.Truncate(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}
            //// 아래
            //else if (velo.x == 0 && velo.y < 0)
            //{
            //    if (_collision[(int)Math.Ceiling(edge.y), (int)Math.Ceiling(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}
            //// 왼
            //else if (velo.x < 0 && velo.y == 0)
            //{
            //    if (_collision[(int)Math.Truncate(edge.y), (int)Math.Truncate(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}
            //// 오
            //else if (velo.x > 0 && velo.y == 0)
            //{
            //    if (_collision[(int)Math.Ceiling(edge.y), (int)Math.Ceiling(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}

            ////왼위
            //else if (velo.x < 0 && velo.y > 0)
            //{
            //    if (_collision[(int)Math.Truncate(edge.y), (int)Math.Truncate(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}

            ////오위
            //else if (velo.x > 0 && velo.y > 0)
            //{
            //    if (_collision[(int)Math.Truncate(edge.y), (int)Math.Ceiling(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}

            ////왼아
            //else if (velo.x < 0 && velo.y < 0)
            //{
            //    if (_collision[(int)Math.Ceiling(edge.y), (int)Math.Truncate(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}

            ////오아
            //else if (velo.x > 0 && velo.y < 0)
            //{
            //    if (_collision[(int)Math.Ceiling(edge.y), (int)Math.Ceiling(edge.x)] == true)
            //    {
            //        Debug.Log(edge);
            //        return false;
            //    }
            //}

            #endregion
        }

        return true;

        //return !_collision[y, x];
    }
}
