using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : State
{
    private float timeCount;

    public LookAround(GuardMachine guardMachine) : base(guardMachine)
    {  
    }

    public override IEnumerator OnEnter()
    {
        timeCount = 0.0f;
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }

    public override IEnumerator OnUpdate()
    {
        if (this.guardMachine.currentTargetRotation == null) return base.OnUpdate();

        float currentRotation = this.guardMachine.currentRotation;
        if (currentRotation <= 0)
        {
            currentRotation += 360;
        }

        this.guardMachine.currentRotation = Mathf.Lerp(currentRotation, this.guardMachine.currentTargetRotation.Value, timeCount);
        timeCount += Time.deltaTime;
        
        if (timeCount >= 1)
        {
            this.guardMachine.nextLook();
        }

        return base.OnUpdate();
    }
}
