using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraManager will hold all the camera positioning and transition logic
/// </summary>
public class CameraManager : MonoBehaviour
{

    /// <summary>
    /// Align the camera with a particular object position
    /// </summary>
    public void AlignCameraWithObject(Vector3 objectPos) {
        transform.position = objectPos + new Vector3(-3, 3, -3);
    }

    // TODO : Animation transition when aligning camera with an object
}
