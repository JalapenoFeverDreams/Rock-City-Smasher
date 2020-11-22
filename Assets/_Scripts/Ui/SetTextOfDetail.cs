using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextOfDetail : MonoBehaviour
{
    public string text;
    public GameObject frame;
    public Text textFrame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDetails()
    {
        textFrame.text = text;
        frame.SetActive(true);
    }

    public void HideDetails()
    {
        frame.SetActive(false);
    }
}
