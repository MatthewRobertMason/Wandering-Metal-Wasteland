using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Faction
{
    Player,
    Enemy,
    Neutral
}

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour 
{
    private static GameManager instance = null;
    public GameObject player = null;

    private AudioSource aSource = null;
    public AudioClip[] audioClips = null;
    private int currentAudioIndex = 0;

    public GameObject renderingCamera = null;
    public Text scoreText = null;
    public Image healthFill = null;
    public Image powerFill = null;

	// Use this for initialization
	void Start () 
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);

        aSource = this.GetComponent<AudioSource>();

        if (audioClips.Length > 0)
        {
            aSource.clip = audioClips[0];
            currentAudioIndex = 0;
            aSource.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (aSource.isPlaying == false)
        {
            if (audioClips.Length > 0)
            {
                currentAudioIndex = (currentAudioIndex++) % audioClips.Length;
                aSource.clip = audioClips[currentAudioIndex];
                aSource.Play();
            }
        }

        updatePowerAndHealthBars();
	}

    void NextTrack()
    {
        ChangeTrack((currentAudioIndex + 1) % audioClips.Length);        
    }

    void PrevTrack()
    {
        ChangeTrack((currentAudioIndex - 1 + audioClips.Length) % audioClips.Length);
    }

    void PauseTrack()
    {
        aSource.Pause();
    }

    void PlayTrack()
    {
        aSource.Play();
    }

    void ChangeTrack(int audioIndex)
    {
        currentAudioIndex = audioIndex;

        if ((audioClips.Length > 0) && (currentAudioIndex < audioClips.Length))
        {
            aSource.clip = audioClips[currentAudioIndex];
            aSource.Play();
        }
    }

    void updatePowerAndHealthBars()
    {
        Player play = player.GetComponent<Player>();

        healthFill.transform.localScale = new Vector3(play.HP / play.maxHP, 1.0f, 1.0f);
        powerFill.transform.localScale = new Vector3(play.PP / play.maxPP, 1.0f, 1.0f);
    }
}
