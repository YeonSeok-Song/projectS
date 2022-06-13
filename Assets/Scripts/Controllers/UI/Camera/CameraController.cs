using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : UIBaseController
{
    public float Horizontal { get; protected set; }
    public float Vertical { get; protected set; }

    protected virtual void SetSize() {}

    protected Vector3 DontOverMap(Vector3 pos)
    {
        if (Managers.Map.XLength == 0 || Managers.Map.XLength == 0) return pos;

        int mapWidth = Managers.Map.XLength;
        int mapHeight = Managers.Map.YLength;

        int maxX = Managers.Map.MaxX;
        int maxY = Managers.Map.MaxY;

        // ¸Ê Á¤ Áß¾Ó ÁÂÇ¥
        int ZeroX = maxX - (mapWidth / 2);
        int ZeroY = maxY - (mapWidth / 2);

        // Ä«¸Þ¶ó ¹üÀ§
        float rightX = (ZeroX - (mapWidth / 2)) + Horizontal;
        float leftX = ZeroX + (mapWidth / 2) - Horizontal;
        float topY = ZeroY + (mapHeight / 2) - Vertical;
        float botY = ZeroY - (mapHeight / 2) + Vertical;

        float x = Mathf.Clamp(pos.x, rightX, leftX);
        float y = Mathf.Clamp(pos.y, botY, topY);

        return new Vector3(x, y, pos.z);

    }
}
