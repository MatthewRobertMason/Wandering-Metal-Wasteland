using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDamage : MonoBehaviour 
{
    public Faction faction = Faction.Neutral;
    public float damage = 1.0f;

    private GameManager gameManager = null;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public float Damage
    {
        get 
        {
            if (faction == Faction.Player)
            {
                Player player = gameManager.player.GetComponent<Player>();
                if (player != null)
                {
                    return damage + player.damageBonus;
                }
            }

            return damage;
        }
    }
}
