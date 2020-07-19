using System;
using System.Collections;
using System.Collections.Generic;
using ShipCore.Terrain;
using UnityEngine;

public class TRCell : MonoBehaviour
{
    private Cell _cell;
    
    public void Initialize(Cell cell, Material topMaterial, Material sideMaterial)
    {
        _cell = cell;
        ApplyTextures(topMaterial, sideMaterial);
    }

    public Guid GetCellGuid()
    {
        return _cell.Id;
    }

    public void ApplyTextures(Material topMaterial, Material sideMaterial)
    {
        transform.Find("Top").GetComponent<MeshRenderer>().material = topMaterial;
        transform.Find("LeftSide").GetComponent<MeshRenderer>().material = sideMaterial;
        transform.Find("RightSide").GetComponent<MeshRenderer>().material = sideMaterial;
    }

    public void UpdatePosition()
    {
        Vector3 updatedPosition = transform.position;
        updatedPosition.y = _cell.GetCellHeight();
        transform.position = updatedPosition;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftControl))
                GameObject.FindObjectOfType<TerrainRenderer>().AddCellSelection(_cell.Id);
            else
                GameObject.FindObjectOfType<TerrainRenderer>().CreateCellSelection(_cell.Id);
            
            Debug.Log("Selected (" + _cell.Position.X + "," + _cell.Position.Y + ")");
        }
    }

    // TODO#001 : Create a TRCell factory to create particular cells based on materials (from an array of data structure)
    // data { name : OCEAN_WATER, top : ###, right_side : ###, left_side : ### }

}