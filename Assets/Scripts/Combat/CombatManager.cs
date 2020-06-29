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

    // TODO : Check if there is a way to pass this responsibility to another task without increasing coupling

    /// <summary>
    /// Get current turn
    /// </summary>
    /// <returns>The current turn</returns>
    public Turn GetActiveTurn() {
        return turns[turns.Count - 1];
    }

    /// <summary>
    /// End the current turn, end battle if conditions checked or creates a new turn
    /// </summary>
    public void EndTurn() {
        if (VerifyEndOfCombat()) {
            print("End of battle");
        }
        else {
            entitiesManager.IncrementActiveEntity();
            turns.Add(new Turn(entitiesManager.GetActiveEntity()));
        }
    }

    // TODO : Make the conditions a customizable module to allow conditions diversity

    /// <summary>
    /// Check if the end of combat's conditions are met
    /// </summary>
    /// <returns>True if conditions are met, false if they are not</returns>
    public bool VerifyEndOfCombat() {
        EntityCount entityCount = entitiesManager.GetEntityCount();
        if (entityCount.blue == 0 || entityCount.red == 0) {
            return true;
        }
        return false;
    }
}