using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EntitiesManager is a tier-2 component
/// Responsibility : manages the entities and the active system
/// </summary>
public class EntitiesManager : MonoBehaviour
{
    public GameObject EntityPrefab;
    public Entity[] entities;
    private int activeEntity = 0;

    /// <summary>
    /// Returns the entity that is at a specified position
    /// </summary>
    /// <param name="pos">The position at which the entity is</param>
    /// <returns>An entity</returns>
    public Entity GetEntityAtPosition(Position pos) {
        return entities.GetEntityByPosition(pos);
    }

    /// <summary>
    /// Returns all entities
    /// </summary>
    /// <returns>An array of entities</returns>
    public Entity[] GetAllEntities() {
        return entities;
    }

    /// <summary>
    /// Returns the active entity
    /// </summary>
    /// <returns>An entity</returns>
    public Entity GetActiveEntity() {
        if (activeEntity >= entities.Length)
            return entities[0];
        return entities[activeEntity];
    }

    public void IncrementActiveEntity() {
        do {
            if (++activeEntity >= entities.Length) {
                activeEntity = 0;
            }
        } while (entities[activeEntity].infos.dead);
    }

    public void UpdateEntities(Entity[] updatedEntities) {
        for (int i = 0; i < updatedEntities.Length; i++) {
            for (int j = 0; j < entities.Length; j++) {
                if (updatedEntities[i].infos.id == entities[j].infos.id) {
                    entities[j].infos = updatedEntities[i].infos;
                    CheckEntity(entities[j]);
                }
            }
        }
    }

    public void UpdateEntity(Entity updatedEntity) {
        UpdateEntities(new Entity[] { updatedEntity });
    }

    public EntityCount GetEntityCount() {
        return entities.GetEntityCount();
    }

    public void InitializeEntities(EntityInfo[] infos) {
        DeleteEntities();
        entities = new Entity[infos.Length];
        for (int i = 0; i < entities.Length; i++) {
            entities[i] = GameObject.Instantiate(EntityPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), transform).GetComponent<Entity>();
            entities[i].InitializeEntity(infos[i]);
            entities[i].name = entities[i].infos.name;
            GameObject.FindObjectOfType<TerrainManager>().PlaceEntity(entities[i]);
        }
        entities = entities.SortByInitiative();
    }

    public void DeleteEntity(Entity entity) {
        // check if dead entity is active entity
        bool turnIsOver = false;
        if (entity.infos.id == entities[activeEntity].infos.id) {
            turnIsOver = true;
        }

        entity.infos.dead = true;
        //GameObject.Destroy(entity.gameObject);

        if (turnIsOver) {
            GameObject.FindObjectOfType<CombatManager>().EndTurn();
        }
    }

    public void DeleteEntities() {
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

    public void CheckEntity(Entity entity) {
        if (entity.infos.lifePoints <= 0) {
            entity.Die();
            DeleteEntity(entity);
        }
    }
}