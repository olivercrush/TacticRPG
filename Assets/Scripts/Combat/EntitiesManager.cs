using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    public GameObject EntityPrefab;
    private Entity[] entities;
    private int activeEntity = 0;

    public Entity GetEntityAtPosition(Position pos) {
        return entities.GetEntityByPosition(pos);
    }

    public Entity[] GetAllEntities() {
        return entities;
    }

    public Entity GetActiveEntity() {
        return entities[activeEntity];
    }

    public void IncrementActiveEntity() {
        if (++activeEntity >= entities.Length) {
            activeEntity = 0;
        }
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
            GameObject.FindObjectOfType<TerrainManager>().PlaceEntity(entities[i]);
        }
        entities = entities.SortByInitiative();
    }

    public void DeleteEntity(Entity entity) {
        int count = 0;
        Entity[] updatedEntities = new Entity[entities.Length - 1];
        for (int i = 0; i < entities.Length; i++) {
            if (entities[i].infos.id != entity.infos.id) {
                updatedEntities[count] = entities[i];
                count++;
            }
        }

        entities = updatedEntities;
        GameObject.Destroy(entity.gameObject);
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