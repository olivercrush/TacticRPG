using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// TerrainManager is a tier-2 component in charge of the interactions with the Terrain
/// </summary>
public class TerrainManager : MonoBehaviour
{
    public Material topMaterial;
    public Material sideMaterial;

    public GameObject movementTile;
    public GameObject attackTile;
    public GameObject terrainPrefab;

    public TerrainCharacteristics terrainCharacteristics;
    public bool autoUpdate;

    private Terrain terrain;

    void Start()
    {
        terrain = new Terrain(this.gameObject, terrainCharacteristics);
    }

    /// <summary>
    /// Generate the terrain
    /// </summary>
    public void GenerateTerrain() {
        terrain.GenerateTerrain(terrainPrefab, topMaterial, sideMaterial);
    }

    // TODO : Use A* algorithm to find paths

    /// <summary>
    /// Create interaction tiles for the player to choose a cell to move to based on other entities and obstacles
    /// </summary>
    /// <param name="entity">The entity for which we create the movement tiles</param>
    /// <param name="entities">The entities that participate in the combat</param>
    public void CreateMovementTiles(Entity entity, Entity[] entities) {
        DeleteActionTiles();
        
        GameObject movementTiles = new GameObject("ActionTiles");
        movementTiles.transform.parent = transform;

        for (int y = entity.infos.position.y - entity.infos.moveRange; y < entity.infos.position.y + entity.infos.moveRange + 1; y++) {
            for (int x = entity.infos.position.x - entity.infos.moveRange; x < entity.infos.position.x + entity.infos.moveRange + 1; x++) {
                float lenght = Mathf.Abs(x - entity.infos.position.x) + Mathf.Abs(y - entity.infos.position.y);
                if (lenght <= entity.infos.moveRange) {
                    if (x < terrainCharacteristics.size && x >= 0 && y < terrainCharacteristics.size && y >= 0) {

                        bool freeTile = true;
                        for (int i = 0; i < entities.Length; i++) {
                            if (entities[i].infos.position.x == x && entities[i].infos.position.y == y) {
                                freeTile = false;
                            }
                        }

                        if (freeTile) {
                            GameObject tmp = GameObject.Instantiate(movementTile, movementTiles.transform);
                            tmp.GetComponent<MovementTile>().InitializeTile(entity, new Position(x, y));
                            tmp.transform.position = new Vector3(x - terrainCharacteristics.size / 2, terrain.GetHeightMapValue(x, y) + 1.01f, y - terrainCharacteristics.size / 2);
                        }

                    }
                }
            }
        }
    }

    /// <summary>
    /// Create interaction tiles for the player to choose a cell to attack
    /// </summary>
    /// <param name="entity">The entity for which we create the attack tiles</param>
    public void CreateAttackTiles(Entity entity) {
        DeleteActionTiles();
        
        GameObject attackTiles = new GameObject("ActionTiles");
        attackTiles.transform.parent = transform;
        for (int y = entity.infos.position.y - entity.infos.attackRange; y < entity.infos.position.y + entity.infos.attackRange + 1; y++) {
            for (int x = entity.infos.position.x - entity.infos.attackRange; x < entity.infos.position.x + entity.infos.attackRange + 1; x++) {
                float lenght = Mathf.Abs(x - entity.infos.position.x) + Mathf.Abs(y - entity.infos.position.y);
                if (lenght <= entity.infos.attackRange) {
                    if (x < terrainCharacteristics.size && x >= 0 && y < terrainCharacteristics.size && y >= 0) {

                        if (x != entity.infos.position.x || y != entity.infos.position.y) {
                            GameObject tmp = GameObject.Instantiate(attackTile, attackTiles.transform);
                            tmp.GetComponent<AttackTile>().InitializeTile(entity, new Position(x, y));
                            tmp.transform.position = new Vector3(x - terrainCharacteristics.size / 2, terrain.GetHeightMapValue(x, y) + 1.01f, y - terrainCharacteristics.size / 2);
                        }

                    }
                }
            }
        }
    }

    /// <summary>
    /// Delete the interaction tiles
    /// </summary>
    public void DeleteActionTiles() {
        GameObject highlightedMoves = GameObject.Find("ActionTiles");
        if (Application.isEditor) {
            GameObject.DestroyImmediate(highlightedMoves);
        } else {
            GameObject.Destroy(highlightedMoves);
        }
    }

    /// <summary>
    /// Place an entity on the terrain
    /// </summary>
    /// <param name="entity">The entity we want to place on the terrain</param>
    public void PlaceEntity(Entity entity) {
        Position pos = entity.infos.position;
        entity.transform.position = new Vector3(pos.x - terrainCharacteristics.size / 2, terrain.GetHeightMapValue(pos.x, pos.y) + 1.5f, pos.y - terrainCharacteristics.size / 2);
        entity.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // TODO : Create a walk path animation

    /// <summary>
    /// Move (update) an entity already on the terrain
    /// </summary>
    /// <param name="entity">The entity we want to move</param>
    public void MoveEntity(Entity entity) {
        Position pos = entity.infos.position;
        entity.transform.position = new Vector3(pos.x - terrainCharacteristics.size / 2, terrain.GetHeightMapValue(pos.x, pos.y) + 1.5f, pos.y - terrainCharacteristics.size / 2);
    }
}
