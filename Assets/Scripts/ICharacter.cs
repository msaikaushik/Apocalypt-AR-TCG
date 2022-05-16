using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public string Name { get; set; }
    public int Mana { get; set; }
    public int Hitpoints { get; set; }  
    public int ExperiencePoints { get; set; }
    public List<string> Attacks { get; set; }
    public List<string> Items { get; set; }
}
