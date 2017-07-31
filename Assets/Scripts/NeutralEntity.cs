using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEntity : MonoBehaviour 
{
    public Faction faction = Faction.Neutral;

    public float HP;
    public float maxHP;

    public GameObject[] loot = null;

    //private GameManager gameManager = null;

    private bool destroy = false;

    public float spawnInvincibleTime = 1.0f;

	// Use this for initialization
	void Awake () 
    {
        //gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (spawnInvincibleTime > 0.0f)
        {
            spawnInvincibleTime -= Time.deltaTime;
        }

        if (destroy)
        {
            //if (!this.GetComponent<AudioSource>().isPlaying)
            //{
                Destroy(this.gameObject);
            //}
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
        HP -= collisionDamage;

        if (HP <= 0.0f)
        {
            // Explode
            DropLoot();
            destroy = true;
            Destroy(this.GetComponent<Collider>());
            Destroy(this.GetComponent<SpriteRenderer>());
            Destroy(this.GetComponent<Rigidbody2D>());
        }
    }

    void DropLoot()
    {
        if ((loot != null) && (loot.Length > 0))
        {
            int randomLoot = Random.Range(0, loot.Length);

            if (loot[randomLoot] != null)
            {
                Instantiate(loot[randomLoot], this.transform.position, Quaternion.Euler(Vector3.zero));
            }
        }
    }
}
