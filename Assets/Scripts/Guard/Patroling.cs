using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroling : State
{
    private float timeCount;

    public Patroling(GuardMachine guardMachine) : base(guardMachine)
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
        if (this.guardMachine.currentTargetPosition != null)
        {
            this.guardMachine.transform.position = Vector2.MoveTowards(this.guardMachine.transform.position, (Vector3)this.guardMachine.currentTargetPosition.Value, Time.deltaTime);

            if (timeCount <= 1)
            {
                Vector3 dir = this.guardMachine.currentTargetPosition.Value - this.guardMachine.transform.position;
                float angle = Vector3.Angle(dir, this.guardMachine.transform.right);
                if (this.guardMachine.currentTargetPosition.Value.y < this.guardMachine.transform.position.y) angle *= -1;
                if (angle > 180) angle -= 360;
                float currRot = this.guardMachine.currentRotation;
                if (currRot > 180) currRot -= 360;

                this.guardMachine.currentRotation = Mathf.Lerp(currRot, angle, timeCount);
                timeCount += Time.deltaTime;
            }
        }
        return base.OnUpdate();
    }


}
