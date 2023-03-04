using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour, IHitable
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int health = 1;
    public GameObject drop;

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
            drop = null;
        }
    }

    public bool IsDug()
    {
        return health == 0 ? true : false;
    }

    public void SetDug(bool state)
    {
        health = state ? 0 : maxHealth;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        float t = (float)health / (float)maxHealth;

        _renderer.color = new Color(t, t, t);
    }
}
