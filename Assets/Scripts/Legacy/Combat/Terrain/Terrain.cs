using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Terrain is a data object that creates and holds the map data
/// Responsibility : Creation and stocking of the map
/// </summary>
public class Terrain {

    private GameObject manager;
    private TerrainCharacteristics characteristics;
    private float[,] heightMap;
    private GameObject[,] terrain;

    public Terrain(GameObject manager, TerrainCharacteristics characteristics) {
        this.manager = manager;
        this.characteristics = characteristics;
    }

    /// <summary>
    /// Returns the height value at the specified coordinates
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>The height value at the coordinates (x, y)</returns>
    public float GetHeightMapValue(int x, int y) {
        return heightMap[x, y];
    }

    // TODO : Make the class read a JSON file that represents a terrain and generate it from there, then create an editor level

    /// <summary>
    /// Generate a terrain object from the characteristics with the prefab and materials
    /// </summary>
    /// <param name="terrainPrefab">The GameObject that will be used as a terrain cell</param>
    /// <param name="topMaterial">The material that will be used to render the top part of a cell - SOON TO BE DEPRECATED</param>
    /// <param name="sideMaterial">The material that will be used to render the side parts of a cell - SOON TO BE DEPRECATED</param>
    public void GenerateTerrain(GameObject terrainPrefab, Material topMaterial, Material sideMaterial) {
        DestroyAllChidren();
        heightMap = NoiseGenerator.GenerateNoiseMap(characteristics.size, characteristics.scale, characteristics.amplitude, characteristics.frequency, characteristics.octave);
        terrain = new GameObject[characteristics.size, characteristics.size];

        GameObject cells = new GameObject("Cells");
        cells.transform.parent = manager.transform;

        for (int y = 0; y < characteristics.size; y++) {
            for (int x = 0; x < characteristics.size; x++) {
                GameObject cell = GameObject.Instantiate(terrainPrefab);
                cell.transform.position = new Vector3(x - characteristics.size / 2, heightMap[x, y], y - characteristics.size / 2);
                cell.transform.parent = cells.transform;
                cell.GetComponent<TerrainCell>().Initialize(topMaterial, sideMaterial);
                terrain[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// Destroy all the terrain cells' game objects
    /// </summary>
    private void DestroyAllChidren() {
        var tmpList = manager.transform.Cast<Transform>().ToList();
        foreach (Transform child in tmpList) {
            if (Application.isEditor) {
                GameObject.DestroyImmediate(child.gameObject);
            } else {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

}

[System.Serializable]
public struct TerrainCharacteristics {
    public int size;
    public int scale;
    public float amplitude;
    public int frequency;
    public int octave;
}

