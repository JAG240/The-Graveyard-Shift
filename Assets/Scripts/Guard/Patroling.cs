using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroling : State
{
    public Patroling(GuardMachine guardMachine) : base(guardMachine)
    {
    }

    public override IEnumerator OnEnter()
    {
        this.guardMachine.transform.right = this.guardMachine.currentTargetPosition.Value - this.guardMachine.transform.position;
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }

    public override IEnumerator OnUpdate()
    {
        if (this.guardMachine.currentTargetPosition != null)
        {
            this.guardMachine.transform.position = Vector2.MoveTowards(this.guardMachine.transform.position, (Vector3)this.guardMachine.currentTargetPosition.Value, Time.deltaTime);
        }
        return base.OnUpdate();
    }
}
