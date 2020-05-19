using System.Collections;
using System.Collections.Generic;

public static class CharacterArrayExtension {

    public static Character[] SortByInitiative(this Character[] array) {
        for (int i = 0; i < array.Length; i++) {
            for (int j = i + 1; j < array.Length; j++) {
                if (array[j].infos.initiative > array[i].infos.initiative) {
                    Character tmp = array[i];
                    array[i] = array[j];
                    array[j] = tmp;
                }
            }
        }

        return array;
    }

    public static Character GetCharacterByPosition(this Character[] array, Position pos) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i].infos.position.x == pos.x && array[i].infos.position.y == pos.y) {
                return array[i];
            }
        }
        return null;
    }
}