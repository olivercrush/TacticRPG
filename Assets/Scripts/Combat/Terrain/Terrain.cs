using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Terrain {

    private GameObject manager;
    private TerrainCharacteristics characteristics;
    private float[,] heightMap;
    private GameObject[,] terrain;

    public Terrain(GameObject manager, TerrainCharacteristics characteristics) {
        this.manager = manager;
        this.characteristics = characteristics;
    }

    public float GetHeightMapValue(int x, int y) {
        return heightMap[x, y];
    }

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

                //cell.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
                //cell.GetComponent<TerrainCubeUVsMapper>().InitializeUVs();
                
                terrain[x, y] = cell;
            }
        }
    }

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

