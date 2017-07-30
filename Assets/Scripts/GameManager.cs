﻿using System.Collections;
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

    public GameObject gameCamera = null;

    public GameObject renderingCamera = null;
    public Text scoreText = null;
    public Image healthFill = null;
    public Image powerFill = null;

    public GameObject soundSource = null;
    public AudioClip passwordSound = null;
    public AudioClip menuButtonClickSound = null;

    public Canvas gameCanvas = null;
    public Canvas mainMenuCanvas = null;
    public Canvas menuCanvas = null;

    private float deadTimer = 3.0f;

    public GameObject mainMenuSoundSource = null;
    public GameObject mainMenuAudioListener = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            aSource = this.GetComponent<AudioSource>();

            if (audioClips.Length > 0)
            {
                aSource.clip = audioClips[0];
                currentAudioIndex = 0;
                aSource.Play();
            }
        }
        else
            Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () 
    {
        
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

        if (player != null)
        {
            updatePowerAndHealthBars();
        }

        if (playerDead())
        {
            deadTimer -= Time.deltaTime;

            if (deadTimer <= 0.0f)
            {
                deadTimer = 3.0f;
                gameCanvas.gameObject.SetActive(false);
                mainMenuCanvas.gameObject.SetActive(true);
                mainMenuSoundSource.SetActive(true);
                mainMenuAudioListener.SetActive(true);
                //LoadScene("MainMenu");
                LoadScene(0);
                gameCamera.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
                ChangeTrack(0);
            }
        }
	}

    public void NextTrack()
    {
        ChangeTrack((currentAudioIndex + 1) % audioClips.Length);        
    }

    public void PrevTrack()
    {
        ChangeTrack((currentAudioIndex - 1 + audioClips.Length) % audioClips.Length);
    }

    public void PauseTrack()
    {
        aSource.Pause();
    }

    public void PlayTrack()
    {
        aSource.Play();
    }

    public void ChangeTrack(int audioIndex)
    {
        currentAudioIndex = audioIndex;

        if ((audioClips.Length > 0) && (currentAudioIndex < audioClips.Length))
        {
            aSource.clip = audioClips[currentAudioIndex];
            aSource.Play();
        }
    }

    private void updatePowerAndHealthBars()
    {
        Player play = player.GetComponent<Player>();

        healthFill.transform.localScale = new Vector3(play.HP / play.maxHP, 1.0f, 1.0f);
        powerFill.transform.localScale = new Vector3(play.PP / play.maxPP, 1.0f, 1.0f);
    }

    private void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    private void LoadScene(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void PlayGame()
    {
        soundSource.GetComponent<AudioSource>().PlayOneShot(menuButtonClickSound);
        //load level
        //LoadScene("Level1");
        LoadScene(1);
        mainMenuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        mainMenuSoundSource.SetActive(false);
        mainMenuAudioListener.SetActive(false);
        ChangeTrack(1);
    }

    public void Password()
    {
        soundSource.GetComponent<AudioSource>().PlayOneShot(passwordSound);
    }

    public void Credits()
    {
        soundSource.GetComponent<AudioSource>().PlayOneShot(menuButtonClickSound);
    }

    private bool playerDead()
    {
        if (player != null)
        {
            Player play = player.GetComponent<Player>();

            if ((play.HP <= 0) || (play.PP <= 0))
            {
                return true;
            }
        }

        return false;
    }
}
