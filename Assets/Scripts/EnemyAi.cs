using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour 
{
    public enum AIType
    {
        CHASER,
        SHOOTER,
        CHASER_SHOOTER
    }
    public AIType EnemyAIType;

    private GameManager gameManager = null;

    public Faction faction = Faction.Enemy;

    public GameObject target;
    public float moveSpeed;
    public float HP;
    public float maxHP;
    public float detectionDistance = 10.0f;

    public float spitDelay = 3.0f;
    public float spitTimer = 0.0f;
    public float spitDistance = 5.0f;
    public float spitSpeed = 2.0f;
    public float spitLiftime = 3.0f;
    public GameObject spit = null;
    public GameObject spitSpawnLocation = null;

    public float spawnInvincibleTime = 1.0f;

    private Rigidbody2D rBody = null;

    private Vector2 forward;

    public AudioClip deathSound = null;

    private bool destroy = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

	// Use this for initialization
	void Start () 
    {
        target = gameManager.player;

        rBody = this.GetComponent<Rigidbody2D>();
        forward = Vector2.up;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (destroy)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (spawnInvincibleTime > 0.0f)
            {
                spawnInvincibleTime -= Time.deltaTime;
            }

            if (target == null)
            {
                target = FindObjectOfType<Player>().gameObject;
            }

            if (target != null)
            {
                if (Vector3.Distance(this.transform.position, target.transform.position) < detectionDistance)
                {
                    Move();
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (spawnInvincibleTime <= 0.0f)
        {
            CollideDamage collideDamage = collision.gameObject.GetComponent<CollideDamage>();
            if (collideDamage != null)
                TakeDamage(collideDamage.damage, collideDamage.faction);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (spawnInvincibleTime <= 0.0f)
        {
            CollideDamage collideDamage = collider.gameObject.GetComponent<CollideDamage>();
            if (collideDamage != null)
                TakeDamage(collideDamage.damage, collideDamage.faction);
        }
    }

    void TakeDamage(float collisionDamage, Faction collisionFaction)
    {
        if (collisionFaction != faction)
        {
            HP -= collisionDamage;

            if (HP <= 0.0f)
            {
                // Explode
                destroy = true;
                Destroy(this.GetComponent<Rigidbody2D>());
                Destroy(this.GetComponent<CapsuleCollider2D>());
                Destroy(this.GetComponent<CollideDamage>());
                Destroy(this.GetComponentInChildren<SpriteRenderer>());

                this.GetComponent<AudioSource>().PlayOneShot(deathSound);
            }
        }
        //rBody.velocity = (-1 * rBody.velocity.normalized) * 50;
    }

    void Move()
    {
        if (target != null)
        {
            forward = target.transform.position - this.transform.position;

            switch (EnemyAIType)
            {
                case AIType.CHASER:
                    this.transform.up = forward;

                    rBody.velocity = forward.normalized * moveSpeed;
                    break;

                case AIType.CHASER_SHOOTER:
                    spitTimer -= Time.deltaTime;
                    this.transform.up = forward;

                    if (Vector3.Distance(this.transform.position, target.transform.position) > spitDistance)
                    {
                        rBody.velocity = forward.normalized * moveSpeed;
                    }
                    else
                    {
                        rBody.velocity = Vector2.zero;

                        if (spitTimer <= 0.0f)
                        {
                            spitProjectile();
                            spitTimer = spitDelay;
                        }
                    }
                    break;
            }
        }
    }

    private void spitProjectile()
    {
        GameObject newSpit = Instantiate(spit, spitSpawnLocation.transform.position, Quaternion.Euler(Vector3.zero));
        newSpit.GetComponent<Rigidbody2D>().velocity = forward.normalized * spitSpeed;

        newSpit.transform.up = forward;

        Destroy(newSpit, spitLiftime);
    }
}
