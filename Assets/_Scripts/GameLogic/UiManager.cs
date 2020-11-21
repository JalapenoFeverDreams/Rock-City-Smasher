using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text moneyText;
    [Header("Click-Shop Values")]
    public GameObject shopWindowClicker;
    public Text pikeAmount;
    public Text showlAmount;
    public Text cartAmount;
    public static UiManager instance;

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

    public void BuyShopItem(string _name)
    {
        GameManager.instance.BuyItem(_name);
    }

    public void OpenShopWindow()
    {
        shopWindowClicker.SetActive(!shopWindowClicker.activeSelf);   
    }
}
