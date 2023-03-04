using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject _cam;
    private GameObject _player; 

    private void Start()
    {
        _cam = Camera.main.gameObject;
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        _cam.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, _cam.transform.position.z);
    }
}
