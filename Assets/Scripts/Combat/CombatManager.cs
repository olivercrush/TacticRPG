using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject EntityPrefab;
    public bool autoUpdate;
    public EntityInfo[] entitiesInfos;

    private Terrain terrain;
    private Entity[] entities;
    private int activeEntity;

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

    public void Initialize()
    {
        terrain = GetComponentInChildren<Terrain>();
        terrain.GenerateTerrain();

        GenerateEntities();
        turns = new List<Turn>();
        turns.Add(new Turn(entities[activeEntity]));
    }

    public void StartAction(ActionType actionType) {
        if (actionType == ActionType.MOVE && turns[turns.Count - 1].status.moveCount < 1)
            terrain.CreateMovementTiles(entities[activeEntity], entities);
        else if (actionType == ActionType.ATTACK && turns[turns.Count - 1].status.attackCount < 1)
            terrain.CreateAttackTiles(entities[activeEntity]);
    }

    public void EndTurn() {
        activeEntity++;
        if (activeEntity >= entities.Length) {
            activeEntity = 0;
        }

        if (VerifyEndOfCombat()) {
            print("End of battle");
        }
        else {
            turns.Add(new Turn(entities[activeEntity]));
        }
    }

    public bool VerifyEndOfCombat() {
        int blueCount = 0;
        int redCount = 0;

        for (int i = 0; i < entities.Length; i++) {
            if (entities[i].infos.team == Team.RED) {
                redCount++;
            }
            else {
                blueCount++;
            }
        }

        if (blueCount == 0 || redCount == 0) {
            return true;
        }

        return false;
    }

    public Turn GetActiveTurn() {
        return turns[turns.Count - 1];
    }

    public void UpdateEntities(Entity[] updatedEntities) {
        entities.UpdateEntities(updatedEntities);
    }

    public void UpdateEntities(Entity updatedEntity) {
        Entity[] updatedEntities = { updatedEntity };
        entities.UpdateEntities(updatedEntities);
    }

    public void GenerateEntities() {
        DeleteAllEntities();
        entities = new Entity[entitiesInfos.Length];
        for (int i = 0; i < entities.Length; i++) {
            entities[i] = GameObject.Instantiate(EntityPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), transform).GetComponent<Entity>();
            entities[i].InitializeEntity(entitiesInfos[i]);
            terrain.PlaceEntity(entities[i]);
        }

        entities = entities.SortByInitiative();
        activeEntity = 0;
    }

    public Entity GetEntityAtPosition(Position pos) {
        return entities.GetEntityByPosition(pos);
    }

    private void DeleteAllEntities() {
        if (entities != null) {
            for (int i = 0; i < entities.Length; i++) {
                if (Application.isEditor) {
                    GameObject.DestroyImmediate(entities[i].gameObject);
                } else {
                    GameObject.Destroy(entities[i].gameObject);
                }
            }
        }
        entities = null;
    }
}