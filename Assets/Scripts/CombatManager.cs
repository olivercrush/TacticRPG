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

        GenerateCharacters();
        UpdateCombat();
    }

    public void StartAction(ActionType actionType) {
        if (actionType == ActionType.MOVE && !turnStatus.move)
            terrain.CreateMovementTiles(characters[activeCharacter].infos, characters);
        else if (actionType == ActionType.ATTACK && !turnStatus.attack)
            terrain.CreateAttackTiles(characters[activeCharacter].infos);
    }

    public void MoveActiveCharacter(Position pos) {
        characters[activeCharacter].infos.position = pos;
        terrain.MoveCharacter(characters[activeCharacter]);
        turnStatus.move = true;
        UpdateCombat();
    }

    public void AttackCharacter(CharacterInfo info, Position position) {
        Character attackedCharacter = characters.GetCharacterByPosition(position);
        attackedCharacter.ReceiveAttack(info.attackPoints);
        turnStatus.attack = true;
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

        characters = characters.SortByInitiative();
        activeCharacter = 0;
    }

    public void UpdateCombat() {
        if (turnStatus.IsTurnOver()) {
            activeCharacter++;
            if (activeCharacter >= characters.Length) {
                activeCharacter = 0;
            }

            turnStatus.ResetStatus();
        }
        
        terrain.DeleteActionTiles();
        characters[activeCharacter].CenterCamera();

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

public enum ActionType {
    MOVE,
    ATTACK
}