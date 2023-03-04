using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : State
{
    private float timeCount;

    public LookAround(GuardMachine guardMachine) : base(guardMachine)
    {
        timeCount = 0.0f;
    }

    public override IEnumerator OnEnter()
    {
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }

    public override IEnumerator OnUpdate()
    {
        if (this.guardMachine.currentTargetRotation == null) return base.OnUpdate();

        this.guardMachine.transform.rotation = Quaternion.Lerp(this.guardMachine.transform.rotation, Quaternion.AngleAxis(this.guardMachine.currentTargetRotation.Value, Vector3.forward), timeCount);
        timeCount += Time.deltaTime;
        
        return base.OnUpdate();
    }
}
