using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : State
{
    public Paused(GuardMachine guardMachine) : base(guardMachine)
    {
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
        return base.OnUpdate();
    }
}
