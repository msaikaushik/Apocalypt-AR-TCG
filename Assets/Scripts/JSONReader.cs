using System;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Attack
    {
        public string name;
        public int damage;
        public int cost;
    }

    [System.Serializable]
    public class Item
    {
        public string name;
        public string stat;
        public string effect;
        public int amount;
    }

    [System.Serializable]
    public class AttackList
    {
        public Attack[] attacks;
    }

    [System.Serializable]
    public class ItemList
    {
        public Item[] items;
    }

    [SerializeField]
    public static AttackList attackList = new AttackList();

    [SerializeField]
    public static ItemList itemList = new ItemList();

    private void Start()
    {
        attackList = JsonUtility.FromJson<AttackList>(textJSON.text);

        itemList = JsonUtility.FromJson<ItemList>(textJSON.text);
    }

}
