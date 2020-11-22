using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameAsOther : MonoBehaviour
{
    public GameObject other;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(other.gameObject.activeSelf);
    }
}
