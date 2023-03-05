using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int _day = 0;
    public List<string> bodyParts = new List<string>();
    [SerializeField] private bool _gamePaused = false;

    public event Action<int> startDay;

    public event Action endDay;

    public event Action<bool> pauseDay;

    void Start()
    {
        StartDay();
    }

    public void EndDay()
    {
        Debug.Log("Ending Day");

        endDay?.Invoke();
        if (_day < 6)
            _day++;
    }

    public void StartDay()
    {
        Debug.Log($"Starting Day {_day}");
        startDay.Invoke(_day);
    }

    public void PauseDay()
    {
        _gamePaused = !_gamePaused;
        pauseDay.Invoke(_gamePaused);
    }

    public void OverrideDay()
    {
        _day = 1;
    }

    public int GetDay()
    {
        return _day;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.name == "Player")
        {
            PlayerInventory inv = collision.gameObject.GetComponent<PlayerInventory>();
            string inHand = inv.GetInHand();

            if(!string.IsNullOrEmpty(inHand))
            {
                bodyParts.Add(inHand);
                EndDay();
            }
        }
    }
}
