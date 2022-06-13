using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager {
    public Dictionary<int, Data.Skill> SkillDict { get; private set; } = new Dictionary<int, Data.Skill>();
    public Dictionary<int, Data.Map> MapDict { get; private set; } = new Dictionary<int, Data.Map>();
    public Dictionary<int, Data.Monster> MonsterDict { get; private set; } = new Dictionary<int, Data.Monster>();
    public Dictionary<int, Data.Item> ItemDict { get; private set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.Player> PlayerDict { get; private set; } = new Dictionary<int, Data.Player>();

    public void Init()
    {
        //StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
        SkillDict = LoadJson<Data.SkillLoader, int, Data.Skill>("SkillData").MakeDict();
        MapDict = LoadJson<Data.MapLoader, int, Data.Map>("MapData").MakeDict();
        MonsterDict = LoadJson<Data.MonsterLoader, int, Data.Monster>("MonsterData").MakeDict();
        ItemDict = LoadJson<Data.ItemLoader, int, Data.Item> ("ItemData").MakeDict();
        PlayerDict = LoadJson<Data.PlayerLoader, int, Data.Player>("PlayerData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}
