using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private bool isDone;
    private ActionType actionType;
    private Target[] targets;
    private Entity trigger;

    public Action(Target[] targets, Entity trigger, ActionType actionType) {
        isDone = false;
        this.targets = targets;
        this.trigger = trigger;
        this.actionType = actionType;

        ApplyAction();
        GameObject.FindObjectOfType<CombatManager>().GetActiveTurn().RegisterAction(this);
    }

    private void ApplyAction() {
        if (actionType == ActionType.MOVE) {
            Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") moved to (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ")");
            trigger.Move(targets[0].GetPosition());
            GameObject.FindObjectOfType<Terrain>().MoveEntity(trigger);
        }
        else if (actionType == ActionType.ATTACK) {

            if (targets[0].IsEntity()) {
                Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") attacked entity at (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ") for " + trigger.infos.attackPoints + " points");
                trigger.Attack(targets[0].GetEntity());
            }
            else {
                Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") attacked nothing at (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ") for " + trigger.infos.attackPoints + " points");
            }
            
        }

        GameObject.FindObjectOfType<Terrain>().DeleteActionTiles();
        isDone = true;
    }

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