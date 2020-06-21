using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTile : MonoBehaviour
{
    Entity trigger;
    Position tilePosition;

    public void InitializeTile(Entity trigger, Position pos) {
        this.trigger = trigger;
        tilePosition = pos;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            //print("X:" + tilePosition.x + " / Y:" + tilePosition.y);
            Target target = new Target(tilePosition);
            Action action = new MoveAction(target, trigger);
        }
    }

    void OnMouseEnter() {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255);
    }

    void OnMouseExit() {
        //<Renderer>().material.color = new Color(97, 209, 84);
        GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }
}
