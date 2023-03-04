using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Detection : MonoBehaviour
{
    [SerializeField] public float radius = 20f;
    [SerializeField] float detectionTimer = 0.2f;
    [SerializeField] public float detectionAngle = 90;
    [SerializeField] public LayerMask detectionMasks;

    [SerializeField] public GuardMachine guardMachine;

    Collider2D thing;

    Vector3 offsetPosition;

    private void Start()
    {
        StartCoroutine(CheckForFood());
    }

    private void Update()
    {
        offsetPosition = transform.position + guardMachine.offset;
    }

    private void OnDrawGizmosSelected()
    {
        /* Vision Debugging
         * Vector3 dir = Quaternion.AngleAxis(guardMachine.currentRotation + (detectionAngle / 2), Vector3.forward) * Vector3.right;
        Gizmos.DrawLine(offsetPosition + dir, offsetPosition + (dir * 3));

        Vector3 dir2 = Quaternion.AngleAxis(guardMachine.currentRotation - (detectionAngle / 2), Vector3.forward) * Vector3.right;
        Gizmos.DrawLine(offsetPosition + dir2, offsetPosition + (dir2 * 3));

        if (thing != null) 
            Gizmos.DrawLine(offsetPosition, thing.transform.position);*/
    }

    private IEnumerator CheckForFood()
    {
        Vector3 direction;
        RaycastHit2D hit;
        float angle;


        while (true)
        {
            yield return new WaitForSeconds(detectionTimer);

            thing = Physics2D.OverlapCircle(offsetPosition, radius, detectionMasks);
            if (thing)
            {
                direction = thing.transform.position - offsetPosition;
                angle = Vector3.Angle(direction, transform.right);
                if (thing.transform.position.y < offsetPosition.y) angle = 360 - angle;

                if (angle > guardMachine.currentRotation + (detectionAngle / 2) || angle < guardMachine.currentRotation - (detectionAngle / 2))
                    continue;

                hit = Physics2D.Raycast(offsetPosition, direction);
                if (hit && hit.transform.gameObject == thing.gameObject)
                {
                    Debug.Log("Detected: " + thing);
                }
            }
        }
    }
}