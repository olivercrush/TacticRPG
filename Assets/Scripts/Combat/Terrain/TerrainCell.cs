using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TerrainCell is a script attached to all terrain cells.
/// Responsibility : The texture attributions to the different parts, the animations (for earthquakes, water, lava..)
/// </summary>
public class TerrainCell : MonoBehaviour {

    /// <summary>
    /// Assign materials for the top, right and left planes
    /// </summary>
    public void Initialize(Material topMaterial, Material sideMaterial) {
        transform.Find("Top").GetComponent<MeshRenderer>().material = topMaterial;
        transform.Find("LeftSide").GetComponent<MeshRenderer>().material = sideMaterial;
        transform.Find("RightSide").GetComponent<MeshRenderer>().material = sideMaterial;
    }

    // TODO : Create a TerrainCell factory to create particular cells based on materials (from an array of data structure)
    // data { name : OCEAN_WATER, top : ###, right_side : ###, left_side : ### }

}