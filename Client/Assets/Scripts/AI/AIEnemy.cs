using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : FSMBehaviour
{
    // Use this for initialization
    void Start()
    {
        MakeFSM();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFSM();
    }

    protected override void MakeFSM()
    {
        base.MakeFSM();
        fsm = new FSMSystem();

        NpcState_Idle idle = new NpcState_Idle(ba, StateID.Idle);
        idle.AddTransition(StateID.Chase);
        idle.AddTransition(StateID.Death);

        NpcState_Chase chase = new NpcState_Chase(ba, StateID.Chase);
        chase.AddTransition(StateID.Idle);
        chase.AddTransition(StateID.Death);

        NpcState_Death death = new NpcState_Death(ba, StateID.Death);

        fsm.AddState(idle);
        fsm.AddState(chase);
        fsm.AddState(death);
    }
}
