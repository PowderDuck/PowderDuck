using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager manager;

    [Header("Sounds\n")]
    public AudioClip soundtrack;
    public AudioClip uiClick;
    public AudioClip exitUIClick;
    public AudioClip exitGame;
    public AudioClip combo;
    public List<AudioClip> extraComboSounds = new List<AudioClip>();
    public AudioClip[] gameModes = new AudioClip[2];
    public AudioClip gameOver;
    public AudioClip bombBoost;
    public AudioClip reductionBoost;
    public AudioClip multiplierBoost;
    public AudioClip tileSelection;

    [Header("AudioSources\n")]
    [SerializeField] private List<AudioSource> sources = new List<AudioSource>();
    [SerializeField] private Vector2 soundtrackPitch = new Vector2(0.7f, 1f);

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlaySound(soundtrack, 2);
    }

    private void Update()
    {
        float pitch = MapHandler.handler.isPlaying ? soundtrackPitch.y : soundtrackPitch.x;
        sources[sources.Count - 1].pitch = pitch;
    }

    public void PlaySound(AudioClip clip, int sourceIndex, float soundPitch = 1f)
    {
        if(clip)
        {
            sources[sourceIndex].clip = clip;
            sources[sourceIndex].pitch = soundPitch;
            sources[sourceIndex].Play();
        }
    }
}
