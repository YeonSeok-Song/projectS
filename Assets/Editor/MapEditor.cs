using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR
    [MenuItem("Tools/GenerateMap")] // ����Ű�� ����
    private static void GenerateMap()
    {

        GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach(GameObject go in maps)
        {
            Tilemap tmBase = Utils.FindChild<Tilemap>(go, "Background", true);
            Tilemap co = Utils.FindChild<Tilemap>(go, "Collision", true);
            Tilemap spPlayer = Utils.FindChild<Tilemap>(go, "Spawn_Player", true);
            Tilemap spMonster = Utils.FindChild<Tilemap>(go, "Spawn_Monster", true);

            #region
            //List<Vector3Int> blocked = new List<Vector3Int>();

            // _tilemap.cellBounds.allPositionsWithin
            // Ÿ�ϸʿ��� cellBounds => Ÿ�ϸ� ��輱�� ���´�
            // allPositionsWithin => Ÿ�ϸ� ��輱 �ȿ� �ִ� ��� cell ������ �迭���·� �����´�.
            //foreach (Vector3Int pos in tm.cellBounds.allPositionsWithin)
            //{
            //    TileBase tile = tm.GetTile(pos);
            //    if (tile != null)
            //    {
            //        blocked.Add(pos);
            //    }
            //}
            #endregion

            // ���� �����
            // ���� ���� ���ϱ� => ���̳ʸ�? �ؽ�Ʈ?
            using (var writer = File.CreateText($"Assets/Resources/Map/{go.name}.txt"))
            {
                writer.WriteLine(tmBase.cellBounds.xMax);
                writer.WriteLine(tmBase.cellBounds.xMin);
                writer.WriteLine(tmBase.cellBounds.yMax);
                writer.WriteLine(tmBase.cellBounds.yMin);

                // y, x
                // max, 0           max, max
                //
                //
                // 0,0               0, max

                for (int y = tmBase.cellBounds.yMax; y >= tmBase.cellBounds.yMin; y--)
                {
                    for (int x = tmBase.cellBounds.xMin; x <= tmBase.cellBounds.xMax; x++)
                    {
                        TileBase tl = co.GetTile(new Vector3Int(x, y, 0));

                        if (tl != null)
                        {
                            writer.Write('1');
                        }
                        else
                        {
                            writer.Write('0');
                        }
                    }
                    writer.WriteLine();
                }
            }

            if (spPlayer != null && spMonster != null)
            {
                using (var writer = File.CreateText($"Assets/Resources/Map/{go.name}_Spawn.txt"))
                {
                    writer.WriteLine(tmBase.cellBounds.xMax);
                    writer.WriteLine(tmBase.cellBounds.xMin);
                    writer.WriteLine(tmBase.cellBounds.yMax);
                    writer.WriteLine(tmBase.cellBounds.yMin);

                    // y, x
                    // max, 0           max, max
                    //
                    //
                    // 0,0               0, max

                    for (int y = tmBase.cellBounds.yMax; y >= tmBase.cellBounds.yMin; y--)
                    {
                        for (int x = tmBase.cellBounds.xMin; x <= tmBase.cellBounds.xMax; x++)
                        {
                            TileBase tilePlayer = spPlayer.GetTile(new Vector3Int(x, y, 0));
                            TileBase tileMonster = spMonster.GetTile(new Vector3Int(x, y, 0));

                            if (tilePlayer != null)
                            {
                                writer.Write('1');
                            }
                            else if (tilePlayer == null && tileMonster != null)
                            {
                                writer.Write('2');
                            }
                            else
                            {
                                writer.Write('0');
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
        }
    }

    
#endif


}
