using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    // SOAP (Simple Object Access Protocol) 포맷(XML 형태)으로 클래스 객체를 Serialize해서 저장한다
    public class Pos
    {
        float x;
        float y;
    }

    [Serializable]
    public class Skill
    {
        public int id;
    }


    [Serializable]
    public class Map
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Monster
    {
        public int id;
        public string name;
        public float maxHp;
        public float attack;
        public float defense;
    }

    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public float damage;
        public float defense;
        public float price;
        //todo: 효과를 어떻게 저장해야할까?
    }

    [Serializable]
    public class Player
    {
        public int id;
        public string name;
        public float maxHp;
        public float attack;
    }

    // ILoader 인터페이스를 상속해서 makeDict를 만들어야 함.
    public class SkillLoader : ILoader<int, Skill>
    {
        public List<Skill> skills = new List<Skill>();
        // 아니 대체 리스트를 어디서 받아오는거지?
        // json 데이터 보니까 skills 배열이 있다..
        // 아마 파싱할때 이 배열로 바로 꽂아주니까 디버깅해도 안나온것일듯.
        // 암픈 파일 작성시 key 이름을 일치시켜야 할듯.
        
        public Dictionary<int, Skill> MakeDict()
        {
            Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
            foreach(Skill skill in skills)
            {
                dict.Add(skill.id, skill);
            }
            return dict;
        }
    }

    public class MapLoader : ILoader<int, Map>
    {
        public List<Map> maps = new List<Map>();

        public Dictionary<int, Map> MakeDict()
        {
            Dictionary<int, Map> dict = new Dictionary<int, Map>();
            foreach (Map map in maps)
            {
                dict.Add(map.id, map);
            }
            return dict;
        }
    }

    public class MonsterLoader : ILoader<int, Monster>
    {
        public List<Monster> monsters = new List<Monster>();

        public Dictionary<int, Monster> MakeDict()
        {
            Dictionary<int, Monster> dict = new Dictionary<int, Monster>();
            foreach (Monster monster in monsters)
            {
                dict.Add(monster.id, monster);
            }
            return dict;
        }
    }

    public class ItemLoader : ILoader<int, Item>
    {
        public List<Item> items = new List<Item>();

        public Dictionary<int, Item> MakeDict()
        {
            Dictionary<int, Item> dict = new Dictionary<int, Item>();
            foreach (Item item in items)
            {
                dict.Add(item.id, item);
            }
            return dict;
        }
    }

    public class PlayerLoader : ILoader<int, Player>
    {
        public List<Player> players = new List<Player>();

        public Dictionary<int, Player> MakeDict()
        {
            Dictionary<int, Player> dict = new Dictionary<int, Player>();
            foreach (Player player in players)
            {
                dict.Add(player.id, player);
            }
            return dict;
        }
    }

}
