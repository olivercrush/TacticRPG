using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interaction tile used to dispatch an attack action
/// </summary>
public class AttackTile : MonoBehaviour
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
            Target[] targets = { new Target(position) };
            Action action = new AttackAction(targets, trigger);
        }
    }

    void OnMouseEnter() {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255);
    }

    void OnMouseExit() {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }
}
