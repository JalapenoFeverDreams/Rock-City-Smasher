using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock
{
    private int value;
    private string name;
    private float money;


    public int Value { get => value; set => this.value = value; }
    public string Name { get => name; set => name = value; }
    public float Money { get => money; set => money = value; }

    public Rock(string _name, int _value, float _moneyValue)
    {
        Name = _name;
        Value = _value;
        Money = _moneyValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
