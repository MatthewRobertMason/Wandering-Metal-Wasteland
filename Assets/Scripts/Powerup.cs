using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Powerup : MonoBehaviour 
{
    public float powerChange = 0.0f;
    public float healthChange = 0.0f;
    public float moveSpeedIncrease = 0.0f;
    public float turnSpeedIncrease = 0.0f;
    public float bulletSpeedIncrease = 0.0f;
    public float damageIncerease = 0.0f;
    public float maximumLifeIncrease = 0.0f;
    public float maximumPowerIncrease = 0.0f;

    public AudioClip pickupSound = null;

    private bool destroy = false;
    private GameManager gameManager = null;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (destroy)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        
        Player player = gameManager.player.GetComponent<Player>();

        player.maxHP += maximumLifeIncrease;
        player.maxPP += maximumPowerIncrease;

        player.moveSpeed += moveSpeedIncrease;
        player.bulletSpeed += bulletSpeedIncrease;
        player.turnSpeed += turnSpeedIncrease;

        player.HP = Mathf.Min(player.HP + healthChange, player.maxHP);
        player.PP = Mathf.Min(player.PP + powerChange, player.maxPP);

        destroy = true;
        Destroy(this.GetComponent<SpriteRenderer>());
        Destroy(this.GetComponent<BoxCollider2D>());
    }
}
