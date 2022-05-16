using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI nameTextField;
    public GameObject character;

    void Start()
    {
        setName(character.name);
    }

    public void setName(string name)
    {
        nameTextField.text = name;
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health) 
    { 
        slider.value = health;
    }
}
