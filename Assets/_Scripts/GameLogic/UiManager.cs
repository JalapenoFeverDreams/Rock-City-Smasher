﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Buildings;

public class UiManager : MonoBehaviour
{
    public Text moneyText;
    [Header("Click-Shop Values")]
    public GameObject shopWindowClicker;
    public Text pikeAmount;
    public Text showlAmount;
    public Text cartAmount;

    [Header("Buttons")]
    public GameObject townUI;
    public GameObject backToTown;
    public GameObject rocks;
    public GameObject shop;

    [Header("Building Costs")]
    public Text houseCost;
    public Text farmCost;
    public Text entertainmentCost;
    public Text marketplaceCost;
    public Text streetCost;
    public static UiManager instance;

    [Header("Values")]
    [SerializeField] private Text m_PeopleCountToLimitText;

    /// <summary>
    /// Gets or sets the People count to limit text object.
    /// </summary>
    public Text PeopleCountToLimitText
    {
        get => m_PeopleCountToLimitText;
        set => m_PeopleCountToLimitText = value;
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
        initClickShopValues();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void initClickShopValues()
    {
        pikeAmount.text = GameManager.instance.basepikeUpgrade + " $";
        showlAmount.text = GameManager.instance.baseshovelUpgrade + " $";
        cartAmount.text = GameManager.instance.basecartUpgrade + " $";
    }

    public void SetBuildingCost(BaseBuilding building)
    {
        int cost = building.Cost;
        switch(building.BuildingType)
        {
            case BuildingType.House:
                houseCost.text = $"House ({cost} $)";
                break;
            case BuildingType.Farm:
                farmCost.text = $"Farm ({cost} $) - (5 P)";
                break;
            case BuildingType.Entertainment:
                entertainmentCost.text = $"Entertainment ({cost} $)";
                break;
            case BuildingType.Marketplace:
                marketplaceCost.text = $"Marketplace ({cost }$)";
                break;
            case BuildingType.Street:
                streetCost.text = $"Street ({cost} $)";
                break;
        }        
    }

    public void BuyShopItem(string _name)
    {
        GameManager.instance.BuyItem(_name);
    }

    public void BuyBuilding(BaseBuilding building)
    {
        GameManager.instance.BuyBuilding(building);
    }

    public void OpenShopWindow()
    {
        shopWindowClicker.SetActive(!shopWindowClicker.activeSelf);   
    }

    public void BackToTown()
    {
        townUI.SetActive(!townUI.activeSelf);
        rocks.SetActive(!townUI.activeSelf);
        shop.SetActive(!townUI.activeSelf);
        backToTown.SetActive(!townUI.activeSelf);
    }
}
