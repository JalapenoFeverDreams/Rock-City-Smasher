using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Details")]
    public GameObject details;
    public Text detailsDescr;

    [Header("RockSmashStates")]
    [SerializeField]
    public List<Sprite> rockState;
    [SerializeField]
    public Image smashableRock;
    public GameObject rockSmashPlayground;

    [Header("Values")]
    [SerializeField] private Text m_PeopleCountToLimitText;

    [Header("LoadingPanel")]
    public GameObject loadingPanel;

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
        details.SetActive(false);
        shopWindowClicker.SetActive(false);
        try
        {
            GameObject.Find("LoadingPanel").SetActive(false);
            loadingPanel.SetActive(false);
        }
        catch (System.Exception)
        {

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

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
        SoundManager.instance.ButtonSound();
        shopWindowClicker.SetActive(!shopWindowClicker.activeSelf);
        details.SetActive(false);
        rocks.SetActive(!shopWindowClicker.activeSelf);
    }

    public void BackToTown()
    {
        SoundManager.instance.ButtonSound();
        shopWindowClicker.SetActive(false);
        townUI.SetActive(!townUI.activeSelf);
        rocks.SetActive(!townUI.activeSelf);
        shop.SetActive(!townUI.activeSelf);
        backToTown.SetActive(!townUI.activeSelf);
        rockSmashPlayground.SetActive(!townUI.activeSelf);
    }

    public void setDetailsText(string _text)
    {
        details.SetActive(true);
        detailsDescr.text = _text;
    }

    public void StartGame()
    {
        loadingPanel.SetActive(true);
        Destroy(this.gameObject);
        SceneManager.LoadScene("Buildings");
    }

    public void BackToMain()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene("MainMenu");
        try
        {
            Destroy(GameManager.instance.gameObject);
            Destroy(UiManager.instance.gameObject);
        }
        catch (System.Exception)
        {

        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
