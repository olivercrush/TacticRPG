using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Terrain : MonoBehaviour
{
    public Material terrainMaterial;

    public GameObject higlightedMove;
    public GameObject terrainPrefab;

    public int terrainSize = 5;
    public float scale = 1;
    public float amplitude = 1;
    public int frequency = 1;
    public int octave = 1;

    public bool autoUpdate;

    private float[,] heightMap;
    private GameObject[,] terrain;

    void Start()
    {
        GenerateTerrain();
    }

    public void GenerateTerrain() {
        DestroyAllChidren();
        heightMap = NoiseGenerator.GenerateNoiseMap(terrainSize, scale, amplitude, frequency, octave);
        terrain = new GameObject[terrainSize, terrainSize];

        for (int y = 0; y < terrainSize; y++) {
            for (int x = 0; x < terrainSize; x++) {
                GameObject cell = GameObject.Instantiate(terrainPrefab);
                cell.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y], y - terrainSize / 2);
                cell.transform.parent = transform;
                cell.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
                cell.GetComponent<TerrainCubeUVsMapper>().InitializeUVs();
                terrain[x, y] = cell;
            }
        }
    }

    public void HighlightCharacterMovement(CharacterInfo info) {
        DeleteHighlightedCharacterMovement();
        
        GameObject movementTiles = new GameObject("MovementTiles");
        movementTiles.transform.parent = transform;
        for (int y = info.position.y - info.moveRange; y < info.position.y + info.moveRange + 1; y++) {
            for (int x = info.position.x - info.moveRange; x < info.position.x + info.moveRange + 1; x++) {
                float lenght = Mathf.Abs(x - info.position.x) + Mathf.Abs(y - info.position.y);
                if (lenght <= info.moveRange) {
                    if (x < terrainSize && x >= 0 && y < terrainSize && y >= 0) {
                        if (x != info.position.x || y != info.position.y) {
                            GameObject tmp = GameObject.Instantiate(higlightedMove, movementTiles.transform);
                            tmp.GetComponent<MovementTile>().InitializeTile(new Position(x, y));
                            tmp.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y] + 1.01f, y - terrainSize / 2);
                        }
                    }
                }
            }
        }
    }

    public void DeleteHighlightedCharacterMovement() {
        GameObject highlightedMoves = GameObject.Find("MovementTiles");
        if (Application.isEditor) {
            GameObject.DestroyImmediate(highlightedMoves);
        } else {
            GameObject.Destroy(highlightedMoves);
        }
    }

    public void PlaceCharacter(Character character) {
        Position characterPos = character.infos.position;
        character.transform.position = new Vector3(characterPos.x - terrainSize / 2, heightMap[characterPos.x, characterPos.y] + 1.5f, characterPos.y - terrainSize / 2);
        character.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void MoveCharacter(Character character) {
        Position characterPos = character.infos.position;
        character.transform.position = new Vector3(characterPos.x - terrainSize / 2, heightMap[characterPos.x, characterPos.y] + 1.5f, characterPos.y - terrainSize / 2);
    }

    private void DestroyAllChidren() {
        var tmpList = transform.Cast<Transform>().ToList();
        foreach (Transform child in tmpList) {
            if (Application.isEditor) {
                GameObject.DestroyImmediate(child.gameObject);
            } else {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
