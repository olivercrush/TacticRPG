using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn {

    public TurnStatus status;
    private Entity entity;
    private List<Action> actions;

    public Turn(Entity activeEntity) {
        status = new TurnStatus();
        status.InitializeTurnStatus();
        actions = new List<Action>();
        entity = activeEntity;

        Debug.Log("New turn : " + entity.infos.id);
        entity.CenterCamera();
    }

    public void RegisterAction(Action action) {
        if (action.GetActionType() == ActionType.ATTACK || action.GetActionType() == ActionType.GUARD || action.GetActionType() == ActionType.SKILL) {
            status.attackCount += 1;
        }
        else if (action.GetActionType() == ActionType.MOVE) {
            status.moveCount += 1;
            entity.CenterCamera();
        }

        actions.Add(action);

        if (status.CheckEndConditions(entity)) {
            GameObject.FindObjectOfType<CombatManager>().EndTurn();
        }
    }
}

public struct TurnStatus {
    public int attackCount;
    public int moveCount;

    public void InitializeTurnStatus() {
        attackCount = 0;
        moveCount = 0;
    }

    public bool CheckEndConditions(Entity entity) {
        if (attackCount > 0 && moveCount > 0) return true;
        return false;
    }
}