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
    private TurnStatus turnStatus;

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
        UpdateCombat();
    }

    public void StartAction(ActionType actionType) {
        if (actionType == ActionType.MOVE && !turnStatus.move)
            terrain.CreateMovementTiles(entities[activeEntity], entities);
        else if (actionType == ActionType.ATTACK && !turnStatus.attack)
            terrain.CreateAttackTiles(entities[activeEntity]);
    }

    public void MoveEntity(Entity entity, Position pos) {
        entities.UpdatePosition(entity, pos);
        terrain.MoveEntity(entity);
        turnStatus.move = true;
        UpdateCombat();
    }

    public void AttackEntity(Entity entity, int lifepoints) {
        entities.UpdateLifepoints(entity, lifepoints);
        turnStatus.attack = true;
        UpdateCombat();
    }

    public void CleanActions() {
        terrain.DeleteActionTiles();
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

    public void UpdateCombat() {
        if (turnStatus.IsTurnOver()) {
            activeEntity++;
            if (activeEntity >= entities.Length) {
                activeEntity = 0;
            }

            turnStatus.ResetStatus();
        }
        
        terrain.DeleteActionTiles();
        entities[activeEntity].CenterCamera();

        if (VerifyEndOfCombat()) {
            print("End of battle");
        }
    }

    public Entity GetEntityAtPosition(Position pos) {
        return entities.GetEntityByPosition(pos);
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

public struct TurnStatus {
    public bool move;
    public bool attack;

    public void ResetStatus() {
        move = false;
        attack = false;
    }

    public bool IsTurnOver() {
        return (move && attack);
    }
}