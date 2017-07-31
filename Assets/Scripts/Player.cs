using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour 
{
    public enum AnimationState
    {
        STILL,
        WALKING,
        SHOOTING,
        ATTACKING
    }
    private AnimationState animationState = AnimationState.STILL;

    private GameManager gameManager = null;

    public Faction faction = Faction.Player;
    public Transform bulletSpawn = null;
    public float turnSpeed = 5.0f;
    public float moveSpeed = 1.0f;

    private Vector2 forward = Vector2.up;
    
    public GameObject prefabBullet = null;
    public int maxBullets = 5;
    public float bulletSpeed = 5.0f;
    public AudioClip bulletSound = null;
    public AudioClip meleeSound = null;
    public AudioClip playerDead = null;
    public AudioClip playerPowerDown = null;

    private Queue<GameObject> bullets;

    private Rigidbody2D rBody = null;
    private AudioSource aSource = null;

    public float HP = 10;
    public float maxHP = 10;

    public float PP = 10;
    public float maxPP = 10;
    public float powerUsagePerSecond = 0.5f;

    public GameObject stillAnimation = null;
    public GameObject walkAnimation = null;
    public GameObject shootAnimation = null;
    public GameObject attackAnimation = null;

    private bool attacking = false;

    public GameObject chainsawSpawn = null;
    public GameObject chainsawCollider = null;
    public float chainsawDuration = 0.75f;

    private bool dead = false;

    public float gunPowerUsage = 2.0f;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.player = this.gameObject;

        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start () 
    {
        if (stillAnimation == null ||
            walkAnimation == null  ||
            shootAnimation == null || 
            attackAnimation == null )
        {
            Application.Quit();
        }

        rBody = this.GetComponent<Rigidbody2D>();
        aSource = this.GetComponent<AudioSource>();
        bullets = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (dead)
        {
            animationState = AnimationState.STILL;
            rBody.velocity = Vector2.zero;
            return;
        }

        if (animationState == AnimationState.SHOOTING || animationState == AnimationState.ATTACKING)
        {
            shootingTest();
            meleeTest();

            if (!isShooting() && !isMeleeAttacking())
            {
                // Clean the bullet queue
                while ((bullets.Count > 0) && (bullets.Peek() == null))
                {
                    bullets.Dequeue();
                }

                // Reset to still animation
                animationState = AnimationState.STILL;
                GetControls();
            }
        }
        else
        {
            GetControls();
        }

        losePower();
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

            if ((HP <= 0.0f) && (!dead))
            {
                // Explode
                dead = true;
                aSource.PlayOneShot(playerDead);
                Destroy(attackAnimation);
                Destroy(shootAnimation);
                Destroy(walkAnimation);
                Destroy(stillAnimation);
            }
        }
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

        if (animationState == AnimationState.SHOOTING || animationState == AnimationState.ATTACKING)
        {
            if (!isShooting() && !isMeleeAttacking())
            {
                rBody.velocity = Vector2.zero;
            }
        }
        else if (attacking) 
        {
            rBody.velocity = Vector2.zero;
        }

        if (rBody.velocity.magnitude > 0.0f)
        {
            animationState = AnimationState.WALKING;
        }
        else
        {
            animationState = AnimationState.STILL;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animationState = AnimationState.SHOOTING;
            rBody.velocity = Vector2.zero;
            //attacking = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animationState = AnimationState.ATTACKING;
            rBody.velocity = Vector2.zero;
            //attacking = true;
        }

        switch (animationState)
        {
            case AnimationState.STILL:
                attacking = false;
                //chainsawCollider.SetActive(false);
                stillAnimation.SetActive(true);
                walkAnimation.SetActive(false);
                shootAnimation.SetActive(false);
                attackAnimation.SetActive(false);
                break;
            case AnimationState.WALKING:
                attacking = false;
                //chainsawCollider.SetActive(false);
                stillAnimation.SetActive(false);
                walkAnimation.SetActive(true);
                shootAnimation.SetActive(false);
                attackAnimation.SetActive(false);
                break;
            case AnimationState.SHOOTING:
                stillAnimation.SetActive(false);
                walkAnimation.SetActive(false);
                shootAnimation.SetActive(true);
                attackAnimation.SetActive(false);
                break;
            case AnimationState.ATTACKING:
                stillAnimation.SetActive(false);
                walkAnimation.SetActive(false);
                shootAnimation.SetActive(false);
                attackAnimation.SetActive(true);
                break;

            default:
                stillAnimation.SetActive(true);
                walkAnimation.SetActive(false);
                break;
        }
    }

    private void shootingTest()
    {
        if ((shootAnimation.activeInHierarchy) && (shootAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f))
        {
            if ((!attacking) && ((bullets.Count == 0) || (bullets.Peek() == null)) && (PP > gunPowerUsage))
            {
                aSource.PlayOneShot(bulletSound);
                attacking = true;

                PP -= gunPowerUsage;
                GameObject newBullet = Instantiate(prefabBullet, bulletSpawn.position, Quaternion.Euler(Vector3.zero));
                newBullet.GetComponent<Rigidbody2D>().velocity = forward.normalized * bulletSpeed;
                Destroy(newBullet, 1);
                bullets.Enqueue(newBullet);
            }
        }
    }

    private void meleeTest()
    {
        if ((attackAnimation.activeInHierarchy) && (attackAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f))
        {
            if (!attacking)
            {
                aSource.PlayOneShot(meleeSound);
                attacking = true;

                chainsawCollider.SetActive(true);
                Destroy(Instantiate(chainsawCollider, chainsawSpawn.transform.position, Quaternion.Euler(Vector3.zero)), chainsawDuration);
            }
        }
    }

    private bool isShooting()
    {
        return ((shootAnimation.activeInHierarchy) && (shootAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f));
    }

    private bool isMeleeAttacking()
    {
        return ((attackAnimation.activeInHierarchy) && (attackAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f));
    }

    private void losePower()
    {
        PP -= (powerUsagePerSecond * Time.deltaTime);

        if (PP <= 0.0f)
        {
            dead = true;
            aSource.PlayOneShot(playerPowerDown);
            animationState = AnimationState.STILL;
        }
    }
}
