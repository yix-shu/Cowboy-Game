using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, TURN, LOST, WON}
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public Text dialogueText;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleLoc;
    public Transform enemyBattleLoc;

    Unit playerUnit;
    Unit enemyUnit;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleLoc);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleLoc);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName + "has challenged you";
    }
}
