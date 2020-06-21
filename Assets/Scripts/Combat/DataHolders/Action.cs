using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    protected bool isDone;
    protected ActionType actionType;
    protected Target[] targets;
    protected Entity trigger;

    public Action(Target[] targets, Entity trigger, ActionType actionType) {
        isDone = false;
        this.targets = targets;
        this.trigger = trigger;
        this.actionType = actionType;

        ApplyAction();
        GameObject.FindObjectOfType<CombatManager>().GetActiveTurn().RegisterAction(this);
    }

    protected virtual void ApplyAction() { }

    public ActionType GetActionType() {
        return actionType;
    }
}

public enum ActionType {
    MOVE,
    ATTACK,
    GUARD,
    SKILL
}