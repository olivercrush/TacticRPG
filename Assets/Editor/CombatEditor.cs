using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (CombatManager))]
public class CombatEditor : Editor
{
    public override void OnInspectorGUI() {
        CombatManager combatManager = (CombatManager) target;

        if (DrawDefaultInspector()) {
            if (combatManager.autoUpdate) {
                combatManager.Initialize();
            }
        }

        if (GUILayout.Button("Generate")) {
            combatManager.Initialize();
        }
    }
}
