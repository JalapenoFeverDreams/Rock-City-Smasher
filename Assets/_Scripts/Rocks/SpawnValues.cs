using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnValues : MonoBehaviour
{
    public List<GameObject> stoneValues;
    public int speed;
    public float amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector2 startingPos;

    void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (amount > 0)
        {
            amount -= Time.deltaTime * 5;
            transform.position = new Vector3(startingPos.x + Mathf.Sin((Time.time * speed) * amount)*amount, startingPos.y + (Mathf.Sin((Time.time * speed) * amount)), 0);
        }
    }


    public void SpawnStones(int _value)
    {
        if (UiManager.instance.rockSmashPlayground.activeSelf)
        {
            var tmp = Instantiate(stoneValues[_value], this.transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)), this.transform.parent.transform);
            tmp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-90, 90), 90) * 5, ForceMode2D.Impulse);
        }
    }
}
