using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Terrain : MonoBehaviour
{
    public Material terrainMaterial;

    public GameObject movementTile;
    public GameObject attackTile;
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

        GameObject cells = new GameObject("Cells");
        cells.transform.parent = transform;

        for (int y = 0; y < terrainSize; y++) {
            for (int x = 0; x < terrainSize; x++) {
                GameObject cell = GameObject.Instantiate(terrainPrefab);
                cell.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y], y - terrainSize / 2);
                cell.transform.parent = cells.transform;
                cell.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
                cell.GetComponent<TerrainCubeUVsMapper>().InitializeUVs();
                terrain[x, y] = cell;
            }
        }
    }

    public void CreateMovementTiles(CharacterInfo info, Character[] characters) {
        DeleteActionTiles();
        
        GameObject movementTiles = new GameObject("ActionTiles");
        movementTiles.transform.parent = transform;

        for (int y = info.position.y - info.moveRange; y < info.position.y + info.moveRange + 1; y++) {
            for (int x = info.position.x - info.moveRange; x < info.position.x + info.moveRange + 1; x++) {
                float lenght = Mathf.Abs(x - info.position.x) + Mathf.Abs(y - info.position.y);
                if (lenght <= info.moveRange) {
                    if (x < terrainSize && x >= 0 && y < terrainSize && y >= 0) {

                        bool freeTile = true;
                        for (int i = 0; i < characters.Length; i++) {
                            if (characters[i].infos.position.x == x && characters[i].infos.position.y == y) {
                                freeTile = false;
                            }
                        }

                        if (freeTile) {
                            GameObject tmp = GameObject.Instantiate(movementTile, movementTiles.transform);
                            tmp.GetComponent<MovementTile>().InitializeTile(new Position(x, y));
                            tmp.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y] + 1.01f, y - terrainSize / 2);
                        }

                    }
                }
            }
        }
    }

    public void CreateAttackTiles(CharacterInfo info) {
        DeleteActionTiles();
        
        GameObject attackTiles = new GameObject("ActionTiles");
        attackTiles.transform.parent = transform;
        for (int y = info.position.y - info.attackRange; y < info.position.y + info.attackRange + 1; y++) {
            for (int x = info.position.x - info.attackRange; x < info.position.x + info.attackRange + 1; x++) {
                float lenght = Mathf.Abs(x - info.position.x) + Mathf.Abs(y - info.position.y);
                if (lenght <= info.attackRange) {
                    if (x < terrainSize && x >= 0 && y < terrainSize && y >= 0) {

                        if (x != info.position.x || y != info.position.y) {
                            GameObject tmp = GameObject.Instantiate(attackTile, attackTiles.transform);
                            tmp.GetComponent<AttackTile>().InitializeTile(new Position(x, y), info);
                            tmp.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y] + 1.01f, y - terrainSize / 2);
                        }

                    }
                }
            }
        }
    }

    public void DeleteActionTiles() {
        GameObject highlightedMoves = GameObject.Find("ActionTiles");
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
