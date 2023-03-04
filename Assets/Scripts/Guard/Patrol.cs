using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Patrol
{
    [SerializeField]
    public List<Vector3> goToPoints = new List<Vector3>();
    [SerializeField]
    public List<float> lookAtPoints = new List<float>();
}
