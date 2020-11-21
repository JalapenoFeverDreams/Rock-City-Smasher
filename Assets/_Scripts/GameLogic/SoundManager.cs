using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("RockSmash")]
    public AudioSource rocksmashSource;
    public List<AudioClip> rockSmash;

    [Header("StoneDrop")]
    public AudioSource stondeDropSource;
    public List<AudioClip> stoneDrop;

    [Header("Button")]
    public AudioSource buttonSource;
    public List<AudioClip> buttonDrop;

    [Header("Buy Success")]
    public AudioSource buySuccessSource;
    public List<AudioClip> buy;

    [Header("Buy Fail")]
    public AudioSource buyFailSource;
    public List<AudioClip> buyFail;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rocksmash()
    {
        if (UiManager.instance.rocks.activeSelf)
        {
            var clip = rockSmash[Random.Range(0, rockSmash.Count - 1)];
            rocksmashSource.clip = clip;
            rocksmashSource.Play();
        }
    }

    public void StoneDrop()
    {
        if (UiManager.instance.rocks.activeSelf)
        {
            var clip = stoneDrop[Random.Range(0, stoneDrop.Count - 1)];
            stondeDropSource.clip = clip;
            stondeDropSource.Play();
        }
    }

    public void ButtonSound()
    {
        var clip = buttonDrop[Random.Range(0, buttonDrop.Count - 1)];
        buttonSource.clip = clip;
        buttonSource.Play();
    }

    public void BuySuccess()
    {
        var clip = buy[Random.Range(0, buy.Count - 1)];
        buySuccessSource.clip = clip;
        buySuccessSource.Play();
    }

    public void BuyFail()
    {
        var clip = buyFail[Random.Range(0, buy.Count - 1)];
        buyFailSource.clip = clip;
        buyFailSource.Play();
    }
}
