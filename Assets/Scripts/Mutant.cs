using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant : MonoBehaviour, ICharacter
{
    private Transform characterTransform;

    // set health bar max value in the beginning and keep updating the current value.
    private string _name;
    private int _mana;
    private int _hp;
    private int _exp;
    private List<string> _attacks;
    private List<string> _items;

    [SerializeField]
    private DetailBar _detailBar;

    [SerializeField]
    private GameObject GameManager;

    private GameObject[] opponent;

    public string Name { get => _name; set => _name = value; }
    public int Mana { get => _mana; set => _mana = value; }
    public int Hitpoints { get => _hp; set => _hp = value; }
    public int ExperiencePoints { get => _exp; set => _exp = value; }
    public List<string> Attacks { get => _attacks; set => _attacks = value; }
    public List<string> Items { get => _items; set => _items = value; }

    void Start()
    {
        Name = "Mutant";
        Hitpoints = 200;
        Mana = 40;
        ExperiencePoints = 20;
        Attacks = new List<string> { "tackle", "jump attack", "takedown" };
        Items = new List<string> { "health potion", "L.health", "mana potion" };

        opponent = new GameObject[] { };
        _detailBar.setMaxHealth(Hitpoints);
    }

    void Update()
    {
        _detailBar.setHealth(Hitpoints);

        if (opponent.Length == 0)
        {
            opponent = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject character in opponent)
            {
                if (!character.activeInHierarchy)
                {
                    Utilities.RemoveFromArray(opponent, character);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (opponent != null)
        {
            transform.LookAt(opponent[0].GetComponent<Transform>().position);
        }
    }
}
