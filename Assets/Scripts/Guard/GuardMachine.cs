using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMachine : MonoBehaviour
{
    [SerializeField]
    public State state;
    [SerializeField]
    List<Patrol> inputPatrols = new List<Patrol>();

    LinkedList<Patrol> patrols;
    LinkedList<Vector3> goToPositions;
    LinkedList<float> lookAtRotations;

    LinkedListNode<Patrol> currentPatrol;
    public LinkedListNode<Vector3> currentTargetPosition;
    public LinkedListNode<float> currentTargetRotation;
    public float currentRotation;

    public Vector3 start;

    public float angle = 50.0f;
    public float rayRange = 6.0f;

    [SerializeField]
    public GameObject visionIndicator;
    [SerializeField]
    public Vector3 offset;

    public void SetState(State state)
    {
        if (this.state != null)
        {
            this.state.OnExit();
        }

        this.state = state;
        this.state.OnEnter();
    }

    void Start()
    {
        patrols = new LinkedList<Patrol>(inputPatrols);
        currentPatrol = patrols.First;

        if (patrols.Count == 0) return;

        CurrentPatrolToLinkedList();

        start = transform.position;

        SetState(new Patroling(this));
    }

    void Update()
    {
        visionIndicator.transform.rotation = Quaternion.Euler(0, 0, currentRotation - 45);

        if (patrols.Count == 0) return;

        state.OnUpdate();

        if (state.GetType() == typeof(Patroling) 
            && currentTargetPosition != null
            && Vector3.Distance(transform.position, currentTargetPosition.Value) <= 0.1)
        {

            nextPosition();
        }

        if (state.GetType() == typeof(LookAround) && currentTargetRotation == null)
        {
            nextLook();
        }
    }

    public void nextPosition()
    {
        if (currentTargetPosition.Next == null)
        {

            if (currentTargetRotation == null)
            {
                nextPatrol();
                return;
            }

            SetState(new LookAround(this));
            return;
        }

        state.OnEnter();
        currentTargetPosition = currentTargetPosition.Next;
    }

    public void nextLook()
    {
        if (currentTargetRotation.Next == null)
        {
            nextPatrol();
            CurrentPatrolToLinkedList();

            SetState(new Patroling(this));
            return;
        }

        currentTargetRotation = currentTargetRotation.Next;
    }

    public void nextPatrol() 
    {
        currentPatrol = (currentPatrol.Next == null) ? patrols.First : currentPatrol.Next;
        CurrentPatrolToLinkedList();
    }

    void CurrentPatrolToLinkedList()
    {
        if (currentPatrol == null || currentPatrol.Value == null) return;

        if (currentPatrol.Value.goToPoints.Count > 0)
        {
            goToPositions = new LinkedList<Vector3>(currentPatrol.Value.goToPoints);
            currentTargetPosition = goToPositions.First;
        }
        if (currentPatrol.Value.lookAtPoints.Count > 0)
        {
            lookAtRotations = new LinkedList<float>(currentPatrol.Value.lookAtPoints);
            currentTargetRotation = lookAtRotations.First;
        }
    }

    private void OnDrawGizmosSelected()
    {
        /* vision debugging
         * Vector3 dir = Quaternion.AngleAxis(currentRotation, Vector3.forward) * Vector3.right;
        Gizmos.DrawLine(transform.position + dir + offset, transform.position + (dir * 2) + offset);*/

        if (start == Vector3.zero) start = transform.position; 

        Vector3 prevPoint = start;
        foreach (Patrol patrol in inputPatrols)
        {
            if (patrol == null) return;

            foreach(Vector3 goToPoint in patrol.goToPoints)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(prevPoint, goToPoint);
                
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(goToPoint, 0.2f);

                prevPoint = goToPoint;
            }
            foreach(float lookAtPoint in patrol.lookAtPoints)
            {
                Vector3 dir = Quaternion.AngleAxis(lookAtPoint, Vector3.forward) * Vector3.right;
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(prevPoint + dir, 0.2f);

            }
        }
    }

}
