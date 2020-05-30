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

    public static void UpdatePosition(this Entity[] array, Entity entity, Position pos) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == entity) {
                array[i].infos.position = pos;
            }
        }
    }

    public static void UpdateLifepoints(this Entity[] array, Entity entity, int lifepoints) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == entity) {
                array[i].infos.lifePoints = lifepoints;
            }
        }
    }
}