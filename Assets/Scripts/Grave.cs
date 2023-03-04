using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour, IHitable
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject drop;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Hit(int damage)
    {
        int dmgTaken = health - damage >= 0 ? damage : health;

        health -= dmgTaken;

        UpdateSprite();

        if(health == 0 && drop)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

    private void UpdateSprite()
    {
        float t = (float)health / (float)maxHealth;

        _renderer.color = new Color(t, t, t);
    }
}
