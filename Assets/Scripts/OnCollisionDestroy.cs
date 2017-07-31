using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDestroy : MonoBehaviour 
{
    public Faction faction = Faction.Neutral;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Faction collisionFaction = this.faction;

        Player play = collision.GetComponent<Player>();
        if (play != null)
            collisionFaction = play.faction;

        
        EnemyAi enemy = collision.GetComponent<EnemyAi>();
        if (enemy != null)
            collisionFaction = enemy.faction;
        
        NeutralEntity neutral = collision.GetComponent<NeutralEntity>();
        if (neutral != null)
            collisionFaction = neutral.faction;
        
        if (collisionFaction != faction)
        {
            Destroy(this.gameObject);
        }
    }
}
