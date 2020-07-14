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
    /// <returns>An entity - The active entity who has to make actions</returns>
    public Entity GetActiveEntity() {
        if (activeEntity >= entities.Length)
            return entities[0];
        return entities[activeEntity];
    }

    /// <summary>
    /// Shift the active entity to the next in line
    /// </summary>
    public void IncrementActiveEntity() {
        do {
            if (++activeEntity >= entities.Length) {
                activeEntity = 0;
            }
        } while (entities[activeEntity].infos.dead);
    }

    // TODO : Make these update method not take in parameters entities, instead make a system of calls with entity ids and changed characteristics

    /// <summary>
    /// Update the manager's entities with the provided updated entities
    /// </summary>
    /// <param name="updatedEntities">An array of updated entities to be sync with the manager</param>
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

    // TODO : Same that the above TODO comment

    /// <summary>
    /// Update the manager's entity with the provided updated entity
    /// </summary>
    /// <param name="updatedEntity">An updated entity to be sync with the manager</param>
    public void UpdateEntity(Entity updatedEntity) {
        UpdateEntities(new Entity[] { updatedEntity });
    }

    /// <summary>
    /// Returns the count of total players and team players
    /// </summary>
    /// <returns>An EntityCount struct - A data structure that represents the count of total players and team players</returns>
    public EntityCount GetEntityCount() {
        return entities.GetEntityCount();
    }

    /// <summary>
    /// Creates an array of entities based on a provided array of EntityInfo struct and initialize each one of the entities
    /// </summary>
    /// <param name="infos">An array of EntityInfo struct</param>
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

    // TODO : Refactor the behaviour of this + change the name to RemoveEntity to better grasp the method's responsibility

    /// <summary>
    /// Remove an entity from combat
    /// </summary>
    /// <param name="entity">The entity we want to remove from combat</param>
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

    /// <summary>
    /// Delete all the manager's entities
    /// </summary>
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

    /// <summary>
    /// Check an entity for potential events to apply based on his infos
    /// </summary>
    /// <param name="entity">The entity we want to check for potential events to apply</param>
    public void CheckEntity(Entity entity) {
        if (entity.infos.lifePoints <= 0) {
            entity.Die();
            DeleteEntity(entity);
        }
    }
}