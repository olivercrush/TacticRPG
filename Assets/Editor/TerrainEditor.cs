using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Terrain))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI() {
        Terrain terrain = (Terrain) target;

        if (DrawDefaultInspector()) {
            if (terrain.autoUpdate) {
                terrain.GetComponentInParent<CombatManager>().Initialize();
            }
        }
    }
}
