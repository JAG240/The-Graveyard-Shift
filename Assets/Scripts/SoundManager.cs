using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioClip swing;
    [SerializeField] private AudioClip pickup;

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    private AudioSource _music;
    private AudioSource _SFX;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _music = transform.Find("Music").GetComponent<AudioSource>();
        _SFX = transform.Find("SFX").GetComponent<AudioSource>();
    }

    public void PlayGameMusic()
    {
        _music.Stop();
        _music.clip = gameMusic;
        _music.Play();
    }

    public void PlayFootStep()
    {
        _SFX.Stop();
        _SFX.clip = footstep;
        _SFX.Play();
    }

    public void PlaySwing()
    {
        _SFX.Stop();
        _SFX.clip = swing;
        _SFX.Play();
    }

    public void PlayPickup()
    {
        _SFX.Stop();
        _SFX.clip = pickup;
        _SFX.Play();
    }
}
