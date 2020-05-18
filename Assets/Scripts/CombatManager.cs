using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public bool autoUpdate;
    public CharacterInfo[] charactersInfos;

    private Terrain terrain;
    private Character[] characters;
    private int activeCharacter;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        terrain = GetComponentInChildren<Terrain>();
        terrain.GenerateTerrain();

        GenerateCharacters();
        UpdateCombat();
    }

    public void MoveActiveCharacter(Position pos) {
        characters[activeCharacter].infos.position = pos;
        terrain.MoveCharacter(characters[activeCharacter]);
        
        UpdateCombat();
    }

    public void GenerateCharacters() {
        DeleteAllCharacters();
        characters = new Character[charactersInfos.Length];
        for (int i = 0; i < characters.Length; i++) {
            characters[i] = GameObject.Instantiate(characterPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), transform).GetComponent<Character>();
            characters[i].InitializeCharacter(charactersInfos[i]);
            terrain.PlaceCharacter(characters[i]);
        }

        for (int i = 0; i < characters.Length; i++) {
            for (int j = i + 1; j < characters.Length; j++) {
                if (characters[j].infos.initiative > characters[i].infos.initiative) {
                    Character tmp = characters[i];
                    characters[i] = characters[j];
                    characters[j] = tmp;
                }
            }
        }

        activeCharacter = -1;
    }

    public void UpdateCombat() {
        
        activeCharacter++;
        if (activeCharacter >= characters.Length) {
            activeCharacter = 0;
        }

        //print("New turn : " + activeCharacter);

        characters[activeCharacter].CenterCamera();
        //terrain.CreateMovementTiles(characters[activeCharacter].infos, characters);
        terrain.CreateAttackTiles(characters[activeCharacter].infos);

        if (VerifyEndOfCombat()) {
            print("End of battle");
        }
    }

    public bool VerifyEndOfCombat() {

        int blueCount = 0;
        int redCount = 0;

        for (int i = 0; i < characters.Length; i++) {
            if (characters[i].infos.team == Team.RED) {
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

    private void DeleteAllCharacters() {
        if (characters != null) {
            for (int i = 0; i < characters.Length; i++) {
                if (Application.isEditor) {
                    GameObject.DestroyImmediate(characters[i].gameObject);
                } else {
                    GameObject.Destroy(characters[i].gameObject);
                }
            }
        }
        characters = null;
    }
}
