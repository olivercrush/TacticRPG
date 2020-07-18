using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action {

    public AttackAction(Target[] targets, Entity trigger) : base(targets, trigger, ActionType.ATTACK) { }

    protected override void ApplyAction() {
        for (int i = 0; i < targets.Length; i++) {
            if (targets[i].IsEntity()) {
                Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") attacked entity at (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ") for " + trigger.infos.attackPoints + " points");
                targets[i].GetEntity().infos.lifePoints -= trigger.infos.attackPoints;
                GameObject.FindObjectOfType<EntitiesManager>().UpdateEntity(targets[i].GetEntity());
            }
            else {
                Debug.Log("Action applied : entity at pos (" + trigger.infos.position.x + "," + trigger.infos.position.y + ") attacked nothing at (" + targets[0].GetPosition().x + "," + targets[0].GetPosition().y + ") for " + trigger.infos.attackPoints + " points");
            }
        }

        GameObject.FindObjectOfType<TerrainManager>().DeleteActionTiles();
        isDone = true;
    }

}