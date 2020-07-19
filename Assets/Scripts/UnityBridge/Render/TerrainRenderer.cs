using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void InitializeRenderCells(Terrain terrain)
    {
        _renderedCells = new GameObject[terrain.Cells.Length];
        for (int y = 0; y < terrain.Cells.GetLength(1); y++) {
            for (int x = 0; x < terrain.Cells.GetLength(0); x++) {
                Vector3 position = new Vector3(x, terrain.Cells[x, y].GetCellHeight(), y);
                GameObject cell = GameObject.Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.GetComponent<TerrainCell>().Initialize(topMaterial, sideMaterial);
                
                _renderedCells[x + terrain.Cells.GetLength(0) * y] = cell;
            }
        }
    }
}