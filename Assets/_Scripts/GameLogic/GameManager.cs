using System;
using System.Linq;
using System.Collections.Generic;
using Scripts.Buildings;
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

    private float money = 1000000;
    private int m_PeopleCount = 0;
    private int m_PeopleLimit = 0;

    float counter = 0;
    int smashcounter = 1;

    public float smashMultiplier = 1;
    public float hitMultiplier = 1;

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

    /// <summary>
    /// Gets or sets the current people count in the game.
    /// </summary>
    public int PeopleCount 
    { 
        get => m_PeopleCount; 
        set 
        {
            m_PeopleCount = value;
            UiManager.instance.PeopleCountToLimitText.text = $"{value} / {PeopleLimit} P";
        } 
    }

    public int PeopleLimit
    {
        get => m_PeopleLimit;
        set
        {
            m_PeopleLimit = value;
            UiManager.instance.PeopleCountToLimitText.text = $"{PeopleCount} / {value} P ";
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
    }

    // Start is called before the first frame update
    void Start()
    {
        initGamesValues();
        SetRockValues();
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
    }

    private float SelectRandomStone()
    {
        int randomRock = UnityEngine.Random.Range(0,maxRockcounter);
        foreach (var item in rockValues)
        {
            if (item.Value <= randomRock)
            {
                UiManager.instance.smashableRock.GetComponent<SpawnValues>().SpawnStones(NameToIndex(item.Name));
                SoundManager.instance.StoneDrop();
                return item.Money;
            }

            if (randomRock <= rockValues[rockValues.Count-1].Value)
            {
                UiManager.instance.smashableRock.GetComponent<SpawnValues>().SpawnStones(NameToIndex(item.Name));
                SoundManager.instance.StoneDrop();
                return rockValues[rockValues.Count - 1].Money;
            }
        }
        return 0;
    }

    int NameToIndex(string _name)
    {
        switch (_name)
        {
            case "bronze":
                return 0;
            case "silver":
                return 1;
            case "gold":
                return 2;
            case "platin":
                return 3;
            case "diamond":
                return 4;
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
    /// Decrease rock health by _value
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
                try
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[(int)rocklife - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();

                }
                catch (Exception)
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[UiManager.instance.rockState.Count-1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();
                }
            }
            else
            {
                rocklife = defaultRocklife;
                try
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[(int)rocklife - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();

                }
                catch (Exception)
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[UiManager.instance.rockState.Count - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();
                }
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
                try
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[(int)rocklife - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();

                }
                catch (Exception)
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[UiManager.instance.rockState.Count - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();
                }
            }
            else
            {
                rockCounter++;
                rocklife = defaultRocklife;
                try
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[(int)rocklife - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();

                }
                catch (Exception)
                {
                    UiManager.instance.smashableRock.sprite = UiManager.instance.rockState[UiManager.instance.rockState.Count - 1];
                    UiManager.instance.smashableRock.GetComponent<SpawnValues>().amount++;
                    SoundManager.instance.Rocksmash();
                }
            }  
        }
        RockValues(rockCounter);
    }

    private void RockValues(int _amount)
    {
        var multiplier = 1f;

        var farmBuilding = BuildingManager.Instance.Buildings.FirstOrDefault(x => x.BuildingType == BuildingType.Farm);
        if(farmBuilding != null)
        {
            multiplier = Mathf.Pow((farmBuilding as FarmBuilding).MaterialMultiplyFactor, BuildingManager.Instance.Buildings.Count(x => x.BuildingType == BuildingType.Farm));
        }

        for (int i = 0; i < _amount; i++)
        {
            Money += SelectRandomStone() * multiplier;
        }
    }

    public void BuyItem(string _name)
    {
        switch (_name)
        {
            case "showel":
                if (!Invoice(ShovelUpgrade) || ShovelUpgrade * baseshovelUpgrade > Math.Pow(baseshovelUpgrade, 4))
                {
                    SoundManager.instance.BuyFail();
                    break;
                }
                ShovelUpgrade *= baseshovelUpgrade;
                SoundManager.instance.BuySuccess();
                maxRockcounter -= randomDecrease;
                break;
            case "pike":
                if (!Invoice(PikeUpgrade) || PikeUpgrade * basepikeUpgrade > Math.Pow(basepikeUpgrade, 4))
                {
                    SoundManager.instance.BuyFail();
                    break;
                }
                PikeUpgrade *= basepikeUpgrade;
                SoundManager.instance.BuySuccess();
                smashMultiplier += smashMultiplierAdd;
                break;
            case "cart":
                if (!Invoice(CartUpgrade) || CartUpgrade * basecartUpgrade > Math.Pow(basecartUpgrade, 4))
                {
                    SoundManager.instance.BuyFail();
                    break;
                }
                CartUpgrade *= basecartUpgrade;
                SoundManager.instance.BuySuccess();
                hitMultiplier += hitMultiplierAdd;
                break;
        }
    }

    /// <summary>
    /// Buys the defined building.
    /// </summary>
    /// <param name="building"></param>
    public bool BuyBuilding(BaseBuilding building)
    {
        if (building.BuildingType == BuildingType.Farm && !EnoughPlace((building as FarmBuilding).PeopleCountIncrease))
        {
            return false;
        }

        if (!Invoice(building.Cost))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets the current cost for the building.
    /// </summary>
    /// <param name="building"></param>
    public void SetBuildingCost(BaseBuilding building)
    {
        UiManager.instance.SetBuildingCost(building);
    }

    private bool Invoice(float _amount)
    {
        if(Money >= _amount)
        {
            Money -= _amount;
            return true;
        }
        return false;
    }

    private bool EnoughPlace(int amount)
    {
        if(PeopleCount + amount > PeopleLimit)
        {
            return false;
        }

        return true;
    }
}
