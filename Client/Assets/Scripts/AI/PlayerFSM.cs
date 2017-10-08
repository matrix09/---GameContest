using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FSMBehaviour {

	// Use this for initialization
	void Start () {
        MakeFSM();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateFSM();
	}

    protected override void MakeFSM()
    {
        base.MakeFSM();
        fsm = new FSMSystem();

        PlayerState_SelfControl con = new PlayerState_SelfControl(ba, StateID.Idle);
        con.AddTransition(StateID.Injured);
        
        PlayerState_Injure injure = new PlayerState_Injure(ba, StateID.Injured);
       // injure.AddTransition(StateID.Injured);
        injure.AddTransition(StateID.Idle);

        fsm.AddState(con);
        fsm.AddState(injure);
    }

}
