using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    private Rigidbody2D rBody = null;

    private Vector2 forward;

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
        rBody.velocity = (-1 * rBody.velocity.normalized) * 50;
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
            }
        }
    }
}
