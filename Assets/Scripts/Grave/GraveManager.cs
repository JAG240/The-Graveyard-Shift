using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] private List<GraveData> graveData = new List<GraveData>();

    private Grave[] _graves;
    private LevelManager _levelManager;

    void Start()
    {
        _graves = GetComponentsInChildren<Grave>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        _levelManager.startDay += StartDay;
        _levelManager.endDay += EndDay;

        foreach(GameObject part in bodyParts)
        {
            AssignGrave:

            int grave = Random.Range(0, _graves.Length);

            if (_graves[grave].drop != null)
                goto AssignGrave;

            _graves[grave].drop = part;
        }

        EndDay();
    }

    private void StartDay(int day)
    {
        foreach(Grave grave in _graves)
        {
            foreach(GraveData data in graveData)
            {
                if(grave == data.grave)
                {
                    grave.drop = data.drop;
                    grave.SetDug(data.isDug);
                }
            }
        }
    }

    private void EndDay()
    {
        graveData.Clear();

        foreach (Grave grave in _graves)
        {
            GraveData newData = new GraveData();
            newData.grave = grave;
            newData.drop = grave.drop;
            newData.isDug = grave.IsDug();
            graveData.Add(newData);
        }
    }
}
