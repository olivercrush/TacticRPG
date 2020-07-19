using System.Collections;
using System.Collections.Generic;
using ShipCore.Terrain;
using UnityEngine;
public class TRCell : MonoBehaviour
{
    private Cell _cell;

    public void UpdatePosition()
    {
        Vector3 updatedPosition = transform.position;
        updatedPosition.y = _cell.GetCellHeight();
        transform.position = updatedPosition;
    }
    
    public void Initialize(Cell cell, Material topMaterial, Material sideMaterial)
    {
        _cell = cell;
        transform.Find("Top").GetComponent<MeshRenderer>().material = topMaterial;
        transform.Find("LeftSide").GetComponent<MeshRenderer>().material = sideMaterial;
        transform.Find("RightSide").GetComponent<MeshRenderer>().material = sideMaterial;
    }

    // TODO : Create a TerrainCell factory to create particular cells based on materials (from an array of data structure)
    // data { name : OCEAN_WATER, top : ###, right_side : ###, left_side : ### }

}