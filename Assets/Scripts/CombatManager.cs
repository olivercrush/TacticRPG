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
        for (int i = 0; i < characters.Length; i++) {
            if (characters[i].infos.active) {
                characters[i].infos.position = pos;
                terrain.MoveCharacter(characters[i]);
            }
        }
        
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
    }

    public void UpdateCombat() {
        for (int i = 0; i < characters.Length; i++) {
            Character character = characters[i].GetComponent<Character>();
            if (character.infos.active) {
                character.CenterCamera();
                terrain.HighlightCharacterMovement(character.infos);
            }
        }
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
