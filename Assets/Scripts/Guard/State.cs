using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{

    protected GuardMachine guardMachine;

    public State(GuardMachine guardMachine)
    {
        this.guardMachine = guardMachine;
    }

    public virtual IEnumerator OnEnter()
    {
        yield break;
    }

    public virtual IEnumerator OnUpdate()
    {
        yield break;
    }

    public virtual IEnumerator OnExit()
    {
        yield break;
    }
}
