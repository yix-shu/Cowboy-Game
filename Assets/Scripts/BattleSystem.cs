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

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleLoc);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleLoc);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName + " has challenged you";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3f); //a bit of break before the round begins

        state = BattleState.TURN;
        SimultaneousTurn();
    }
    void SimultaneousTurn()
    {
        dialogueText.text = "Choose a move:"; //can change this to "CHOOSE" later

    }
    void OnAttackButton()
    {

    }
}
