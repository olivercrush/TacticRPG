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

    public static int GetEntityOrder(this Entity[] array, Entity entity) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i].infos.id == entity.infos.id) {
                return i;
            }
        }
        return -1;
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

    public static EntityCount GetEntityCount(this Entity[] array) {
        int total = 0, red = 0, blue = 0;
        for (int i = 0; i < array.Length; i++) {
            total++;
            if (array[i].infos.team == Team.RED) red++;
            else if (array[i].infos.team == Team.BLUE) blue++;
        }

        return new EntityCount(total, red, blue);
    }
}

public struct EntityCount {
    public int total;
    public int red;
    public int blue;

    public EntityCount(int total, int red, int blue) {
        this.total = total;
        this.red = red;
        this.blue = blue;
    }
}