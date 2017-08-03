using System;
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

    public GameObject playerPrefab = null;
    public GameObject player = null;

    private AudioSource aSource = null;
    public AudioClip[] audioClips = null;
    private int currentAudioIndex = 0;

    public GameObject gameCamera = null;

    public GameObject renderingCamera = null;
    public Text scoreText = null;
    public Text zombiesText = null;
    public Text highScoreText = null;
    public Image healthFill = null;
    public Image powerFill = null;

    public Canvas credits = null;

    public GameObject soundSource = null;
    public AudioClip passwordSound = null;
    public AudioClip menuButtonClickSound = null;
    public AudioClip zoneSlayerAudio = null;

    public Canvas gameCanvas = null;
    public Canvas mainMenuCanvas = null;
    public Canvas menuCanvas = null;

    private float deadTimer = 3.0f;
    
    public GameObject mainMenuSoundSource = null;
    public GameObject mainMenuAudioListener = null;

    public static int gameScore = 0;

    public GameObject blackScreen = null;

    private string nextLevel = null;
    private float nextLevelTimer = 0.0f;
    public float nextLevelTimerMax = 3.0f;
    private bool changingLevel = false;

    private int highScore = 0;

    public bool wussMode = false;
    public GameObject wussModeActivate = null;
    public GameObject wussModeDeActivate = null;

    public int zonesSlain = 0;

    void Awake()
    {
        if (instance == null)
        {
            if (PlayerPrefs.HasKey("HighScore"))
            {
                highScore = PlayerPrefs.GetInt("HighScore");
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            aSource = this.GetComponent<AudioSource>();

            if (audioClips.Length > 0)
            {
                aSource.clip = audioClips[0];
                currentAudioIndex = 0;
                aSource.Play();
            }

            // For debuging
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                mainMenuCanvas.gameObject.SetActive(false);
                gameCanvas.gameObject.SetActive(true);
                mainMenuAudioListener.SetActive(false);
                ChangeTrack(1);
            }
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ((blackScreen.activeInHierarchy == true) && (nextLevel == null))
        {
            nextLevelTimer = 0.0f;
            changingLevel = false;
            blackScreen.SetActive(false);
        }

        if ((changingLevel) && (nextLevel != null))
        {
            if (nextLevelTimer == nextLevelTimerMax)
            {
                nextLevelTimer -= Time.deltaTime;
                blackScreen.SetActive(true);
            }
            else if ((nextLevelTimer > 0.0f) && (nextLevelTimer <= nextLevelTimerMax))
            {
                nextLevelTimer -= Time.deltaTime;
            }
            else
            {
                player.transform.position = Vector3.zero;
                LoadScene(nextLevel);
                nextLevel = null;
            }
        }

        string tempScore = String.Format("{0,8:00000000}", gameScore);
        scoreText.text = "Score: " + tempScore;

        string tempZombieCount = String.Format("{0,3:000}", CountZombies());
        zombiesText.text = "Zombies: " + tempZombieCount;

        string tempHighScore = String.Format("{0,8:00000000}", highScore);
        highScoreText.text = "Top Score: " + tempHighScore;

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

        if (player == null)
        {
            Player play = FindObjectOfType<Player>();
            if (play != null)
            {
                player = play.gameObject;
            }
        }

        if (playerDead())
        {
            deadTimer -= Time.deltaTime;

            if (deadTimer <= 0.0f)
            {
                if (gameScore > highScore)
                {
                    highScore = gameScore;
                    PlayerPrefs.SetInt("HighScore", highScore);
                }

                gameScore = 0;

                deadTimer = 3.0f;
                gameCanvas.gameObject.SetActive(false);
                mainMenuCanvas.gameObject.SetActive(true);
                mainMenuAudioListener.SetActive(true);
                gameCamera.GetComponent<CameraFollowPlayer>().player = null;
                ChangeTrack(0);
                //LoadScene(0);
                LoadScene("MainMenu");
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

    public void NextLevel(string levelName)
    {
        if (CountZombies() == 0)
        {
            nextLevel = levelName;
            changingLevel = true;
            nextLevelTimer = nextLevelTimerMax;
            soundSource.GetComponent<AudioSource>().PlayOneShot(zoneSlayerAudio);
        }
        else
        {
            aSource.PlayOneShot(passwordSound);
        }
    }

    public void NextLevel(int levelId)
    {
        if (CountZombies() == 0)
        {
            nextLevel = SceneManager.GetSceneAt(levelId).name;
            changingLevel = true;
            nextLevelTimer = nextLevelTimerMax;
            soundSource.GetComponent<AudioSource>().PlayOneShot(zoneSlayerAudio);
        }
        else
        {
            aSource.PlayOneShot(passwordSound);
        }
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
        player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        LoadScene("Level1");
        //LoadScene(1);
        mainMenuCanvas.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
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
        credits.gameObject.SetActive(!credits.gameObject.activeInHierarchy);
    }

    public void WussMode()
    {
        wussMode = !wussMode;

        if (wussMode)
        {
            wussModeActivate.SetActive(true);
            wussModeDeActivate.SetActive(false);
        }
        else
        {
            wussModeActivate.SetActive(false);
            wussModeDeActivate.SetActive(true);
        }
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

    public void addScore(int points)
    {
        if (!playerDead())
        {
            gameScore += (int)((float)points * (zonesSlain * 0.1f));
        }
    }

    public void playTitle()
    {
        soundSource.GetComponent<AudioSource>().PlayOneShot(zoneSlayerAudio);
    }

    public int CountZombies()
    {
        EnemyAi[] enemies = FindObjectsOfType<EnemyAi>();
        if (enemies != null)
            return enemies.Length;

        return 0;
    }
}

