using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCell : MonoBehaviour {

    public void Initialize(Material topMaterial, Material sideMaterial) {
        transform.Find("Top").GetComponent<MeshRenderer>().material = topMaterial;
        transform.Find("LeftSide").GetComponent<MeshRenderer>().material = sideMaterial;
        transform.Find("RightSide").GetComponent<MeshRenderer>().material = sideMaterial;
    }

}