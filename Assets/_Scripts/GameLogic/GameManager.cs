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
    int defaultRandomMin = 0;
    private int randomMin;

    [SerializeField]
    private float defaultTickRate = 0;
    private float tickRate;

    //Rock values
    private List<Rock> rockValues;
    private int maxRockcounter;

    private float money = 0;

    float counter = 0;
    int smashcounter = 1;

    public float Money { get => money; set {
            money = value;
            UiManager.instance.moneyText.text = value + " $";
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
        int randomRock = Random.Range(randomMin,maxRockcounter);
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
        tickRate = defaultTickRate;
    }

    /// <summary>
    /// Decrease rock helath by _value
    /// </summary>
    /// <param name="_value"></param>
    public void SmashRocks(float _value)
    {
        float smashed = _value / defaultRocklife;
        int rockCounter = 0;
        if (smashed < 1)
        {
            if ((rocklife - (smashed * defaultRocklife)) > 0)
            {
                rocklife -= (smashed*defaultRocklife);
            }
            else
            {
                rocklife = 5;
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
                rocklife = 5;
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
}
