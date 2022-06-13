using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T Component = go.GetComponent<T>();
        if (Component == null)
        {
            Component = go.AddComponent<T>();
        }
        return Component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static List<GameObject> FindChilds(GameObject go, string name = null, bool recursive = false, bool numbering = false)
    {
        List<GameObject> objects = new List<GameObject>();

        for(int i = 0; i < go.transform.childCount; i++)
        {
            if (numbering == true)
            {
                string nameing = $"{name}_{i}";

                Transform tr = FindChild<Transform>(go, nameing, recursive);
                if (tr != null)
                {
                    objects.Add(tr.gameObject);
                }
            }

            else
            {
                Transform tr = FindChild<Transform>(go, name, recursive);
                if (tr != null)
                {
                    objects.Add(tr.gameObject);
                }
            }

        }

        return objects;
    }

    public static List<GameObject> FindChildsInText(GameObject go, string name = null)
    {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform tr = go.transform.GetChild(i);

            if (tr.gameObject.name.Contains(name))
            {
                objects.Add(tr.gameObject);
            }
        }

        return objects;
    }

    public static Dictionary<string, GameObject> FindEventObjects(GameObject go, int type)
    {
        Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform tr = go.transform.GetChild(i);

            if (Int32.Parse(tr.gameObject.name.Split('-')[1]) == type)
            {
                objects.Add(tr.gameObject.name.Split('-')[2], tr.gameObject);
            }
        }

        return objects;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Texture2D ConvertSpriteToTexture(Sprite sprite)
    {

        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D office_Stamp_Text = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x,
                                                        (int)sprite.rect.y,
                                                        (int)sprite.rect.width,
                                                        (int)sprite.rect.height);
            office_Stamp_Text.SetPixels(pixels);
            office_Stamp_Text.Apply();

            return office_Stamp_Text;
        }
        else
            return sprite.texture;

    }

    public static int GetMonsterId(string name)
    {
        string[] split = name.Split('_');
        return Int32.Parse(split[2]);
    }

    public static int GetMonsterType(string name)
    {
        string[] split = name.Split('_');
        return Int32.Parse(split[1]);
    }

    public static int GetPlayerType(string name)
    {
        string[] split = name.Split('_');
        return Int32.Parse(split[1]);
    }
}
