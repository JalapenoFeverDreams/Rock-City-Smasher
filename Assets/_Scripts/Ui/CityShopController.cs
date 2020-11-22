using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityShopController : MonoBehaviour
{
    public GameObject targetFrame;
    public Sprite up;
    public Sprite down;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapFrame()
    {
        var current = this.GetComponent<Image>().sprite;
        if (current.name == up.name)
        {
            this.GetComponent<Image>().sprite = down;
            targetFrame.SetActive(!targetFrame.activeSelf);
        }
        else
        {
            targetFrame.SetActive(!targetFrame.activeSelf);
            this.GetComponent<Image>().sprite = up;
        }
    }
}
