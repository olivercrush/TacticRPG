using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turn is a data object that holds the active entity and the actions done during said turn.
/// It also has the responsibility to determine if the turn is over based on the actions registered during the turn.
/// </summary>
public class Turn {

    public TurnStatus status;
    private Entity entity;
    private List<Action> actions;

    public Turn(Entity activeEntity) {
        status = new TurnStatus();
        status.InitializeTurnStatus();
        
        actions = new List<Action>();
        entity = activeEntity;

        Debug.Log("New turn : " + entity.infos.name);
        entity.CenterCamera();
    }

    /// <summary>
    /// Register an action done during this turn and store it in a list, then checks if the turn is over.
    /// </summary>
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

    // TODO : Implement strategy pattern to check the victory / defeat conditions
}

/// <summary>
/// Data structure that represents the turn status
/// </summary>
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