using System.Collections.Generic;
using UnityEngine;

public class FSMSystem {
    private readonly List<FSMState> states;
    private FSMState currentState;
    public FSMState CurrentState {
        get {
            return currentState;
        }
    }

    public FSMSystem() {
        states = new List<FSMState>();
    }

    #region 增加
    public void AddState(FSMState s) {
        // Check for Null reference before deleting
        if (s == null) {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (states.Count == 0) {
            states.Add(s);
            currentState = s;
            currentState.DoBeforeEntering(null);
            return;
        }

        if (states.Contains(s)) {
            Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
            return;
        }

        states.Add(s);
    }
    #endregion

    #region 删除
    public void DeleteState(StateID id) {
        // Check for NullState before deleting
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState tempS = GetStateByID(id);
        if (tempS != null) {
            states.Remove(tempS);
            return;
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }
    #endregion

    #region 修改
    //强制进入一个状态，不判断当前状态是否可转换到新状态
    public void ForceState(StateID id, BaseActor target) {
        if (id == StateID.NullStateID) {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }

        //退出当前状态
        currentState.DoBeforeLeaving(target);

        //更换当前状态
        currentState = state;

        //进入新状态
        currentState.DoBeforeEntering(target);
    }

    //执行状态转换
    public void PerformTransition(StateID id, BaseActor target) {
        // Check for NullTransition before changing the current state
        if (id == StateID.NullStateID) {
            Debug.LogError(" FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }
        //当前状态是否包含制定转换
        if (!currentState.IsHaveTransition(id)) {
            return;
        }
        //根据状态ID 获取状态
        FSMState state = GetStateByID(id);
        if (state == null) {
            Debug.LogError("FSM ERROR: Impossible to force to state " + id.ToString() +
                           ". It was not on the list of states");
            return;
        }
        
        //退出当前状态
        currentState.DoBeforeLeaving(target);

        //更换当前状态
        currentState = state;

        //进入新状态
        currentState.DoBeforeEntering(target);
    }
    #endregion

    #region 查看
    //是否在某一状态内
    public bool IsInState(StateID id) {
        if (id == StateID.NullStateID) {
            return false;
        }

        return CurrentState.ID == id;
    }

    private FSMState GetStateByID(StateID id) {
        int count = states.Count;
        for (int i = 0; i < count; ++i) {
            if (states[i].ID == id) {
                return states[i];
            }
        }

        return null;
    }
    #endregion
}
