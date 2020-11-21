using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnValues : MonoBehaviour
{
    public List<GameObject> stoneValues;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnStones(int _value)
    {
        var tmp = Instantiate(stoneValues[_value], this.transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)), this.transform);
        tmp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-90,90), 90)*5, ForceMode2D.Impulse);
    }
}
