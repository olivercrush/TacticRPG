using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target {
    private Position position;

    public Target(Position pos) {
        position = pos;
    }

    public Position GetPosition() {
        return position;
    }

    public bool IsEntity() {
        return (GameObject.FindObjectOfType<CombatManager>().GetEntityAtPosition(position) != null);
    }

    public Entity GetEntity() {
        return GameObject.FindObjectOfType<CombatManager>().GetEntityAtPosition(position);
    }
}