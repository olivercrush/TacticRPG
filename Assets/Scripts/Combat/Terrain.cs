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

    public void CreateMovementTiles(Entity entity, Entity[] Entitys) {
        DeleteActionTiles();
        
        GameObject movementTiles = new GameObject("ActionTiles");
        movementTiles.transform.parent = transform;

        for (int y = entity.infos.position.y - entity.infos.moveRange; y < entity.infos.position.y + entity.infos.moveRange + 1; y++) {
            for (int x = entity.infos.position.x - entity.infos.moveRange; x < entity.infos.position.x + entity.infos.moveRange + 1; x++) {
                float lenght = Mathf.Abs(x - entity.infos.position.x) + Mathf.Abs(y - entity.infos.position.y);
                if (lenght <= entity.infos.moveRange) {
                    if (x < terrainSize && x >= 0 && y < terrainSize && y >= 0) {

                        bool freeTile = true;
                        for (int i = 0; i < Entitys.Length; i++) {
                            if (Entitys[i].infos.position.x == x && Entitys[i].infos.position.y == y) {
                                freeTile = false;
                            }
                        }

                        if (freeTile) {
                            GameObject tmp = GameObject.Instantiate(movementTile, movementTiles.transform);
                            tmp.GetComponent<MovementTile>().InitializeTile(entity, new Position(x, y));
                            tmp.transform.position = new Vector3(x - terrainSize / 2, heightMap[x, y] + 1.01f, y - terrainSize / 2);
                        }

                    }
                }
            }
        }
    }

    public void CreateAttackTiles(Entity entity) {
        DeleteActionTiles();
        
        GameObject attackTiles = new GameObject("ActionTiles");
        attackTiles.transform.parent = transform;
        for (int y = entity.infos.position.y - entity.infos.attackRange; y < entity.infos.position.y + entity.infos.attackRange + 1; y++) {
            for (int x = entity.infos.position.x - entity.infos.attackRange; x < entity.infos.position.x + entity.infos.attackRange + 1; x++) {
                float lenght = Mathf.Abs(x - entity.infos.position.x) + Mathf.Abs(y - entity.infos.position.y);
                if (lenght <= entity.infos.attackRange) {
                    if (x < terrainSize && x >= 0 && y < terrainSize && y >= 0) {

                        if (x != entity.infos.position.x || y != entity.infos.position.y) {
                            GameObject tmp = GameObject.Instantiate(attackTile, attackTiles.transform);
                            tmp.GetComponent<AttackTile>().InitializeTile(entity, new Position(x, y));
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

    public void PlaceEntity(Entity entity) {
        Position pos = entity.infos.position;
        entity.transform.position = new Vector3(pos.x - terrainSize / 2, heightMap[pos.x, pos.y] + 1.5f, pos.y - terrainSize / 2);
        entity.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void MoveEntity(Entity entity) {
        Position pos = entity.infos.position;
        entity.transform.position = new Vector3(pos.x - terrainSize / 2, heightMap[pos.x, pos.y] + 1.5f, pos.y - terrainSize / 2);
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
