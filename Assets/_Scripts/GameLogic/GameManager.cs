using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int clickCount;
    [SerializeField]
    private float defaultRocklife = 5;
    private float rocklife;

    [SerializeField]
    public int defaultRandomMin = 0;
    private int randomMin;

    //Rock values
    private List<Rock> rockValues;
    private int maxRockcounter;

    [Header("ClickShop Upgrades")]
    public float smashMultiplierAdd = 2;
    public float hitMultiplierAdd = 1;
    public int randomDecrease = 7;

    private float money = 0;

    float counter = 0;
    int smashcounter = 1;

    private float smashMultiplier = 1;
    private float hitMultiplier = 1;

    public int basepikeUpgrade = 1000;
    public int baseshovelUpgrade = 1000;
    public int basecartUpgrade = 1000;

    private int pikeUpgrade = 1000;
    private int shovelUpgrade = 1000;
    private int cartUpgrade = 1000;

    public float Money { get => money; set {
            money = value;
            UiManager.instance.moneyText.text = value + " $";
        } 
    }

    public int PikeUpgrade { get => pikeUpgrade; set {
            pikeUpgrade = value;
            UiManager.instance.pikeAmount.text = pikeUpgrade + " $";
        } 
    }
    public int ShovelUpgrade { get => shovelUpgrade; set {
            shovelUpgrade = value;
            UiManager.instance.showlAmount.text = shovelUpgrade + " $";
        } 
    }
    public int CartUpgrade { get => cartUpgrade; set {
            cartUpgrade = value;
            UiManager.instance.cartAmount.text = cartUpgrade + " $";
        }
    }

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        initGamesValues();
        SetRockValues();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetRockValues()
    {
        
        rockValues = new List<Rock>();
        rockValues.Add(new Rock("bronze", 50, 1));
        rockValues.Add(new Rock("silver", 25, 2));
        rockValues.Add(new Rock("gold", 12, 4));
        rockValues.Add(new Rock("platin", 6, 8));
        rockValues.Add(new Rock("diamond", 3, 16));

        foreach (var item in rockValues)
        {
            maxRockcounter += item.Value;
        }
        Debug.Log(maxRockcounter);
    }

    private float SelectRandomStone()
    {
        int randomRock = UnityEngine.Random.Range(0,maxRockcounter);
        Debug.Log(randomRock);
        foreach (var item in rockValues)
        {
            Debug.Log(item.Name+ " " + item.Value  + " " + randomRock);
            if (item.Value <= randomRock)
            {
                Debug.Log(item.Name);
                return item.Money;
            }

            if (randomRock <= rockValues[rockValues.Count-1].Value)
            {
                return rockValues[rockValues.Count - 1].Money;
            }
        }
        return 0;
    }

    private void initGamesValues()
    {
        rocklife = defaultRocklife;
        randomMin = defaultRandomMin;

        PikeUpgrade = basepikeUpgrade;
        ShovelUpgrade = baseshovelUpgrade;
        CartUpgrade = basepikeUpgrade;
    }

    /// <summary>
    /// Decrease rock helath by _value
    /// </summary>
    /// <param name="_value"></param>
    public void SmashRocks(float _value)
    {
        float smashed = (_value * smashMultiplier * hitMultiplier) / defaultRocklife;
        int rockCounter = 0;
        if (smashed < 1)
        {
            if ((rocklife - (smashed * defaultRocklife)) > 0)
            {
                rocklife -= (smashed*defaultRocklife);
            }
            else
            {
                rocklife = defaultRocklife;
                rockCounter++;
            }
        }
        else if(smashed >= 1)
        {
            while (smashed > 1)
            {
                rockCounter++;
                smashed--;
            }

            if ((rocklife - (smashed * defaultRocklife)) > 0)
            {
                rocklife -= (smashed * defaultRocklife);
            }
            else
            {
                rockCounter++;
                rocklife = defaultRocklife;
            }  
        }
        RockValues(rockCounter);
    }

    private void RockValues(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            Money += SelectRandomStone();
        }
    }

    public void BuyItem(string _name)
    {
        switch (_name)
        {
            case "showel":
                if (!Invoice(ShovelUpgrade) && ShovelUpgrade * baseshovelUpgrade > Math.Pow(baseshovelUpgrade, 4))
                    break;
                ShovelUpgrade *= baseshovelUpgrade;
                maxRockcounter -= randomDecrease;
                break;
            case "pike":
                if (!Invoice(PikeUpgrade) && PikeUpgrade*basepikeUpgrade > Math.Pow(basepikeUpgrade,4))
                    break;
                PikeUpgrade *= basepikeUpgrade;
                smashMultiplier += smashMultiplierAdd;
                break;
            case "cart":
                if (!Invoice(CartUpgrade) && CartUpgrade * basecartUpgrade > Math.Pow(basecartUpgrade, 4))
                    break;
                CartUpgrade *= basecartUpgrade;
                hitMultiplier += hitMultiplierAdd;
                break;
        }
    }

    private bool Invoice(float _amount)
    {
        if(money >= _amount)
        {
            money -= _amount;
            return true;
        }
        return false;
    }
}
