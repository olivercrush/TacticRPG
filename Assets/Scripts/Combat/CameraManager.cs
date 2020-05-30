using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void AlignCameraWithObject(Vector3 objectPos) {
        transform.position = objectPos + new Vector3(-3, 3, -3);
    }
}
