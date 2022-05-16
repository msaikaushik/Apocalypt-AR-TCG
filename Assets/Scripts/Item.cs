using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public string stat;
    public string effect;
    public int amount;

    public Item(string name, string stat, string effect, int amount)
    {
        this.name = name;
        this.stat = stat;
        this.effect = effect;
        this.amount = amount;
    }
}