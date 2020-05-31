using System.Collections;
using System.Collections.Generic;

public static class EntityArrayExtension {

    public static Entity[] SortByInitiative(this Entity[] array) {
        for (int i = 0; i < array.Length; i++) {
            for (int j = i + 1; j < array.Length; j++) {
                if (array[j].infos.initiative > array[i].infos.initiative) {
                    Entity tmp = array[i];
                    array[i] = array[j];
                    array[j] = tmp;
                }
            }
        }

        return array;
    }

    public static Entity GetEntityByPosition(this Entity[] array, Position pos) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i].infos.position.x == pos.x && array[i].infos.position.y == pos.y) {
                return array[i];
            }
        }
        return null;
    }

    public static void UpdateEntities(this Entity[] array, Entity[] updatedEntities) {
        for (int i = 0; i < array.Length; i++) {
            for (int j = 0; j < updatedEntities.Length; j++) {
                if (array[i].infos.id == updatedEntities[j].infos.id) {
                    array[i] = updatedEntities[j];
                }
            }
        }
    }
}