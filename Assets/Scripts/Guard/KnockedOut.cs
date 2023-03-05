using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedOut : State
{
    public KnockedOut(GuardMachine guardMachine) : base(guardMachine)
    {
    }

    public override IEnumerator OnEnter()
    {
        this.guardMachine.visionIndicator.SetActive(false);
        this.guardMachine.transform.rotation = new Quaternion(0, 0, 90, 0);
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        this.guardMachine.visionIndicator.SetActive(true);
        this.guardMachine.transform.rotation = new Quaternion(0, 0, 0, 0);
        return base.OnExit();
    }

    public override IEnumerator OnUpdate()
    {
        return base.OnUpdate();
    }
}
