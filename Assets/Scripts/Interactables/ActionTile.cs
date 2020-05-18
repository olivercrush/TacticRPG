using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTile : MonoBehaviour
{
    Position tilePosition;
    CharacterInfo characterInfo;

    public void InitializeTile(Position pos, CharacterInfo info) {
        tilePosition = pos;
        characterInfo = info;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            print("X:" + tilePosition.x + " / Y:" + tilePosition.y);
            //GameObject.FindObjectOfType<CombatManager>().MoveActiveCharacter(tilePosition);
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
