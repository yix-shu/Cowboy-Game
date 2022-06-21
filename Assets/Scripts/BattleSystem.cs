using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, TURN, LOST, WON}
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public Text dialogueText;
    public AmmoDisplay ammoDisplay;

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
    IEnumerator PlayerShoot()
    {
        playerUnit.reloaded = false;
        dialogueText.text = playerUnit.unitName + " shoots " + enemyUnit.unitName;

        //check if enemy reloaded during this turn

        bool isDead = enemyUnit.TakeDamage(1);

        enemyHUD.updateHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(3f);

        //Check if the enemy has died
        if (isDead)
        {
            //End battle
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //New turn 
            yield return new WaitForSeconds(3f);

            state = BattleState.TURN;
            SimultaneousTurn();
        }

        //Change state based on what has occurred

    }
    IEnumerator PlayerReload()
    {
        dialogueText.text = playerUnit.unitName + " shoots " + enemyUnit.unitName;
        ammoDisplay.updateBullets();
        //check if enemy shot during this turn

    }
    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the duel!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You have lost the duel.";
        }
    }
    void SimultaneousTurn()
    {
        dialogueText.text = "Choose a move:"; //can change this to "CHOOSE" later

    }
    public void OnShootButton()
    {
        if (state != BattleState.TURN) 
        {
            return;
        }
        StartCoroutine(PlayerShoot());
    }
}
