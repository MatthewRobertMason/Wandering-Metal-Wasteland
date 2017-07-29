using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour 
{
    public Faction faction = Faction.Player;
    public Transform bulletSpawn = null;
    public float turnSpeed = 5.0f;
    public float moveSpeed = 1.0f;

    private Vector2 forward = Vector2.up;
    
    public GameObject prefabBullet = null;
    public int maxBullets = 5;
    public float bulletSpeed = 5.0f;
    public AudioClip bulletSound = null;
    private Queue<GameObject> bullets;

    private Rigidbody2D rBody = null;
    private AudioSource aSource = null;

    public float HP = 10;
    public float maxHP = 10;

	// Use this for initialization
	void Start () 
    {
        rBody = this.GetComponent<Rigidbody2D>();
        aSource = this.GetComponent<AudioSource>();
        bullets = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetControls();
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        CollideDamage collideDamage = collision.gameObject.GetComponent<CollideDamage>();
        if (collideDamage != null)
            TakeDamage(collideDamage.damage, collideDamage.faction);
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        CollideDamage collideDamage = collider.gameObject.GetComponent<CollideDamage>();
        if (collideDamage != null)
            TakeDamage(collideDamage.damage, collideDamage.faction);
    }

    void TakeDamage(float collisionDamage, Faction collisionFaction)
    {
        if (collisionFaction != faction)
        {
            HP -= collisionDamage;

            if (HP <= 0.0f)
            {
                // Explode
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter()
    {
    }

    void GetControls()
    {
        float velocity = Input.GetAxis("Vertical");
        float turnVelocity = -Input.GetAxis("Horizontal");

        forward = this.transform.up;

        if (turnVelocity == 0.0f)
        {
            rBody.velocity = forward.normalized * velocity * moveSpeed;
        }
        else
        {
            rBody.velocity = Vector2.zero;
        }

        if (rBody.velocity == Vector2.zero)
        {
            //this.transform.Rotate(0.0f, 0.0f, turnVelocity * turnSpeed);
            this.transform.Rotate(Vector3.forward, turnVelocity * turnSpeed);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // Play Sound
            aSource.PlayOneShot(bulletSound);

            // Remove furthest bullet
            while (bullets.Count >= maxBullets)
            {
                GameObject tempBullet = null;

                if (bullets.Count > 0)
                    tempBullet = bullets.Dequeue();

                if (tempBullet != null)
                    Destroy(tempBullet);
            }

            GameObject newBullet = Instantiate(prefabBullet, bulletSpawn.position, Quaternion.Euler(Vector3.zero));
            newBullet.GetComponent<Rigidbody2D>().velocity = forward.normalized * bulletSpeed;
            Destroy(newBullet, 1);
            bullets.Enqueue(newBullet);
        }
    }
}
