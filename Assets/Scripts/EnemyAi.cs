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

    public Faction faction = Faction.Enemy;

    public GameObject target;
    public float moveSpeed;
    public float HP;
    public float maxHP;

    private Rigidbody2D rBody = null;

    private Vector2 forward;

	// Use this for initialization
	void Start () 
    {
        rBody = this.GetComponent<Rigidbody2D>();
        forward = Vector2.up;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Move();
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

    void Move()
    {
        forward = target.transform.position - this.transform.position;

        switch (EnemyAIType)
        {
            case AIType.CHASER:
                //this.transform.Rotate(Vector3.forward, Vector2.Angle(forward, target.transform.position - this.transform.position));
                //this.transform.Rotate(Vector3.forward, Vector2.Angle(Vector2.up, forward));
                //forward = target.transform.position - this.transform.position;

                rBody.velocity = forward.normalized * moveSpeed;
                break;
        }
    }
}
