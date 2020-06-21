using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : MonoBehaviour
{
    Entity trigger;
    Position position;

    public void InitializeTile(Entity trigger, Position position) {
        this.trigger = trigger;
        this.position = position;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            //print("X:" + tilePosition.x + " / Y:" + tilePosition.y);

            Target[] targets = { new Target(position) };
            Action action = new AttackAction(targets, trigger);
        }
    }

    void OnMouseEnter() {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255);
    }

    void OnMouseExit() {
        //<Renderer>().material.color = new Color(97, 209, 84);
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }
}
