using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManagerV2 : MonoBehaviour
{
    [SerializeField] List<GuardShift> guardShifts = new List<GuardShift>();

    private LevelManager _levelManager;

    void Start()
    {
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _levelManager.startDay += StartDay;
    }

    private void StartDay(int day)
    {
        foreach(GuardShift shift in guardShifts)
        {
            if(shift.day <= day)
                shift.guard.SetActive(true);
            else
                shift.guard.SetActive(false);
        }
    }
}
