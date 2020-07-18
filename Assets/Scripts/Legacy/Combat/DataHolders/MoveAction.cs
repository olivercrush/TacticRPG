using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action {

    public MoveAction(Target target, Entity trigger) : base(new Target[] { target }, trigger, ActionType.MOVE) { }

    protected override void ApplyAction() {
        Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") moved to (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ")");
        
        trigger.infos.position = targets[0].GetPosition();
        GameObject.FindObjectOfType<EntitiesManager>().UpdateEntity(trigger);

        GameObject.FindObjectOfType<TerrainManager>().MoveEntity(trigger);
        GameObject.FindObjectOfType<TerrainManager>().DeleteActionTiles();
        isDone = true;
    }

}