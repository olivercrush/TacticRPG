using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public EntityInfo infos;

    public void InitializeEntity(EntityInfo infos) {
        this.infos = infos;

        if (infos.team == Team.BLUE) {
            GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }
        else {
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
    }

    public Position GetPosition() {
        return infos.position;
    }

    public void Die() {
        // play animation
        Debug.Log("DIE : " + infos.id);
    }

    public void CenterCamera() {
        FindObjectOfType<CameraManager>().AlignCameraWithObject(transform.position);
    }
}

[System.Serializable]
public struct EntityInfo {
    public int id;
    public string name;

    public bool dead;
    public Position position;

    public int lifePoints;
    public int attackPoints;
    public int attackRange;

    public int initiative;
    public int moveRange;
    public Team team;
}

[System.Serializable]
public struct Position {
    public int x;
    public int y;

    public Position(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public enum Team {
    RED,
    BLUE
}