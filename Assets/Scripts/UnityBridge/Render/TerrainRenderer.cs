using System;
using System.Collections;
using System.Collections.Generic;
using ShipCore.Terrain;
using UnityEngine;
using Terrain = ShipCore.Terrain.Terrain;

public class TerrainRenderer : MonoBehaviour
{
    public int width, height;
    public GameObject cellPrefab;
    public Material topMaterial;
    public Material sideMaterial;

    private Terrain _terrain;
    private GameObject[] _renderedCells;

    private void Start()
    {
        _terrain = new Terrain((width, height));
        InitializeRenderCells(_terrain);
        
        _terrain.UpdateCell((3, 3), 3);
        _terrain.UpdateCell((4, 3), -1);
        _terrain.UpdateCell((0, 0), 2);
        _terrain.UpdateCell((1, 3), 1);
    }

    private void InitializeRenderCells(Terrain terrain)
    {
        _renderedCells = new GameObject[terrain.Cells.Length];
        for (int y = 0; y < terrain.Cells.GetLength(1); y++) {
            for (int x = 0; x < terrain.Cells.GetLength(0); x++)
            {
                // Create Vector3 from cell
                Vector3 position = new Vector3(x, terrain.Cells[x, y].GetCellHeight(), y);
                
                // Create the cell Gameobject
                GameObject trCell = GameObject.Instantiate(cellPrefab, position, Quaternion.identity, transform);
                trCell.GetComponent<TRCell>().Initialize(terrain.Cells[x, y], topMaterial, sideMaterial);
                
                _renderedCells[x + terrain.Cells.GetLength(0) * y] = trCell;
                terrain.Cells[x, y].OnHeightChanged += OnHeightChanged;
            }
        }
    }

    private void OnHeightChanged(object sender, EventArgs e)
    {
        Cell c = sender as Cell;
        _renderedCells[c.Position.X + _terrain.Cells.GetLength(0) * c.Position.Y].GetComponent<TRCell>().UpdatePosition();
    }
}