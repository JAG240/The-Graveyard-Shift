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

    public Vector3 start;

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

        Debug.Log(currentPatrol.Next.Value);

        if (patrols.Count == 0) return;

        CurrentPatrolToLinkedList();

        start = transform.position;

        SetState(new Patroling(this));
    }

    void Update()
    {
        if (patrols.Count == 0) return;

        state.OnUpdate();

        if (state.GetType() == typeof(Patroling) 
            && currentTargetPosition != null
            && Vector3.Distance(transform.position, currentTargetPosition.Value) <= 0.1)
        {
            if (currentTargetPosition.Next == null)
            {
                SetState(new LookAround(this));
                return;
            }
    
            currentTargetPosition = currentTargetPosition.Next;
            
        }

        if (state.GetType() == typeof(LookAround) 
            && currentTargetRotation != null
            && Quaternion.AngleAxis(currentTargetRotation.Value, Vector3.forward) == transform.rotation)
        {
            if (currentTargetRotation.Next == null) 
            {
                currentPatrol = (currentPatrol == null) ? patrols.First : currentPatrol.Next;
                CurrentPatrolToLinkedList();

                SetState(new Patroling(this));
                return;
            }

            currentTargetRotation = currentTargetRotation.Next;
        }
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
