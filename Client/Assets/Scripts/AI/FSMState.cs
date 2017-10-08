using UnityEngine;
using System.Collections.Generic;

public enum StateID {
    NullStateID = 0, // Use this ID to represent a non-existing State in your system	
    Birth,             //出生
    Idle,              //待机
    RandomRun,
    Chase,//追踪玩家
    Attacking,//攻击
    Injured,                //受伤
    InjuredBack,        //受伤位移-水平
    InjuredDown,        //受伤位移-砸地下落
    Death,              //死亡
}

public abstract class FSMState {
    protected CameraController CC;
    protected List<StateID> outputStates = new List<StateID>();
    protected StateID stateID;
    protected BaseActor Owner;
    public StateID ID {
        get {
            return stateID;
        }
    }

    public FSMState(BaseActor owner, StateID id)
    {
        Owner = owner;
        stateID = id;
    }

    public void AddTransition(StateID trans) {
        // Check if anyone of the args is invalid
        if (trans == StateID.NullStateID) {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (IsHaveTransition(trans)) {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        outputStates.Add(trans);
    }

    public void DeleteTransition(StateID trans) {
        // Check for NullTransition
        if (trans == StateID.NullStateID) {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (IsHaveTransition(trans)) {
            outputStates.Remove(trans);
            return;
        }

        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    public virtual void DoBeforeEntering(BaseActor target) {

    }

    public virtual void DoBeforeLeaving(BaseActor target) {

    }

    public virtual void Reason(BaseActor target) { }

    public virtual void Act(BaseActor target) { }

    //判断是否可执行某一转换
    public bool IsHaveTransition(StateID trans) {
        return outputStates.Contains(trans);
    }
}
