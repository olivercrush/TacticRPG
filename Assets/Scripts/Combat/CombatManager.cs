using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject EntityPrefab;
    public bool autoUpdate;
    public EntityInfo[] entitiesInfos;

    private TerrainManager terrainManager;
    private EntitiesManager entitiesManager;

    private List<Turn> turns;

    void Start()
    {
        Initialize();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            StartAction(ActionType.MOVE);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            StartAction(ActionType.ATTACK);
        }
    }

    /// <summary>
    /// Creates and initializes the terrain, entities and turns managers
    /// </summary>
    public void Initialize()
    {
        terrainManager = GetComponentInChildren<TerrainManager>();
        terrainManager.GenerateTerrain();

        entitiesManager = GetComponentInChildren<EntitiesManager>();
        entitiesManager.InitializeEntities(entitiesInfos);

        turns = new List<Turn>();
        turns.Add(new Turn(entitiesManager.GetActiveEntity()));
    }

    // TODO : Refactor this method

    /// <summary>
    /// Listen in to the user interactions with the game and acts accordingly
    /// </summary>
    /// <param name="actionType">The ActionType that represents the action the user wants to do</param>
    public void StartAction(ActionType actionType) {
        if (actionType == ActionType.MOVE && turns[turns.Count - 1].status.moveCount < 1)
            terrainManager.CreateMovementTiles(entitiesManager.GetActiveEntity(), entitiesManager.GetAllEntities());
        else if (actionType == ActionType.ATTACK && turns[turns.Count - 1].status.attackCount < 1)
            terrainManager.CreateAttackTiles(entitiesManager.GetActiveEntity());
    }

    public Turn GetActiveTurn() {
        return turns[turns.Count - 1];
    }

    public void EndTurn() {
        entitiesManager.IncrementActiveEntity();

        if (VerifyEndOfCombat()) {
            print("End of battle");
        }
        else {
            turns.Add(new Turn(entitiesManager.GetActiveEntity()));
        }
    }

    public bool VerifyEndOfCombat() {
        EntityCount entityCount = entitiesManager.GetEntityCount();
        if (entityCount.blue == 0 || entityCount.red == 0) {
            return true;
        }
        return false;
    }
}