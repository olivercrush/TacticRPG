using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTile : MonoBehaviour
{
    Position tilePosition;

    public void InitializeTile(Position pos) {
        tilePosition = pos;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            //print("X:" + tilePosition.x + " / Y:" + tilePosition.y);
            GameObject.FindObjectOfType<CombatManager>().MoveActiveCharacter(tilePosition);
        }
    }
}
