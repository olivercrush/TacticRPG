using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interaction tile used to dispatch a movement action
/// </summary>
public class MovementTile : MonoBehaviour
{
    Entity trigger;
    Position position;

    /// <summary>
    /// Initialize the tile with data
    /// </summary>
    /// <param name="trigger">The entity that created the tile</param>
    /// <param name="position">The position of the tile</param>
    public void InitializeTile(Entity trigger, Position position) {
        this.trigger = trigger;
        this.position = position;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            // Creates an action
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
