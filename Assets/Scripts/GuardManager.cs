using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuardDay
{
    [SerializeField]
    public GameObject guard;
    [SerializeField]
    public int dayToTurnOn;
}

public class GuardManager : MonoBehaviour
{
    [SerializeField]
    public List<GuardDay> guardDays;

    [SerializeField]
    public LevelManager levelManager;

    private void Start()
    {
        foreach (GuardDay guardDay in guardDays)
        {
            guardDay.guard.SetActive(false);
        }

        levelManager.startDay += UpdateEnabledGuards;
    }

    void UpdateEnabledGuards(int day)
    {
        foreach (GuardDay guardDay in guardDays)
        {
            if (guardDay.dayToTurnOn == day)
            {
                guardDay.guard.SetActive(true);
            }
        }
    }

}
