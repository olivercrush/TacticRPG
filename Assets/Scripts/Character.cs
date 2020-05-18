using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    public CharacterInfo infos;

    // Start is called before the first frame update
    void Start()
    {
        CenterCamera();
    }

    public void InitializeCharacter(CharacterInfo infos) {
        this.infos = infos;
    }

    public Position GetPosition() {
        return infos.position;
    }

    public void CenterCamera() {
        if (infos.active) {
            FindObjectOfType<CameraManager>().AlignCameraWithObject(transform.position);
        }
    }
}

[System.Serializable]
public struct CharacterInfo {
    public Position position;
    public int moveRange;
    public bool active;
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