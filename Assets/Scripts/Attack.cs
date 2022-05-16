using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string name;
    public int damage;
    public int cost;

    public Attack(string name, int damage, int cost)
    {
        this.name = name;
        this.damage = damage;
        this.cost = cost;
    }
}
