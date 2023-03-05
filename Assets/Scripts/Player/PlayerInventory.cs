using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private string _inHand;
    private PlayerController _playerController;
    private LevelManager _levelManager;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _levelManager.endDay += EndDay;
        _levelManager.startDay += StartDay;
    }

    public bool AddToHand(string part)
    {
        if (!string.IsNullOrEmpty(_inHand))
            return false;

        _inHand = part;

        _playerController.SwingDisabled(true);

        return true;
    }

    private void EndDay()
    {
        _inHand = null;
        _playerController.SwingDisabled(false);
    }

    private void StartDay(int day)
    {
        EndDay();
    }

    public string GetInHand()
    {
        if (!string.IsNullOrEmpty(_inHand))
            return _inHand;

        return null;
    }
}
